using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class PunchlistImage
    {
        public int FileID { get; set; }

        public byte[] FileBinary { get; set; }

        public string FileName { get; set; }

        public int FileIndex { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? DateModified { get; set; }

        public string ModifiedBy { get; set; }
    }
}
