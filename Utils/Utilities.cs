using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Utils
{
    public class Utilities
    {
        public Utilities() { }

        public string GenSerial()
        {
            var random = new Random();
            string serial = string.Empty;
            for (int i = 0; i < 6; i++)
                serial = String.Concat(serial, random.Next(6).ToString());

            return serial;
        }
    }
}
