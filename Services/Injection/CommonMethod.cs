using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection
{
    public static class CommonMethod
    {
        public static string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
        //public static bool GenerateBarcode(string barcode)
        //{
        //    if (barcode != null)
        //    {
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (Bitmap bitMap = new(barcode.Length * 40, 80))
        //            {
        //                using (Graphics graphics = Graphics.FromImage(bitMap))
        //                {
        //                    Font oFont = new Font("IDAutomationHC39M", 16);
        //                    PointF point = new PointF(2f, 2f);
        //                    SolidBrush whiteBrush = new SolidBrush(Color.White);
        //                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
        //                    SolidBrush blackBrush = new SolidBrush(Color.DarkBlue);
        //                    graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
        //                }
        //                bitMap.Save(memoryStream, ImageFormat.Jpeg);
        //                //ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
        //            }
        //        }
        //        return true;
        //    }
        //}
    }
}
