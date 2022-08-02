using CTI.HI.Business.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace CTI.HI.Business.Managers
{
    class UtilityManager //: IMessaingService
    {

        [Import]
        IMessagingService _EmailValidationManager;
    }
}
