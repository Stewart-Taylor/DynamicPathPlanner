using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace DynamicPathPlanner.Code
{
    public unsafe class BitmapHelper
    {

        Bitmap bitmap;

        int width;
        BitmapData bitmapData = null;
        Byte* pBase = null;

        public struct Pixel
        {
            public byte B;
            public byte G;
            public byte R;
        }

        public BitmapHelper(Bitmap bitmap)
        {
            this.bitmap = new Bitmap(bitmap);
        }

        public BitmapHelper(int width, int height)
        {
            this.bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }

        public Bitmap Bitmap
        {
            get
            {
                return (bitmap);
            }
        }

        private Point PixelSize
        {
            get
            {
                GraphicsUnit unit = GraphicsUnit.Pixel;
                RectangleF bounds = bitmap.GetBounds(ref unit);

                return new Point((int)bounds.Width, (int)bounds.Height);
            }
        }

        public void LockBitmap()
        {
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF boundsF = bitmap.GetBounds(ref unit);
            Rectangle bounds = new Rectangle((int)boundsF.X,
           (int)boundsF.Y,
           (int)boundsF.Width,
           (int)boundsF.Height);

            width = (int)boundsF.Width * sizeof(Pixel);
            if (width % 4 != 0)
            {
                width = 4 * (width / 4 );
            }
            bitmapData =
           bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            pBase = (Byte*)bitmapData.Scan0.ToPointer();
        }

        public Pixel GetPixel(int x, int y)
        {
            Pixel returnValue = *PixelAt(x, y);
            return returnValue;
        }

        public void SetPixel(int x, int y, Color color)
        {
            Pixel p = new Pixel();
            p.R = color.R;
            p.G = color.G;
            p.B = color.B;

            Pixel* pixel = PixelAt(x, y);
            *pixel = p;
        }

        public void UnlockBitmap()
        {
            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
        }
        public Pixel* PixelAt(int x, int y)
        {
            return (Pixel*)(pBase + y * width + x * sizeof(Pixel));
        }
    }

}
