using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace GenerateQrCode.Controllers
{
    public class GenerateQrController : Controller
    {
         // GET: QRCode
            public ActionResult Generate()
            {
                return View();
            }

            [HttpPost]
            public ActionResult Generate(tblGenerateQrCode qrcode)
            {
                try
                {
                    qrcode.QRCodeImagePath = GenerateQRCode(qrcode.QRCodeText);
                    ViewBag.Message = "QR Code Created successfully";
                }
                catch (Exception ex)
                {
                }
                return View(qrcode);
            }

            private string GenerateQRCode(string qrcodeText)
            {
                string folderPath = "~/Images/";
                string imagePath = "~/Images/QrCode.jpg";
                // If the directory doesn't exist then create it.
                if (!Directory.Exists(Server.MapPath(folderPath)))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var result = barcodeWriter.Write(qrcodeText);

                string barcodePath = Server.MapPath(imagePath);
                var barcodeBitmap = new Bitmap(result);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                return imagePath;
            }

            public ActionResult Read()
            {
                return View(ReadQRCode());
            }

            private tblGenerateQrCode ReadQRCode()
            {
                tblGenerateQrCode barcodeModel = new tblGenerateQrCode();
                string barcodeText = "";
                string imagePath = "~/Images/QrCode.jpg";
                string barcodePath = Server.MapPath(imagePath);
                var barcodeReader = new BarcodeReader();

                var result = barcodeReader.Decode(new Bitmap(barcodePath));
                if (result != null)
                {
                    barcodeText = result.Text;
                }
                return new tblGenerateQrCode() { QRCodeText = barcodeText, QRCodeImagePath = imagePath };
            }
        }
	}
