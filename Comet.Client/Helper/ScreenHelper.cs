using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Comet.Client.Utilities;

namespace Comet.Client.Helper
{
    public static class ScreenHelper
    {
        private const int SRCCOPY = 0x00CC0020;

        public static Bitmap CaptureScreen(int screenNumber, bool isShowMouseCursor)
        {
            Rectangle bounds = GetBounds(screenNumber);
            Bitmap screen = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppPArgb);

            // 先采样一次光标信息（尽量与 BitBlt 同步）
            CURSORINFO? ciSnap = null;
            ICONINFO? iiSnap = null;
            if (isShowMouseCursor)
                (ciSnap, iiSnap) = SnapshotCursor();

            using (Graphics g = Graphics.FromImage(screen))
            {
                IntPtr destDC = g.GetHdc();
                IntPtr srcDC = NativeMethods.CreateDC("DISPLAY", null, null, IntPtr.Zero);

                NativeMethods.BitBlt(destDC, 0, 0, bounds.Width, bounds.Height, srcDC, bounds.X, bounds.Y, SRCCOPY);

                if (isShowMouseCursor && ciSnap.HasValue && iiSnap.HasValue)
                {
                    TryDrawCursor(destDC, bounds, ciSnap.Value, iiSnap.Value);
                }

                NativeMethods.DeleteDC(srcDC);
                g.ReleaseHdc(destDC);
            }

            // 清理 ICONINFO 位图
            if (iiSnap.HasValue)
            {
                if (iiSnap.Value.hbmColor != IntPtr.Zero) DeleteObject(iiSnap.Value.hbmColor);
                if (iiSnap.Value.hbmMask != IntPtr.Zero) DeleteObject(iiSnap.Value.hbmMask);
            }

            return screen;
        }

        public static Rectangle GetBounds(int screenNumber)
        {
            return Screen.AllScreens[screenNumber].Bounds;
        }

        private const int CURSOR_SHOWING = 0x00000001;
        private const int DI_NORMAL = 0x0003;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int x; public int y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct CURSORINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ICONINFO
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")] private static extern bool GetCursorInfo(ref CURSORINFO pci);
        [DllImport("user32.dll")] private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);
        [DllImport("user32.dll")] private static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);
        [DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr hObject);

        private static (CURSORINFO?, ICONINFO?) SnapshotCursor()
        {
            var ci = new CURSORINFO { cbSize = Marshal.SizeOf(typeof(CURSORINFO)) };
            if (!GetCursorInfo(ref ci)) return (null, null);
            if ((ci.flags & CURSOR_SHOWING) == 0 || ci.hCursor == IntPtr.Zero) return (null, null);

            if (!GetIconInfo(ci.hCursor, out ICONINFO ii)) return (null, null);
            return (ci, ii);
        }

        private static void TryDrawCursor(IntPtr hdc, Rectangle captureBounds, CURSORINFO ci, ICONINFO ii)
        {
            try
            {
                int cursorX = ci.ptScreenPos.x - ii.xHotspot - captureBounds.X;
                int cursorY = ci.ptScreenPos.y - ii.yHotspot - captureBounds.Y;

                if (cursorX < captureBounds.Width && cursorY < captureBounds.Height &&
                    cursorX > -64 && cursorY > -64)
                {
                    DrawIconEx(hdc, cursorX, cursorY, ci.hCursor, 0, 0, 0, IntPtr.Zero, DI_NORMAL);
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}