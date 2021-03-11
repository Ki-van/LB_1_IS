using System;
using LB_1_IS;

namespace LB_1_IS
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Crypto.EncodePleifer("in.txt", "аддище");
            Crypto.DecodePleifer("encoded.txt", "аддище");
        }
    }
}
