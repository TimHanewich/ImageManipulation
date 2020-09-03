using System;
using System.Drawing;

namespace TimHanewich.ImageManipulation
{
    public static class ConvolutionHelper
    {
        public static Bitmap ApplyKernel(this Bitmap bm, Kernel k)
        {
            Bitmap nbm = new Bitmap(bm.Width - 2, bm.Height - 2);

            int r = 0;
            int c = 0;
            for (r = 1; r < bm.Height - 1; r++)
            {
                for (c = 1; c < bm.Width - 1; c++)
                {

                    Color pix0 = bm.GetPixel(c - 1, r - 1);
                    Color pix1 = bm.GetPixel(c, r - 1);
                    Color pix2 = bm.GetPixel(c + 1, r - 1);
                    Color pix3 = bm.GetPixel(c - 1, r);
                    Color pix4 = bm.GetPixel(c, r);
                    Color pix5 = bm.GetPixel(c + 1, r);
                    Color pix6 = bm.GetPixel(c - 1, r + 1);
                    Color pix7 = bm.GetPixel(c, r + 1);
                    Color pix8 = bm.GetPixel(c + 1, r + 1);


                    int pix0c = Convert.ToInt32(Math.Floor((float)(pix0.R + pix0.G + pix0.B) / (float)3));
                    int pix1c = Convert.ToInt32(Math.Floor((float)(pix1.R + pix1.G + pix1.B) / (float)3));
                    int pix2c = Convert.ToInt32(Math.Floor((float)(pix2.R + pix2.G + pix2.B) / (float)3));
                    int pix3c = Convert.ToInt32(Math.Floor((float)(pix3.R + pix3.G + pix3.B) / (float)3));
                    int pix4c = Convert.ToInt32(Math.Floor((float)(pix4.R + pix4.G + pix4.B) / (float)3));
                    int pix5c = Convert.ToInt32(Math.Floor((float)(pix5.R + pix5.G + pix5.B) / (float)3));
                    int pix6c = Convert.ToInt32(Math.Floor((float)(pix6.R + pix6.G + pix6.B) / (float)3));
                    int pix7c = Convert.ToInt32(Math.Floor((float)(pix7.R + pix7.G + pix7.B) / (float)3));
                    int pix8c = Convert.ToInt32(Math.Floor((float)(pix8.R + pix8.G + pix8.B) / (float)3));

                    float ans = 0;
                    ans = ans + k.Cell0Weight * pix0c;
                    ans = ans + k.Cell1Weight * pix1c;
                    ans = ans + k.Cell2Weight * pix2c;
                    ans = ans + k.Cell3Weight * pix3c;
                    ans = ans + k.Cell4Weight * pix4c;
                    ans = ans + k.Cell5Weight * pix5c;
                    ans = ans + k.Cell6Weight * pix6c;
                    ans = ans + k.Cell7Weight * pix7c;
                    ans = ans + k.Cell8Weight * pix8c;

                    ans = Math.Min(255, ans);
                    ans = Math.Max(0, ans);
                    int ansi = Convert.ToInt32(ans);

                    Color nc = Color.FromArgb(ansi, ansi, ansi);

                    nbm.SetPixel(c - 1, r - 1, nc);

                }
            }

            return nbm;
        }
    }
}