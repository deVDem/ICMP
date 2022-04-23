using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace ICMP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Write IP range");
                Console.Write("(ex. 192.168.0.1-192.168.2.255) : ");
                string remoteHostsStr = Console.ReadLine();

                if(remoteHostsStr == "exit")
                {
                    break;
                }


                if (remoteHostsStr == null)
                {
                    Console.WriteLine("No hosts.");
                    continue;
                }
                // 192.168.0.1-192.168.2.255

                string[] remoteHosts = remoteHostsStr.Split('-');

                if (remoteHosts.Length != 2)
                {
                    Console.WriteLine("Not found separator");
                    continue;
                }

                string startAddr = remoteHosts[0];
                string endAddr = remoteHosts[1];

                string[] octets = startAddr.Split('.');

                if (octets.Length != 4)
                {
                    Console.WriteLine("Wrong start IP address");
                    continue;
                }


                int[] startAddrOctets = new int[octets.Length];

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        startAddrOctets[i] = Convert.ToInt32(octets[i]);
                        if (startAddrOctets[i] > 256)
                        {
                            Console.WriteLine("Wrong start IP address");
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                octets = endAddr.Split('.');

                if (octets.Length != 4)
                {
                    Console.WriteLine("Wrong end IP address");
                    continue;
                }


                int[] endAddrOctets = new int[octets.Length];

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        endAddrOctets[i] = Convert.ToInt32(octets[i]);
                        if (endAddrOctets[i] > 256)
                        {
                            Console.WriteLine("Wrong end IP address");
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                List<Device> devices = new List<Device>();
                List<Router> routers = new List<Router>();


                int devicesCount =
                    (endAddrOctets[0] - startAddrOctets[0]) * 255 * 255 * 255 +
                    (endAddrOctets[1] - startAddrOctets[1]) * 255 * 255 +
                    (endAddrOctets[2] - startAddrOctets[2]) * 255 +
                    (endAddrOctets[3] - startAddrOctets[3]);



                int[] lastAddrOctets = startAddrOctets;

                devices.Add(new Device(String.Format("{0}.{1}.{2}.{3}", startAddrOctets[0], startAddrOctets[1], startAddrOctets[2], startAddrOctets[3])));


                for (int i = 0; i < devicesCount; i++)
                {
                    lastAddrOctets[3]++;
                    if (lastAddrOctets[3] > 255)
                    {
                        lastAddrOctets[3] = 1;
                        lastAddrOctets[2]++;
                        if (lastAddrOctets[2] > 255)
                        {
                            lastAddrOctets[2] = 0;
                            lastAddrOctets[1]++;
                            if (lastAddrOctets[1] > 255)
                            {
                                lastAddrOctets[1] = 0;
                                lastAddrOctets[0]++;
                                if (lastAddrOctets[0] > 255)
                                {
                                    Console.WriteLine("Пошёл нахуй");
                                    continue;
                                }
                            }
                        }
                    }
                    string newIp = String.Format("{0}.{1}.{2}.{3}", lastAddrOctets[0], lastAddrOctets[1], lastAddrOctets[2], lastAddrOctets[3]);
                    devices.Add(new Device(newIp));
                }

                Console.WriteLine("DC: " + devices.Count);

                Console.WriteLine("Write routers");
                Console.Write("(ex. 192.168.0.1,192.168.1.1,192.168.2.1) : ");
                string routersStr = Console.ReadLine();
                string[] routersList = routersStr.Split(',');



                foreach (string routerStr in routersList)
                {
                    Router router = new Router(routerStr);
                    routers.Add(router);
                    Device removeDevice = null;
                    foreach (Device device in devices)
                    {
                        if(device.ip == routerStr)
                        {
                            removeDevice = device;
                            continue;
                        }
                        int[] devStartOctets = device.getStartOctets();
                        int[] routerStartOctets = router.getStartOctets();
                        if (devStartOctets[0] == routerStartOctets[0] &&
                            devStartOctets[1] == routerStartOctets[1] &&
                            devStartOctets[2] == routerStartOctets[2])
                        {
                            router.devices.Add(device);
                        }
                    }
                    devices.Remove(removeDevice);
                }


                Console.WriteLine("Routers\t = "+routers.Count);
                Console.WriteLine("Devices\t = "+devices.Count);

                foreach (Router router in routers)
                {
                    router.StartPing();
                }
            }
        }
    }
}