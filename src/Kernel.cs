using System;

namespace TimHanewich.ImageManipulation
{
    public class Kernel
    {
        //  0   1   2
        //  3   4   5
        //  6   7   8

        public float Cell0Weight { get; set; }
        public float Cell1Weight { get; set; }
        public float Cell2Weight { get; set; }
        public float Cell3Weight { get; set; }
        public float Cell4Weight { get; set; }
        public float Cell5Weight { get; set; }
        public float Cell6Weight { get; set; }
        public float Cell7Weight { get; set; }
        public float Cell8Weight { get; set; }


        public static Kernel Create(float cell0, float cell1, float cell2, float cell3, float cell4, float cell5, float cell6, float cell7, float cell8)
        {
            Kernel k = new Kernel();
            k.Cell0Weight = cell0;
            k.Cell1Weight = cell1;
            k.Cell2Weight = cell2;
            k.Cell3Weight = cell3;
            k.Cell4Weight = cell4;
            k.Cell5Weight = cell5;
            k.Cell6Weight = cell6;
            k.Cell7Weight = cell7;
            k.Cell8Weight = cell8;
            return k;
        }

        public static Kernel LoadFromPreset(KernelPreset preset)
        {
            if (preset == KernelPreset.Blur)
            {
                Kernel k = Kernel.Create(0.0625f, 0.125f, 0.0625f, 0.125f, 0.25f, 0.125f, 0.0625f, 0.125f, 0.0625f);
                return k;
            }
            else if (preset == KernelPreset.Emboss)
            {
                Kernel k = Kernel.Create(-2, -1, 0, -1, 1, 1, 0, 1, 2);
                return k;
            }
            else if (preset == KernelPreset.Sharpen)
            {
                Kernel k = Kernel.Create(0, -1, 0, -1, 5, -1, 0, -1, 0);
                return k;
            }
            else
            {
                throw new Exception("We do not have a valid preset for that.");
            }
        }
    }
}