using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace WebAssistedSurvey.Business
{
    internal class QRHelper
    {
        internal static byte[] GetQrImageDataForUrl(string url, ImageFormat imageFormat)
        {
            PayloadGenerator.Url payload = new PayloadGenerator.Url(url);

            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            var qrBitmap = qrCode.GetGraphic(20);

            byte[] data;
            using (var stream = new MemoryStream())
            {
                qrBitmap.Save(stream, imageFormat);
                data = stream.ToArray();
            }

            return data;
        }
    }
}