using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StreamStore.Benchmarking
{
    static class RandomGenerator
    {
        public static int GenerateInt(int min, int max)
        {
            var generator = RandomNumberGenerator.GetInt32(min, max);
            byte[] data = new byte[4];
            BitConverter.ToInt32(data, 0);
        }
    }
}
