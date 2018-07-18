using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Config.Settings
{
    public class EncryptionSettings
    {
        public int KeySize { get; set; }
        public int BlockSize { get; set; }
        public string Password { get; set; }
        public string PrivatePassword { get; set; }
        public byte[] Salt { get; set; }
    }
}
