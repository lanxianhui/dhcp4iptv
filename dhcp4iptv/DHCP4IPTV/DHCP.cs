using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

using System.Windows.Forms;

using SharpPcap;
using PacketDotNet;

namespace DHCP4IPTV
{
    enum DHCPMessageType
    {
        DHCPDISCOVER = 1,
        DHCPOFFER,
        DHCPREQUEST,
        DHCPDECLINE,
        DHCPACK,
        DHCPNAK,
        DHCPRELEASE,
        DHCPINFORM
    }

    struct DHCPRequest
    {
        public byte opcode;
        public byte htype;
        public byte hlen;
        public byte hops;
        public UInt32 xid;
        public UInt16 secs;
        public UInt16 fill;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] ciaddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] yiaddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] siaddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] giaddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] chaddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] sname;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] file;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 130)]
        public byte[] vend;
    }

    enum DHCPState
    {
        INIT = 0,
        DISCOVER = 1,
        REQUEST = 2,
        BOUND = 3
    }

    class DHCP
    {
        private static readonly byte[] m_BroadcastAddr = { 255, 255, 255, 255};
        private string m_strVendorClassID;

        private Thread m_TimerThread = null;
        private Thread m_FilterThread = null;

        private bool m_bStop = false;

        private byte[] m_byMAC;
        private byte[] m_byDecoder;

        private string m_strNIC;

        DateTime m_dtBound = DateTime.MaxValue;
        TimeSpan m_spanLease = new TimeSpan(0);

        private UInt32 m_dwServerID;
        private UInt32 m_dwRequestedIP;
        private UInt32 m_dwLeaseTime;
        private UInt32 m_dwSubnetMask;

        private LivePcapDevice m_Device = null;

        private IStatusUpdate m_IStatusUpdate;

        private DHCPState m_State = DHCPState.INIT;

        static DHCP m_Instance;

        public void Connect(IStatusUpdate IStatusUpdate)
        {
            m_State = DHCPState.INIT;
            m_bStop = false;
            m_IStatusUpdate = IStatusUpdate;
            m_Instance = this;
            m_Device = null;
            m_dtBound = DateTime.MaxValue;

            m_FilterThread = new Thread(new ThreadStart(StartFilter));
            m_FilterThread.Start();

            m_TimerThread = new Thread(new ThreadStart(TimerHandler));
            m_TimerThread.Start();
        }

        private void StartFilter()
        {
            while (!m_bStop)
            {
                try
                {
                    LivePcapDeviceList devices = null;
                    devices = LivePcapDeviceList.Instance;

                    int i = 0;
                    /* Scan the list printing every entry */
                    foreach (LivePcapDevice dev in devices)
                    {
                        if (dev.Description.ToString() == m_strNIC)
                            m_Device = devices[i];
                        break;
                    }
                
                    //Open the device for capturing
                    int readTimeoutMilliseconds = 1000;
                    m_Device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

                    //Register our handler function to the 'packet arrival' event
                    m_Device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

                    // udpdump filter to capture only UDP/IP packets
                    string filter = "udp";
                    m_Device.SetFilter(filter);

                    if (m_dtBound != DateTime.MaxValue)
                    {
                        m_IStatusUpdate.UpdateStatus("Next update at " + (m_dtBound + m_spanLease).ToString());
                    }
                    else
                    {
                        m_IStatusUpdate.UpdateStatus("Started DHCP Client...");
                    }
                    // Start capture packets
                    m_Device.Capture();
                    // NO stop request...
                    if (!m_bStop)
                    {
                        if (m_Device != null)
                        {
                            m_Device.Close();
                            m_Device = null;
                        }
                    }
                }
                catch (Exception exc)
                {
                    m_IStatusUpdate.UpdateStatus("Exception: " + exc.Message);
                    try
                    {
                        m_Device.Close();
                    }
                    catch (Exception)
                    { }
                    m_Device = null;
                }
                Thread.Sleep(1000);
            } 
        }

        public void StopReceive()
        {
            // First set a stop condition
            // This forces the Timer Handler to terminate
            m_bStop = true;

            try
            {
                // Force the device to stop
                if (m_Device != null)
                {
                    m_Device.StopCapture();
                    m_Device.Close();
                }

                m_IStatusUpdate.UpdateStatus("Waiting for DHCP client to stop...");

                m_FilterThread.Join();
                m_TimerThread.Join();

                m_FilterThread = null;
                m_TimerThread = null;

                m_IStatusUpdate.UpdateStatus("DHCP Client stopped");
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var time = e.Packet.Timeval.Date;
            if ( e.Packet.Data.Length > 400 )
                m_Instance.HandleResponse(e.Packet.Data);
        }

        private void TimerHandler()
        {
            DHCPState previousState = m_State;
            
            while (!m_bStop)
            {
                switch (m_State)
                {
                    case DHCPState.INIT:
                        previousState = DHCPState.INIT;
                        SendDiscover();
                        break;
                    case DHCPState.DISCOVER:
                        m_State = DHCPState.INIT;
                        previousState = DHCPState.DISCOVER;
                        break;
                    case DHCPState.BOUND:
                        if (previousState != m_State)
                        {
                            previousState = DHCPState.BOUND;

                            byte[] temp = BitConverter.GetBytes(m_dwLeaseTime);
                            Array.Reverse(temp);
                            int dwLeaseTime = BitConverter.ToInt32(temp,0);
                            
                            m_spanLease = new TimeSpan(0, 0, dwLeaseTime/2);
                            m_dtBound = DateTime.Now;
                        }
                        if (DateTime.Now.Subtract(m_dtBound) > m_spanLease)
                        {
                            m_State = DHCPState.INIT;
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(5000);
            }
        }

        private void SendDiscover()
        {
            try
            {
                m_IStatusUpdate.UpdateStatus("Assembling DHCP Discover Packet");
                DHCPRequest DhcpRequest = new DHCPRequest();
                AssembleDiscover(ref DhcpRequest);

                string destHwAddress = "FFFFFFFFFFFF";
                var dest = System.Net.NetworkInformation.PhysicalAddress.Parse(destHwAddress);
                EthernetPacket packet = new EthernetPacket(m_Device.Interface.MacAddress, dest, EthernetPacketType.IpV4);
                packet.Type = EthernetPacketType.IpV4;

                var ipSourceAddress = System.Net.IPAddress.Parse("0.0.0.0");
                var ipDestinationAddress = System.Net.IPAddress.Parse("255.255.255.255");
                var ipPacket = new IPv4Packet(ipSourceAddress, ipDestinationAddress);

                UdpPacket udpPacket = new UdpPacket(67, 68);
                byte[] payload = StructureToByteArray(DhcpRequest);

                ipPacket.Version = IpVersion.IPv4;

                udpPacket.PayloadData = payload;
                ipPacket.PayloadPacket = udpPacket;
                packet.PayloadPacket = ipPacket;

                udpPacket.UpdateUDPChecksum();
                ipPacket.UpdateIPChecksum();

                m_IStatusUpdate.UpdateStatus("Sending DHCP Discover");
                m_Device.SendPacket(packet);
                m_IStatusUpdate.UpdateStatus("Sent DHCP Discover");
            }
            catch ( Exception )
            {
                m_IStatusUpdate.UpdateStatus("Failed to send DHCP Discover");
            }
        }

        private void SendRequest()
        {
            try
            {
                m_IStatusUpdate.UpdateStatus("Assembling DHCP Request Packet");
                DHCPRequest DhcpRequest = new DHCPRequest();
                AssembleRequest(ref DhcpRequest);

                string destHwAddress = "FFFFFFFFFFFF";
                var dest = System.Net.NetworkInformation.PhysicalAddress.Parse(destHwAddress);
                EthernetPacket packet = new EthernetPacket(m_Device.Interface.MacAddress, dest, EthernetPacketType.IpV4);
                packet.Type = EthernetPacketType.IpV4;

                var ipSourceAddress = System.Net.IPAddress.Parse("0.0.0.0");
                var ipDestinationAddress = System.Net.IPAddress.Parse("255.255.255.255");
                var ipPacket = new IPv4Packet(ipSourceAddress, ipDestinationAddress);

                UdpPacket udpPacket = new UdpPacket(67, 68);
                byte[] payload = StructureToByteArray(DhcpRequest);

                ipPacket.Version = IpVersion.IPv4;

                udpPacket.PayloadData = payload;
                ipPacket.PayloadPacket = udpPacket;
                packet.PayloadPacket = ipPacket;

                udpPacket.UpdateUDPChecksum();
                ipPacket.UpdateIPChecksum();

                m_IStatusUpdate.UpdateStatus("Sending DHCP Discover");
                m_Device.SendPacket(packet);
                m_IStatusUpdate.UpdateStatus("Sent DHCP Discover");
            }
            catch (Exception)
            {
                m_IStatusUpdate.UpdateStatus("Failed to send DHCP Request");
            }
        }

        #region OLD CODE
        //private void OLD_SendSingleRequest()
        //{
        //    try
        //    {
        //        DHCPRequest DhcpRequest = new DHCPRequest();
        //        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        //        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

        //        IPAddress IP;
        //        NetworkManagement networkMgmt = new NetworkManagement();
        //        networkMgmt.getIP(m_strNIC, out IP);

        //        AssembleDiscover(ref DhcpRequest);
        //        socket.Bind(new IPEndPoint(IP, 68));
        //        socket.Connect(new IPEndPoint(new IPAddress(m_BroadcastAddr), 67));
        //        socket.Send(StructureToByteArray(DhcpRequest));
        //        m_IStatusUpdate.UpdateStatus("Sent DHCP Discover");
        //        m_State = DHCPState.DISCOVER;
        //        socket.Close();
        //    }
        //    catch (Exception exc)
        //    {
        //        MessageBox.Show("Error opening Socket, error " + exc.Message, "Socket error (SendSingleRequest)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}

        //private void OLD_SendAck()
        //{
        //    try
        //    {
        //        DHCPRequest DhcpRequest = new DHCPRequest();
        //        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        //        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

        //        IPAddress IP;
        //        NetworkManagement networkMgmt = new NetworkManagement();
        //        networkMgmt.getIP(m_strNIC, out IP);
        //        AssembleAck(ref DhcpRequest);
        //        socket.Bind(new IPEndPoint(IP, 68));
        //        socket.Connect(new IPEndPoint(new IPAddress(m_BroadcastAddr), 67));
        //        socket.Send(StructureToByteArray(DhcpRequest));
        //        m_IStatusUpdate.UpdateStatus("Sent DHCP Request");
        //        m_State = DHCPState.REQUEST;
        //        socket.Close();
        //    }
        //    catch (Exception exc)
        //    {
        //        MessageBox.Show("Error opening Socket, error " + exc.Message, "Socket error (SendAck)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //public void OLD_StartReceive()
        //{
        //    try
        //    {
        //        m_ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //        m_ReceiveSocket.Bind(new IPEndPoint(IPAddress.Any, 68));

        //        m_ReceiveThread = new Thread(new ThreadStart(ReceiveResponse));
        //        m_ReceiveThread.Start();
        //    }
        //    catch (Exception exc)
        //    {
        //        MessageBox.Show("Error opening Socket, error " + exc.Message, "Socket error (StartReceive)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}

        //public void OLD_StopReceive()
        //{
        //    m_bStop = true;
        //    if (m_ReceiveThread != null)
        //    {
        //        if (m_ReceiveSocket != null)
        //        {
        //            m_ReceiveSocket.Close();
        //            m_ReceiveSocket = null;
        //        }
        //    }
        //}

        //private void OLD_ReceiveResponse()
        //{
        //    while (!m_bStop)
        //    {
        //        try
        //        {
        //            SubTest();
        //            byte[] buffer = new byte[1500];
        //            m_ReceiveSocket.Receive(buffer, SocketFlags.None);
        //            HandleResponse(buffer);
        //        }
        //        catch (Exception exc)
        //        {
        //            if (!m_bStop)
        //                MessageBox.Show("Error while starting receive: " + exc.Message, "Start Receive Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
        #endregion // OLD CODE

        private void GetOfferData(ref byte[] buffer)
        {
            int idx = 0xec +42;
            byte byOption;
            byte byLength;
            
            if (0x63538263 == BitConverter.ToUInt32(buffer, idx))
            {
                idx += 4; // skip cookie

                m_dwRequestedIP = BitConverter.ToUInt32(buffer, 0x10+42);

                while (idx < buffer.Length && buffer[idx]!=0)
                {
                    byOption = buffer[idx++];
                    byLength = buffer[idx++];

                    switch (byOption)
                    {
                        case 1: // subnet mask
                            m_dwSubnetMask = BitConverter.ToUInt32(buffer, idx);
                            idx += 4;
                            break;
                        case 51: // lease time
                            m_dwLeaseTime = BitConverter.ToUInt32(buffer, idx);
                            idx += 4;
                            break;
                        case 54: // server ID
                            m_dwServerID = BitConverter.ToUInt32(buffer, idx);
                            idx += 4;
                            break;
                        default:
                            idx += (int)byLength;
                            break;
                    }
                }
            }
        }

        private void HandleResponse(byte[] buffer)
        {
            if (buffer[0Xf0 +42 ] == 0x35 && buffer[0xf2+42] == 0x2) // Offer
            {
                m_IStatusUpdate.UpdateStatus("Received DHCP Offer");
                GetOfferData(ref buffer);
                SendRequest();
            }
            else if (buffer[0Xf0+42] == 0x35 && buffer[0xf2+42] == 0x5) // ACK
            {
                string strIP = IPToString(m_dwRequestedIP);
                string strSubnetMask = IPToString(m_dwSubnetMask);
                m_IStatusUpdate.UpdateStatus("Received DHCP ACK with IP " + strIP);
                NetworkManagement networkMgmt = new NetworkManagement();
                networkMgmt.setIP(m_strNIC, strIP, strSubnetMask);
                m_State = DHCPState.BOUND;
            }
        }

        private string IPToString(UInt32 dwIP)
        {
            string strIP;
            strIP = ((byte)(dwIP & 0x000000ff)).ToString() + ".";
            strIP += ((byte)((dwIP & 0x0000ff00) >> 8)).ToString() + ".";
            strIP += ((byte)((dwIP & 0x00ff0000) >> 16)).ToString() + ".";
            strIP += ((byte)((dwIP & 0xff000000) >> 24)).ToString();

            return strIP;
        }

        private void AssembleDiscover(ref DHCPRequest DhcpRequest)
        {
            DhcpRequest.opcode = (byte)DHCPMessageType.DHCPDISCOVER;
            DhcpRequest.htype = 1;
            DhcpRequest.hlen = 6;
            DhcpRequest.hops = 0;
            DhcpRequest.secs = 0x0a00;
            DhcpRequest.xid = 0xe4b0b618;
            DhcpRequest.chaddr = m_byMAC;
            DhcpRequest.fill = 0;

            DhcpRequest.ciaddr = new byte[4];
            DhcpRequest.yiaddr = new byte[4];
            DhcpRequest.siaddr = new byte[4];
            DhcpRequest.giaddr = new byte[4];

            DhcpRequest.sname = new byte[64];
            DhcpRequest.file = new byte[128];

            DhcpRequest.vend = new byte[130];
            int idx = 0;

            AddMagicCookie(ref DhcpRequest.vend, ref idx);
            AddDHCPMessageType(ref DhcpRequest.vend, ref idx, DHCPMessageType.DHCPDISCOVER);
            AddMaxDHCPMessageSize(ref DhcpRequest.vend, ref idx);
            AddOptionLeaseTime(ref DhcpRequest.vend, ref idx, UInt32.MaxValue);
            AddParameterRequestList(ref DhcpRequest.vend, ref idx);
            AddVendorSpecificInfo(ref DhcpRequest.vend, ref idx);
            AddVendorClassID(ref DhcpRequest.vend, ref idx);
            AddVendorClientID(ref DhcpRequest.vend, ref idx);
        }

        private void AssembleRequest(ref DHCPRequest DhcpRequest)
        {
            DhcpRequest.opcode = (byte)DHCPMessageType.DHCPDISCOVER;
            DhcpRequest.htype = 1;
            DhcpRequest.hlen = 6;
            DhcpRequest.hops = 0;
            DhcpRequest.secs = 0x0a00;
            DhcpRequest.xid = 0xe4b0b618;
            DhcpRequest.chaddr = m_byMAC;
            DhcpRequest.fill = 0;

            DhcpRequest.ciaddr = new byte[4];
            DhcpRequest.yiaddr = new byte[4];

            DhcpRequest.siaddr = new byte[4];
            DhcpRequest.giaddr = new byte[4];
            DhcpRequest.sname = new byte[64];
            DhcpRequest.file = new byte[128];

            DhcpRequest.vend = new byte[130];
            int idx = 0;

            AddMagicCookie(ref DhcpRequest.vend, ref idx);
            AddDHCPMessageType(ref DhcpRequest.vend, ref idx, DHCPMessageType.DHCPREQUEST);
            AddMaxDHCPMessageSize(ref DhcpRequest.vend, ref idx);
            AddDHCPServerID(ref DhcpRequest.vend, ref idx, m_dwServerID);
            AddRequestedIP(ref DhcpRequest.vend, ref idx, m_dwRequestedIP);
            AddOptionLeaseTime(ref DhcpRequest.vend, ref idx, m_dwLeaseTime);
            AddParameterRequestList(ref DhcpRequest.vend, ref idx);
            AddVendorSpecificInfo(ref DhcpRequest.vend, ref idx);
            AddVendorClassID(ref DhcpRequest.vend, ref idx);
            AddVendorClientID(ref DhcpRequest.vend, ref idx);
        }

        private void AddMagicCookie(ref byte[] vend, ref int idx)
        {
            vend[idx++] = 0x63;
            vend[idx++] = 0x82;
            vend[idx++] = 0x53;
            vend[idx++] = 0x63;
        }

        private void AddDHCPMessageType(ref byte[] vend, ref int idx, DHCPMessageType type)
        {
            vend[idx++] = 53;
            vend[idx++] = 0x01;
            vend[idx++] = (byte)type;
        }

        private void AddOptionLeaseTime(ref byte[] vend, ref int idx, UInt32 secs)
        {
            vend[idx++] = 51;
            vend[idx++] = 0x4;
            byte[] byLease = BitConverter.GetBytes(secs);

            foreach (byte b in byLease)
            {
                vend[idx++] = b;
            }
        }

        private void AddDHCPServerID(ref byte[] vend, ref int idx, UInt32 ID)
        {
            vend[idx++] = 54;
            vend[idx++] = 0x4;
            byte[] byLease = BitConverter.GetBytes(ID);

            foreach (byte b in byLease)
            {
                vend[idx++] = b;
            }
        }

        private void AddRequestedIP(ref byte[] vend, ref int idx, UInt32 IP)
        {
            vend[idx++] = 50;
            vend[idx++] = 0x4;
            byte[] byLease = BitConverter.GetBytes(IP);

            foreach (byte b in byLease)
            {
                vend[idx++] = b;
            }
        }

        private void AddMaxDHCPMessageSize(ref byte[] vend, ref int idx)
        {
            vend[idx++] = 57;
            vend[idx++] = 0x02;
            vend[idx++] = 0x02;
            vend[idx++] = 0x24;
        }

        private void AddParameterRequestList(ref byte[] vend, ref int idx)
        {
            vend[idx++] = 55; // Request list
            vend[idx++] = 0x10; // length
            vend[idx++] = 0x1;  // Subnet
            vend[idx++] = 0x2;  // Time offset
            vend[idx++] = 0x3;  // Router
            vend[idx++] = 0x6;  // DNS
            vend[idx++] = 0x0c; // Host name
            vend[idx++] = 0x0f; // Domain name
            vend[idx++] = 0x11; // Root Path
            vend[idx++] = 0x17; // Default IP Time-to-live;
            vend[idx++] = 0x1c; // Broadcast address
            vend[idx++] = 0x1d; // Perform mask discovery
            vend[idx++] = 0x1f; // Perform router discovery
            vend[idx++] = 0x21; // Static route
            vend[idx++] = 0x28; // Network information service domain
            vend[idx++] = 0x29; // Network information service servers
            vend[idx++] = 0x2a; // Network time protocol servers
            vend[idx++] = 0x2b; // Vendor specific information
        }

        void AddVendorSpecificInfo(ref byte[] vend, ref int idx)
        {
            string strAmino;
            string strfisys = "fisys";

            vend[idx++] = 43;  // Vendor specific information
            vend[idx++] = 0x13;   // length = 19 chars
            vend[idx++] = 0x03;
            vend[idx++] = 0x0a;

            if (m_strVendorClassID == "Aminoaminet130mfisys")
                strAmino = "aminet130m";
            else
                strAmino = "aminet110";

            System.Text.ASCIIEncoding encoding=new System.Text.ASCIIEncoding();
            byte[] byAmino = encoding.GetBytes(strAmino);
            foreach (byte b in byAmino)
                vend[idx++] = b;

            vend[idx++] = 0x04;
            vend[idx++] = 0x05;

            byte[] byfisys = encoding.GetBytes(strfisys);
            foreach (byte b in byfisys)
                vend[idx++] = b;
        }

        void AddVendorClassID(ref byte[] vend, ref int idx)
        {
            vend[idx++] = 60; // Vendor class ID;
            vend[idx++] = (byte)m_strVendorClassID.Length;
            System.Text.ASCIIEncoding encoding=new System.Text.ASCIIEncoding();
            byte[] byVendClassID = encoding.GetBytes(m_strVendorClassID);
            foreach (byte b in byVendClassID)
            {
                vend[idx++] = b;
            }
        }

        void AddVendorClientID(ref byte[] vend, ref int idx)
        {
            vend[idx++] = 61;
            vend[idx++] = 0x07;
            vend[idx++] = 0x01;
            for (int i = 0; i < 6; i++)
                vend[idx++] = m_byMAC[i];
        }

        static byte[] StructureToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public string MAC
        {
            set
            {
                int discarded;
                string strMAC = value + "00000000000000000000";
                m_byMAC = HexEncoding.GetBytes(strMAC, out discarded);
            }
        }

        public string NIC
        {
            set
            {
                m_strNIC = value;
            }
        }

        public string Decoder
        {
            set
            {
                int discarded;

                if ("Amino 130m" == value)
                    m_strVendorClassID = "Aminoaminet130mfisys";
                else if ("Amino 110" == value)
                    m_strVendorClassID = "Aminoaminet110fisys";
                
                m_byDecoder = HexEncoding.GetBytes("0a", out discarded);
            }
        }
    }
}
