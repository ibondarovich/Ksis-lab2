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

        

        // public Tracert(string hostName)
        // {
        //     hostIP = Dns.GetHostEntry(hostName);
        // }

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
            Console.WriteLine("Трассировка маршрута к "+ hostIP.HostName + " [" + hostIP.AddressList[0]+"]\nс максимальным числом прыжков "  + maxHop +":");
            
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp); // создаем гнездо
            IPEndPoint iPEndPoint = new IPEndPoint(hostIP.AddressList[0], 0); //преставляет конечную точку с адресом + любым доступным портом, используем для связи с сокетом
            EndPoint ipend =iPEndPoint;


            //socket.Bind(ipend); // связывет гнездо с ip-адрессом и портом

            DateTime dateTime = new DateTime();

            byte[] icmpPackage = new byte[72];
            ICMP icmp = new ICMP(icmpPackage);


            byte[] recivePackage = new byte[256];
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);
            while(ttl <= maxHop)
            {
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
                        Console.Write(timeSpan.TotalMilliseconds + " " +"ms ");
                    }
                    catch(Exception)
                    {
                        Console.Write("    *    ");
                    }

                    icmp.SequenceNumber(icmpPackage, ++sequenceNumber);
                    icmp.CheckSum(icmpPackage);
                    
                }
                Console.WriteLine(ipend + "\n");

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