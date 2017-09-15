using System.Collections.Generic;

namespace DuplicatedFilesFinder
{
    public class AuxObj
    {
        public byte[] checksum { get; set; }
        public List<Model.FileItem> files { get; set; }
        public AuxObj()
        {
            files = new List<Model.FileItem>();
        }
    }
}
