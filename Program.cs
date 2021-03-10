using System;
using LB_1_IS;

namespace LB_1_IS
{
    class Program
    {
        static void Main(string[] args)
        {
            Crypto.EncodeMultCipher("in.txt", 2);
            Crypto.DecodeMultCipher("encoded.txt", 2);
            Crypto.EncodePleifer("in.txt", "аддище");
        }
    }
}
