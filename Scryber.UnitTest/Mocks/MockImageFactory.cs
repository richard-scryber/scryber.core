using System;
using Scryber.Drawing;


namespace Scryber.UnitTests.Mocks
{
    public class MockImageFactory : IPDFImageDataFactory
    {
            
        public bool ShouldCache { get { return false; } }

        public ImageData LoadImageData(IDocument document, IComponent owner, string path)
        {
            
            try
            {
                var uri = new Uri(path);
                var param = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                var name = System.IO.Path.GetFileNameWithoutExtension(param);

                // Standard System.Drawing routines to draw a bitmap
                // could load an image from SQL, use parameters, whatever is needed

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(300, 100);
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bmp))
                {
                    graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue), new System.Drawing.Rectangle(0, 0, 300, 100));
                    graphics.DrawString(name, new System.Drawing.Font("Times", 12), new System.Drawing.SolidBrush(System.Drawing.Color.Blue), System.Drawing.PointF.Empty);
                    graphics.Flush();
                }
                var dir = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var png = System.IO.Path.Combine(dir, "Temp.png");
                bmp.Save(png);

                ImageData data = ImageData.LoadImageFromBitmap(path, bmp, false);
                return data;
            }
            catch(Exception ex)
            {
                throw new ArgumentException("The image creation failed", ex);
            }
        }
    }
}
