using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace Queue.Hub.Svetovod
{
    public class SvetovodMatrixDisplayConnection : SvetovodConnection, ISvetovodDisplayConnection
    {
        public SvetovodMatrixDisplayConnection(string port) :
            base(port)
        {
        }

        public void ShowText(byte sysnum, string text, int width, int height)
        {
            var body = CreateBody(GetTextBytes(text, width, height));
            var header = CreateHeader(sysnum, 0x00, 0x00, (byte)(body.Length - 1));
            WriteToPort(header, body);
        }

        private byte[] GetTextBytes(string text, int width, int height)
        {
            var matrix = new List<byte>();

            using (var bitmap = new Bitmap(width, height))
            using (var g = Graphics.FromImage(bitmap))
            using (var font = new Font("Courier New", 10, FontStyle.Bold, GraphicsUnit.Point))
            {
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(text, font, Brushes.Black, -2, -2);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var index = (int)(width * y + x) / 8;
                        if (matrix.Count <= index)
                        {
                            matrix.Add(0);
                        }

                        if (bitmap.GetPixel(x, y).A != 0)
                        {
                            matrix[index] |= (byte)(1 << (7 - (x <= 7 ? x : x % 8)));
                        }
                    }
                }
            }

            return matrix.ToArray();
        }
    }
}