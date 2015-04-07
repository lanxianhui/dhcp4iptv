# Description #
Some (Dutch) IPTV providers offering an Amino set-top box use dynamic IP assignment. This is not handy for people who use a media center, since the DHCP request/response is specific for the STB/provider. Therefor, it is impossible to use the Windows DHCP client. This project implements a Windows DHCP client for users of an Amino STB.

# Prerequisites for running DHCP4IPTV #
You need to install:
  * [.Net Framework 4](http://tinyurl.com/2wc8ulb)
  * [WinPcap](http://www.winpcap.org/)

**Run the tool with Administrator privileges**

# Network card setup #
Prior to running DHCP4IPTV, you need to change the MAC address of the card you will use to connect to the IPTV network. You must change it to the MAC address of you Amino set-top box. You can find instructions on how to modify your MAC address on [this page](http://www.online-tech-tips.com/computer-tips/how-to-change-mac-address/).

The subnet mask of the network needs to be adjusted to the mask used by the IPTV provider. In The Netherlands, subnet mask 255.255.248.0 is used by Glashart (OnsBrabantNet, OnsNet, XMS, ...)

# Screenshot #
![http://www.vandenmuyzenberg.nl/images/dhcp4iptv.jpg](http://www.vandenmuyzenberg.nl/images/dhcp4iptv.jpg)