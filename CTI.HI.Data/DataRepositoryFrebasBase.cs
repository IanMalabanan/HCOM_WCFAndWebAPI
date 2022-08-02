using Core.Common.Contracts;
using Core.Common.Data; 
using CTI.HI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CTI.HI.Data
{
    public abstract class DataRepositoryFrebasBase<T> : DataRepositoryBase<T, FrebasContext>
         where T : class, IIdentifiableEntity, new()
    {
      
    }
}
