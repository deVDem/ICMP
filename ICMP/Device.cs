
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ICMP
{
    public class Device
    {
        public EndPoint ipAddress;
        public string ip;
        public bool pinged;

        private HelperICMP helper;


        public Device(string ip)
        {
            this.ip = ip;
            ipAddress = new IPEndPoint(IPAddress.Parse(ip), 0);
            helper = new HelperICMP(ipAddress);
        }

        public void StartPing()
        {
            for (int i = 0; i < 5; i++)
            {
                pinged = helper.StartPing(ip);
                if (pinged) break;
            }

            System.Console.WriteLine(". Result: "+pinged);
        }


        public int[] getStartOctets()
        {
            int[] octets = new int[3];
            string[] octetsStr = ip.Split('.');
            for(int i = 0; i < 3; i++)
            {
                octets[i] = Convert.ToInt32(octets[i]);
            }
            return octets;
        }
    }
}
