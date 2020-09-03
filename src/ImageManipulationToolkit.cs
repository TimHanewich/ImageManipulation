using System;
using System.Drawing;
using System.Collections.Generic;

namespace TimHanewich.ImageManipulation
{
    public static class ImageManipulationToolkit
    {

        /// <summary>
        /// Returns a new image with only the specific pixels colored that have been changed on a white background.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="tolerance"></param>
        /// <param name="highlight_color"></param>
        /// <returns></returns>
        public static Image HighlightDifferences(Image image1, Image image2, Color highlight_color, float tolerance = 0.1f)
        {
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                throw new Exception("Unable to compare images because the widths and heights were not identical");
            }

            Bitmap bm1 = new Bitmap(image1);
            Bitmap bm2 = new Bitmap(image2);

            //Detect need for changes
            int w = 0;
            int h = 0;
            List<int[]> ToChange = new List<int[]>();
            for (h = 0; h < image1.Height; h++)
            {
                for (w = 0; w < image1.Width; w++)
                {
                    Color i1p = bm1.GetPixel(w, h);
                    Color i2p = bm2.GetPixel(w, h);
                    float brightness_diff = Math.Abs(i1p.GetBrightness() - i2p.GetBrightness());
                    if (brightness_diff > tolerance)
                    {
                        ToChange.Add(new int[] { w, h });
                    }
                }
            }

            //Make the changes
            Bitmap ToReturn = new Bitmap(image1.Width, image1.Height);
            foreach (int[] i in ToChange)
            {
                ToReturn.SetPixel(i[0], i[1], highlight_color);
            }
            return ToReturn;
        }

        public static Image HighlightEdges(this Image i, float tolerance = 20)
        {
            Bitmap bm = new Bitmap(i);
            Bitmap nbm = new Bitmap(i.Width, i.Height);

            int w = 0;
            int h = 0;
            for (h = 0; h < i.Height; h++)
            {
                for (w = 0; w < i.Width; w++)
                {
                    if (w != 0 && w != i.Width - 1 && h != 0 && h != i.Height - 1)
                    {
                        Color pix1 = bm.GetPixel(w, h + 1);
                        Color pix2 = bm.GetPixel(w + 1, h);
                        Color pix3 = bm.GetPixel(w, h - 1);
                        Color pix4 = bm.GetPixel(w - 1, h);
                        Color thispix = bm.GetPixel(w, h);
                        int pix1_avg = Convert.ToInt32((pix1.R + pix1.G + pix1.B) / 3);
                        int pix2_avg = Convert.ToInt32((pix2.R + pix2.G + pix2.B) / 3);
                        int pix3_avg = Convert.ToInt32((pix3.R + pix3.G + pix3.B) / 3);
                        int pix4_avg = Convert.ToInt32((pix4.R + pix4.G + pix4.B) / 3);
                        int thispix_avg = Convert.ToInt32((thispix.R + thispix.G + thispix.B) / 3);

                        bool mark = false;
                        if (Math.Abs(thispix_avg - pix1_avg) > tolerance)
                        {
                            mark = true;
                        }
                        if (Math.Abs(thispix_avg - pix2_avg) > tolerance)
                        {
                            mark = true;
                        }
                        if (Math.Abs(thispix_avg - pix3_avg) > tolerance)
                        {
                            mark = true;
                        }
                        if (Math.Abs(thispix_avg - pix4_avg) > tolerance)
                        {
                            mark = true;
                        }

                        if (mark == true)
                        {
                            nbm.SetPixel(w, h, Color.Red);
                        }
                    }
                }
            }

            return nbm;
        }

        public static int CountPixelDifferences(Image image1, Image image2, float tolerance = 0.1f)
        {
            Image diff = ImageManipulationToolkit.HighlightDifferences(image1, image2, Color.Red, tolerance);
            int pixel_change_count = 0;
            Bitmap bm = new Bitmap(diff);
            int w = 0;
            int h = 0;
            for (h = 0; h < bm.Height; h++)
            {
                for (w = 0; w < bm.Width; w++)
                {
                    Color pix = bm.GetPixel(w, h);
                    if (pix.R != 0 || pix.G != 0 || pix.B != 0)
                    {
                        pixel_change_count = pixel_change_count + 1;
                    }
                }
            }
            return pixel_change_count;
        }

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