using System;
using System.Drawing;

namespace Hsv
{
    public class HsvColor
    {
        private double Hue { get; set; }
        private double Saturation { get; set; }
        private double Value { get; set; }

        public HsvColor(Color rgbColor)
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
            Saturation = Math.Abs(max) < 0.00001 ? 0 : (1 - min/max)*100;
            Value = max*100;
        }

        public HsvColor(double hue, double saturation, double value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        public HsvColor(HsvColor hsvColor)
        {
            Hue = hsvColor.Hue;
            Saturation = hsvColor.Saturation;
            Value = hsvColor.Value;
        }

        public Color ToRgbColor()
        {
            var hi = Convert.ToInt32(Hue)/60%6;
            var vmin = (100 - Saturation)*Value/100;
            var alpha = (Value - vmin)*(Convert.ToInt32(Hue)%60)/60D;
            var v = Convert.ToInt32(Value*255/100);
            var vinc = Convert.ToInt32((vmin + alpha)*255/100);
            var vdec = Convert.ToInt32((Value - alpha)*255/100);
            var vmin2 = Convert.ToInt32(vmin*255/100);
            switch (hi)
            {
                case 0:
                    return Color.FromArgb(v, vinc, vmin2);
                case 1:
                    return Color.FromArgb(vdec, v, vmin2);
                case 2:
                    return Color.FromArgb(vmin2, v, vinc);
                case 3:
                    return Color.FromArgb(vmin2, vdec, v);
                case 4:
                    return Color.FromArgb(vinc, vmin2, v);
                case 5:
                    return Color.FromArgb(v, vmin2, vdec);
            }
            return Color.FromArgb(0, 0, 0);
        }

        public static implicit operator Color(HsvColor hcvColor)
        {
            return hcvColor.ToRgbColor();
        }
    }
}
