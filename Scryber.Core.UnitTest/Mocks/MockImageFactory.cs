using System;
using System.Security.Policy;
using Scryber.Drawing;
using System.Drawing;
using System.Security.Cryptography;

namespace Scryber.UnitTests.Mocks
{
    public class MockImageFactory : IPDFImageDataFactory
    {
        

        public MockImageFactory()
        {
        }
            
        public bool ShouldCache { get { return false; } }

        public PDFImageData LoadImageData(IPDFDocument document, IPDFComponent owner, string path)
        {
            

            try
            {
                var uri = new Uri(path);
                var param = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                var name = System.IO.Path.GetFileNameWithoutExtension(param);

                // Standard System.Drawing routines

                Bitmap bmp = new Bitmap(300, 100);
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.FillRectangle(new SolidBrush(Color.LightBlue), new Rectangle(0, 0, 300, 100));
                    graphics.DrawString(name, new Font("Times", 12), new SolidBrush(Color.Blue), PointF.Empty);
                    graphics.Flush();
                }
                var dir = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var png = System.IO.Path.Combine(dir, "Temp.png");
                bmp.Save(png);

                PDFImageData data = PDFImageData.LoadImageFromBitmap(path, bmp, false);
                return data;
            }
            catch(Exception ex)
            {
                throw new ArgumentException("The image creation failed", ex);
            }
        }
    }
}
