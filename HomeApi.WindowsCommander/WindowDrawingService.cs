using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace HomeApi.WindowsCommander
{
    public static class WindowDrawingService
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public static void DrawPanicBorder(int iterations)
        {
            if (iterations <= 0) return;
            Thread.Sleep(200);
            var desktopPtr = GetDC(IntPtr.Zero);
            var graphic = Graphics.FromHdc(desktopPtr);

            var pen = new Pen(new SolidBrush(Color.Red), 20.0f);
            if (iterations % 2 == 0) pen = new Pen(new SolidBrush(Color.Black), 20.0f);
            graphic.DrawRectangle(pen, new Rectangle(0, 0, 1920, 1080));

            graphic.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);

            DrawPanicBorder(--iterations);
        }
    }
}
