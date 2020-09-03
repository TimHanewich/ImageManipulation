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

    }
}