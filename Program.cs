using System;
using System.Net.Sockets;
using System.Net;
namespace Ksis_labaratory_1
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Input address");

            string input = Console.ReadLine();


            
            Tracert tracert = new Tracert();

            tracert.Start(input);
            Console.ReadLine();
        }
    }
}
