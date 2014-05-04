using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace AntsBattle
{
    public class Images
    {
        private readonly Dictionary<string, Image> images;

        public Images(string path)
        {
            images = Directory.EnumerateFiles(path, "*.png").ToDictionary(Path.GetFileNameWithoutExtension, Image.FromFile);
        }

        public Image GetImage(string key)
        {
            var image = FindImage(key);
            if (image == null)
                throw new Exception("no image for " + key);
            return image;
        }

        private Image FindImage(string key)
        {
            Image res;
            return !images.TryGetValue(key.ToLower(), out res) ? null : res;
        }

        public Image GetChar(char ch)
        {
            string key = "letter." + ch;
            Image img = FindImage(key);
            if (img == null) images[key] = img = CreateCharImage(ch);
            return img;
        }

        private Image CreateCharImage(char ch)
        {
            var bmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.DrawString(ch.ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.DarkCyan, Point.Empty);
            }
            return bmp;
        }
    }
}