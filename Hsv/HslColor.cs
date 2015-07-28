using System;
using System.Collections.Generic;
using System.Drawing;

namespace Hsv
{
    enum ColorVect
    {
        r = 0,
        g = 1,
        b = 2
    }
    public class HslColor
    {
        private double Hue { get; set; }
        private double Saturation { get; set; }
        private double Lightness { get; set; }

        public HslColor(Color rgbColor)
        {
            var r = rgbColor.R/255D;
            var g = rgbColor.G/255D;
            var b = rgbColor.B/255D;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            if (Math.Abs(max - min) < 0.00001) Hue = 0;
            if (Math.Abs(max - r) < 0.00001)
            {
                Hue = 60*(g - b)/(max - min) + (g < b ? 360 : 0);
            }
            if (Math.Abs(max - g) < 0.00001)
            {
                Hue = 60*(b - r)/(max - min) + 120;
            }
            if (Math.Abs(max - b) < 0.00001)
            {
                Hue = 60*(r - g)/(max - min) + 240;
            }
            Saturation = (max - min)*100/(1 - (1 - (min + max)));
            Lightness = (max + min) * 50;
        }

        public HslColor(double hue, double saturation, double lightness)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        public HslColor(HslColor hslColor)
        {
            Hue = hslColor.Hue;
            Saturation = hslColor.Saturation;
            Lightness = hslColor.Lightness;
        }

        public Color ToRgbColor()
        {
            var l = Lightness/50;
            var s = Saturation/100;
            var h = Hue/360;
            var q = l < 0.5 
                ? l * (1 + s) 
                : l + s - (l * s);
            var p = 2*l - q;
            var t = new double[3];
            t[(int) ColorVect.r] = h + 1D/3D;
            t[(int) ColorVect.g] = h;
            t[(int) ColorVect.b] = h - 1D/3D;
            for (var i = (int)ColorVect.r; i <= (int)ColorVect.b; i++)
            {
                t[i] = t[i] < 0 ? t[i] + 1 : t[i];
                t[i] = t[i] > 1 ? t[i] - 1 : t[i];
            }
            var color = new double[3];

            for (var i = (int)ColorVect.r; i <= (int)ColorVect.b; i++)
            {
                if (t[i] < 1D/6D)
                {
                    color[i] = p + ((q - p)*6*t[i]);
                }
                if (t[i] >= 1D/6D && t[i] < 1D/2D)
                {
                    color[i] = q;
                }
                if (t[i] >= 1D/2D && t[i] < 2D/3D)
                {
                    color[i] = p + ((q - p)*(2D/3D - t[i])*6);
                }
                if (t[i] >= 2D/3D)
                {
                    color[i] = p;
                }
            }
            return Color.FromArgb(
                Convert.ToInt32(color[(int) ColorVect.r]*255),
                Convert.ToInt32(color[(int) ColorVect.g]*255),
                Convert.ToInt32(color[(int) ColorVect.b]*255));
        }

        public static implicit operator Color(HslColor hclColor)
        {
            return hclColor.ToRgbColor();
        }
    }
}
