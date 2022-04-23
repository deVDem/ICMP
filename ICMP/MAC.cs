using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ICMP
{
    public static class MAC
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

        public static string ConvertIpToMAC(string ip)
        {
            IPAddress ip_adr = IPAddress.Parse(ip);
            byte[] ab = new byte[6];
            int len = ab.Length;
            int r = SendARP(ip_adr.GetHashCode(), 0, ab, ref len);
            return BitConverter.ToString(ab, 0, 6);
        }
    }
}
