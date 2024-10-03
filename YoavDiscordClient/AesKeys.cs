using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    [Serializable]
    public class AesKeys
    {
        public byte[] Key { get; set; }

        public byte[] Iv { get; set; }


    }
}
