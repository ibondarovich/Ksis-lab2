using System;
using System.Net.Sockets;
using System.Net;
namespace Ksis_labaratory_1
{
    class Program
    {
        static void Main(string[] args)
        {
            // byte[] icmpPackage = new byte[72];;
            // ICMP icmp = new ICMP(icmpPackage);
            // icmp.CheckSum(icmpPackage);



            
            Tracert tracert = new Tracert();

            tracert.Start("8.8.8.8");
        }
    }
}
