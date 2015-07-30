using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SoulViewer
{
    public class BitmapBuilder
    {
        private readonly int mWidth;
        private readonly int mHeight;
        private readonly PixelFormat mFormat;

        public BitmapBuilder(int width, int height, PixelFormat format)
        {
            mWidth = width;
            mHeight = height;
            mFormat = format;
        }

        public Bitmap AssembleFromRawBytes(byte[] bytes)
        {
            var bmp = new Bitmap(mWidth, mHeight, PixelFormat.Format16bppRgb565);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0,
                                                            bmp.Width,
                                                            bmp.Height),
                                                ImageLockMode.WriteOnly,
                                                bmp.PixelFormat);

            Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
