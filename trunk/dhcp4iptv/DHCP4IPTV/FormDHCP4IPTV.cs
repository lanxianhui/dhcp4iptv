using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Management;
using System.Net.NetworkInformation;
using System.Net;
using System.Reflection;
using System.Diagnostics;

using SharpPcap;

namespace DHCP4IPTV
{
    interface IStatusUpdate
    {
        void UpdateStatus(string strUpdate);
    }

    public partial class frmDHCP4IPTV : Form, IStatusUpdate
    {
        private DHCP m_DHCP;
        private string m_strKeyName = @"Software\Muyz\DHCP4IPTV";
        private bool m_bInitializing;
        delegate void UpdateStatusCallback(string strStatus);

        public frmDHCP4IPTV()
        {
            InitializeComponent();
            
            m_bInitializing = true;
            GetNICList();
            RetrieveSettings();
            m_bInitializing = false;
            
            UpdateStatus("Idle; only use the MAC address of your set-top box!");

            if (chkStartMinimized.Checked)
                this.WindowState = FormWindowState.Minimized;

            m_DHCP = new DHCP();

            System.Version verCur = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += " v" + verCur.ToString() + " by Muyz ©2010";

            if (chkStartWithWindows.Checked)
                btnStart_Click(null,null);
        }

        private void RetrieveSettings()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            #region StartWithWindows
            if (rkApp.GetValue("DHCP4IPTV") == null)
            {
                chkStartWithWindows.Checked = false;
            }
            else
            {
                chkStartWithWindows.Checked = true;
            }
            #endregion

            RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strKeyName);

            #region NICName
            string strNIC = (string)key.GetValue("NICName");
            cmbNIC.SelectedIndex = 0;
            if (strNIC != null)
            {
                for (int i = 0; i<cmbNIC.Items.Count; i++)
                {
                    if ((string)cmbNIC.Items[i] == strNIC)
                    {
                        cmbNIC.SelectedIndex = i;
                    }
                }
            }
            #endregion

            #region DecoderName
            string strDecoder = (string)key.GetValue("DecoderName");
            cmbDecoder.SelectedIndex = 0;
            if (strDecoder != null)
            {
                for (int i = 0; i < cmbDecoder.Items.Count; i++)
                {
                    if ((string)cmbDecoder.Items[i] == strDecoder)
                    {
                        cmbDecoder.SelectedIndex = i;
                    }
                }
            }
            #endregion

            #region MAC3
            string strMAC3 = (string)key.GetValue("MAC3");
            if (strMAC3 != null)
            {
                txtMAC3.Text = strMAC3;
            }
            #endregion

            #region MAC4
            string strMAC4 = (string)key.GetValue("MAC4");
            if (strMAC4 != null)
            {
                txtMAC4.Text = strMAC4;
            }
            #endregion

            #region MAC5
            string strMAC5 = (string)key.GetValue("MAC5");
            if (strMAC5 != null)
            {
                txtMAC5.Text = strMAC5;
            }
            #endregion

            #region SaveSettings
            chkSaveSettings.Checked = Convert.ToBoolean(key.GetValue("SaveSettings", false));
            #endregion

            #region StartMinimized
            chkStartMinimized.Checked = Convert.ToBoolean(key.GetValue("StartMinimized", false));
            #endregion
        }

        private void SaveSettings()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strKeyName);

            key.SetValue("NICName", cmbNIC.SelectedItem.ToString());
            key.SetValue("DecoderName", cmbDecoder.SelectedItem.ToString());
            key.SetValue("MAC3", txtMAC3.Text);
            key.SetValue("MAC4", txtMAC4.Text);
            key.SetValue("MAC5", txtMAC5.Text);
            key.SetValue("SaveSettings", chkSaveSettings.Checked);
            key.SetValue("StartMinimized", chkStartMinimized.Checked);
        }

        private void DeleteSettings()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strKeyName);

            key.DeleteValue("NICName", false);
            key.DeleteValue("DecoderName", false);
            key.DeleteValue("MAC3", false);
            key.DeleteValue("MAC4", false);
            key.DeleteValue("MAC5", false);
            key.DeleteValue("SaveSettings", false);
            key.DeleteValue("StartMinimized", false);
        }

        private void GetNICList()
        {
            LivePcapDeviceList devices = null;

            try
            {
                devices = LivePcapDeviceList.Instance;

                /* Scan the list printing every entry */
                foreach (LivePcapDevice dev in devices)
                    cmbNIC.Items.Add(dev.Description.ToString());
            }
            catch (Exception)
            {
                UpdateStatus("Error loading network card list");
            }
        }


        #region OLD CODE
        private void OLD_GetNICList()
        {
            NetworkInterface[] LocalInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface Networkcard in LocalInterfaces)
            {
                cmbNIC.Items.Add(Networkcard.Description);
            }

        }
        #endregion // OLD CODE

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            if (this.btnStart.Text == "&Start")
            {
                if (CheckSpecification())
                {
                    m_DHCP.MAC = txtMAC0.Text + txtMAC1.Text + txtMAC2.Text + txtMAC3.Text + txtMAC4.Text + txtMAC5.Text;
                    m_DHCP.Decoder = cmbDecoder.SelectedItem.ToString();

                    m_DHCP.NIC = cmbNIC.SelectedItem.ToString();
                    m_DHCP.Connect((IStatusUpdate)this);
                    DisableEditing(true);
                    this.btnStart.Text = "St&op";
                }
            }
            else
            {
                DisableEditing(false);
                m_DHCP.StopReceive();
                this.btnStart.Text = "&Start";
                UpdateStatus("Idle; only use the MAC address of your set-top box!");
            }
        }

        private void DisableEditing(bool bDisable)
        {
            txtMAC3.Enabled = !bDisable;
            txtMAC4.Enabled = !bDisable;
            txtMAC5.Enabled = !bDisable;
            cmbDecoder.Enabled = !bDisable;
            cmbNIC.Enabled = !bDisable;
            chkSaveSettings.Enabled = !bDisable;
            chkStartWithWindows.Enabled = !bDisable;
            chkStartMinimized.Enabled = !bDisable;
        }

        private bool CheckSpecification()
        {
            bool bResult = true;

            if (cmbNIC.SelectedItem == null)
            {
                lblStatus.Text = "Specify network interface";
                bResult = false;
            }

            if (cmbDecoder.SelectedItem == null)
            {
                lblStatus.Text = "Specify decoder";
                bResult = false;
            }

            if (!CheckMAC())
            {
                lblStatus.Text = "Invalid MAC address";
                bResult = false;
            }

            return bResult;
        }

        private bool CheckMAC()
        {
            bool bResult = true;
            byte bByte;
            bResult &= CheckByte(txtMAC3.Text, out bByte);
            bResult &= CheckByte(txtMAC4.Text, out bByte);
            bResult &= CheckByte(txtMAC5.Text, out bByte);
            return bResult;
        }

        private bool CheckByte(string strByte, out byte bByte)
        {
            bool bResult = false;
            int discarded = 0;
            int nrOfChars = (int)HexEncoding.GetByteCount(strByte);

            bByte = 0;
            if ( nrOfChars == 1 )
            {
                byte[] byteArray = HexEncoding.GetBytes(strByte, out discarded);
                bByte = byteArray[0];
                bResult = true;
            }
            return bResult;
        }

        private void chkStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (chkStartWithWindows.Checked)
            {
                rkApp.SetValue("DHCP4IPTV", Application.ExecutablePath.ToString());
            }
            else
            {
                rkApp.DeleteValue("DHCP4IPTV", false);
            }
        }

        private void chkSaveSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bInitializing)
            {
                if (chkSaveSettings.Checked)
                {
                    SaveSettings();
                }
                else
                {
                    DeleteSettings();
                }
            }
        }

        private void cmbNIC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( chkSaveSettings.Checked )
                SaveSettings();
        }

        private void cmbDecoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkSaveSettings.Checked)
                SaveSettings();
        }

        public void UpdateStatus(string strUpdate)
        {
            if (statusStrip1.InvokeRequired)
            {
                UpdateStatusCallback d = new UpdateStatusCallback(UpdateStatus);
                this.Invoke(d, new object[] { strUpdate });
            }
            else
            {
                DateTime dt = DateTime.Now;
                toolStripStatusLabel1.Text = dt.ToString() + " : " + strUpdate;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("Mailto:muyzthuyz@hotmail.com?Subject=DHCP4IPTV");
        }

        private void chkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSaveSettings.Checked)
                SaveSettings();
        }
    }
}
