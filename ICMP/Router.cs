using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICMP
{
    public class Router : Device
    {
        public List<Device> devices;
        public string ip;

        public Router(string ip) : base(ip)
        {
            this.ip = ip;
            devices = new List<Device>();
            ipAddress = new IPEndPoint(IPAddress.Parse(ip), 0);
        }

        public new void StartPing()
        {
            foreach (Device device in devices)
            {
                Console.Write("Start ping "+device.ip);
                device.StartPing();
            }
        }

    }
}
