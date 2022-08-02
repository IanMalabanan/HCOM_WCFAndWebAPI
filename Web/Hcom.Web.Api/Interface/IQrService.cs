 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface IQrService
    { 
         
        byte[] GenerateQrInByte(string qrText);
        void GenerateQrToImage(string qrText, string fullpath); 

    }
}
