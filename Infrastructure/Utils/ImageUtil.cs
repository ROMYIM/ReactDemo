using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ReactDemo.Infrastructure.Utils
{
    public class ImageUtil
    {
        private readonly Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        private readonly FontFamily[] _fontFamilys;

        public ImageUtil()
        {
            _fontFamilys = FontFamily.Families;
        }

        public MemoryStream CreateVerifyCodePicture(string verifyCode, Random random)
        {
            var image = new Bitmap(verifyCode.Length * 18, 32);
            var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                graphics.DrawRectangle(new Pen(_colors[random.Next(_colors.Length)]), x, y, 1, 1);
            }

            for (int i = 0; i < verifyCode.Length; i++)
            {
                var brush = new SolidBrush(_colors[random.Next(_colors.Length)]);
                var font = new Font(_fontFamilys[random.Next(40, 50)], 20);
                graphics.DrawString(verifyCode.Substring(i, 1), font, brush, i * 10 + 3, random.Next(3));
            }

            var memorySteam = new MemoryStream();
            image.Save(memorySteam, ImageFormat.Png);
            graphics.Dispose();
            image.Dispose();
            return memorySteam;
        }
    }
}