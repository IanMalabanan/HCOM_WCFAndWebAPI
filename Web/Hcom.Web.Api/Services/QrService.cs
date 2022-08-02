
using Hcom.Web.Api.Interface; 
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Services
{
    public class QrService : IQrService
    {
        public byte[] GenerateQrInByte(string qrText)
        {
            var bmp = GenerateQr(qrText);
            return BitmapToBytes(bmp);
        }

        public void GenerateQrToImage(string qrText, string fullpath)
        {
            var bmp = GenerateQr(qrText);
            bmp.Save(fullpath, ImageFormat.Png);
        }

        private static Bitmap GenerateQr(string qrText)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    return qrCodeImage;
                };
            };
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
         
    }
}
