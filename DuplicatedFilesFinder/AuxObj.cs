using System.Security.Cryptography;

namespace DuplicatedFilesFinder
{
    public class AuxObj
    {
        public MD5 checksum { get; set; }
        public int reps { get; set; }
        public string path { get; set; }
    }
}
