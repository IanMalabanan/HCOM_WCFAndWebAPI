using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class StoredProcedureError
    {
        public int ErrorNumber { get; set; }
        public string Message { get; set; }
        public bool IsSucess { get { return ErrorNumber == 8888; } }

    }
}
