using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business
{
    [Export(typeof(IBusinessEngineFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BusinessEngineFactory : IBusinessEngineFactory
    {
        T IBusinessEngineFactory.GetBusinessEngine<T>()
        {
            return ObjectBase.Container.GetExportedValue<T>();
        }
    }
}
