using System;
using System.Net.Sockets;
using System.Net;
namespace Ksis_labaratory_1
{
    class Tracert
    {
        private IPHostEntry hostIP;
       

        private int ttl = 1; //время жизни пакета
        private int maxHop = 30; // максимальное количество прыжков при поиске узла
        private int timeout = 4000; //максимальное время ожидания ICMP-ответа от удаленного узла

        private byte sequenceNumber = 0;


        public void Start(string hostName)
        {
            try
            {
                hostIP = Dns.GetHostEntry(hostName);
            }
            catch
            {
                Console.WriteLine("Не удается разрешить системное имя узла dns.");
                return;
            }
            Console.WriteLine("Трассировка маршрута к "+ hostIP.HostName + " [" + hostIP.AddressList[0]+"]\nс максимальным числом прыжков "  + maxHop +":\n");
            
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp); // создаем гнездо
            IPEndPoint iPEndPoint = new IPEndPoint(hostIP.AddressList[0], 0); //преставляет конечную точку с адресом + любым доступным портом, используем для связи с сокетом
            EndPoint ipend =iPEndPoint;


            //socket.Bind(ipend); // связывет гнездо с ip-адрессом и портом

            DateTime dateTime = new DateTime();

            byte[] icmpPackage = new byte[72];
            ICMP icmp = new ICMP(icmpPackage);


            byte[] recivePackage = new byte[256];
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);

            int err = 0;
            while(ttl <= maxHop)
            {
                err = 0;
                icmp.SequenceNumber(icmpPackage, ++sequenceNumber);
                icmp.CheckSum(icmpPackage);
                Console.Write(ttl + " ");
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive,ttl);
                for(int i = 0; i < 3; i++)
                {
                    try
                    {
                        dateTime = DateTime.Now;
                        socket.SendTo(icmpPackage, iPEndPoint);
                        socket.ReceiveFrom(recivePackage, ref ipend);

                        TimeSpan timeSpan = DateTime.Now - dateTime;
                        Console.Write("{0,2}",timeSpan.TotalMilliseconds +" ms ");
                        
                    }
                    catch(Exception)
                    {
                        err++;
                        Console.Write("    *    ");
                    }

                    icmp.SequenceNumber(icmpPackage, ++sequenceNumber);
                    icmp.CheckSum(icmpPackage);
                    
                }

                if(err == 3) 
                    Console.WriteLine("Превышен интервал ожидания для запроса.");
                else 
                {
                    IPEndPoint ipep = (IPEndPoint)ipend;
                    Console.WriteLine(ipep.Address.ToString());
                }

                if(recivePackage[20] == 0)
                {
                    Console.WriteLine("Трассировка завершена.");
                    break;
                }
                ttl++;
            }
        }
    }
}