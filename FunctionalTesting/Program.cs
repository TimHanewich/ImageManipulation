using System;
using TimHanewich.ImageManipulation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FunctionalTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap bm = new Bitmap(8, 8);
            
            bm.SetPixel(3, 3, Color.Black);
            bm.SetPixel(3, 2, Color.Black);

            Stream write_to = System.IO.File.OpenWrite("C:\\Users\\tihanewi\\Downloads\\test.png");
            bm.Save(write_to, ImageFormat.Png);
            write_to.Dispose();

        }
    }
}
