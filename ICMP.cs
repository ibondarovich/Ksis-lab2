using System;
using System.Net.Sockets;
using System.Net;
namespace Ksis_labaratory_1
{
    class ICMP
    {

        public ICMP(byte[] icmpPackage)
        {
            // icmpPackage = new byte[72];

            icmpPackage[0] = (byte)8; // type: echo request (1 byte)
            icmpPackage[1] = (byte)0; // code: 0 (сеть недостижима) (1 byte)

            //checksum - 2 bytes
            icmpPackage[2] = (byte)0; 
            icmpPackage[3] = (byte)0; 

            //identifier - 2 bytes
            icmpPackage[4] = (byte)0;
            icmpPackage[5] = (byte)1;

            //sequence number - 2 bytes
            icmpPackage[6] = (byte)0;
            icmpPackage[7] = (byte)0;

            //64 bytes for data
        }

        public void SequenceNumber(byte[] icmpPackage, byte sequenceNumber)
        {
            icmpPackage[7] = sequenceNumber;
            icmpPackage[6] = (byte)(sequenceNumber >> 8);
            
        }

        public void CheckSum(byte[] icmpPackage)
        {

            UInt16 checksum = 0;
            UInt16 temp = 0;
            checksum = (UInt16)(icmpPackage[0] << 8);
            checksum += (UInt16)(icmpPackage[1]);



            for( int i = 4; i < icmpPackage.Length; i+=2)
            {
                if(i!= 2)
                {
                    temp = (UInt16)0;
                    temp = (UInt16)(icmpPackage[i] << 8);
                    temp += (UInt16)(icmpPackage[i+1]);
                    checksum += temp;
                }
            }

            checksum = (UInt16)~checksum;

            icmpPackage[3] = (byte)checksum;
            icmpPackage[2] = (byte)(checksum >> 8);
        }
    }
}
