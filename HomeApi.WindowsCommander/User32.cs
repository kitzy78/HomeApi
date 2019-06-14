using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace HomeApi.WindowsCommander
{
    public class User32
    {
        internal class TimerState
        {
            public int Counter;
            public Graphics Graphic;

            public TimerState(int counter, Graphics graphic)
            {
                Counter = counter;
                Graphic = graphic;
            }
        }

        [DllImport("user32.dll")]
        static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        
        public static IEnumerable<Tuple<string, string>> GetOpenWindowsByApplication(string name)
        {
            var processList = Process.GetProcesses();
            return processList.Where(p => p.ProcessName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .Select(p => new Tuple<string, string>(p.ProcessName, p.MainWindowTitle));
        }

        public static void IssueSystemCommand(string processName, SysCommand command)
        {
            var windows = GetOpenWindowsByApplication(processName);
            foreach (var window in windows)
            {
                var handle = FindWindow(window.Item1, window.Item2);
                if (handle <= 0) continue;

                Debug.WriteLine($"{Enum.GetName(typeof(SysCommand), command)} issued to window {window.Item2} on handle {handle}");
                PostMessage(handle, (int)WinMessage.WM_SYSCOMMAND, (int)command, 0);
            }
            DrawPanicBorder(10, 250);
        }

        public static void DrawPanicBorder(int flashes, int delay)
        {
            var desktopPtr = GetDC(IntPtr.Zero);
            var graphic = Graphics.FromHdc(desktopPtr);

            var timerState = new TimerState(0, graphic);
            var timer = new Timer(DrawPanicBorderTimerTask, timerState, delay, delay);

            while (timerState.Counter <= flashes)
            {
                Task.Delay(delay).Wait();
            }

            timer.Dispose();

            graphic.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }

        public static void DrawPanicBorderTimerTask(object timerState)
        {
            if (!(timerState is TimerState state)) return;
            var pen = new Pen(new SolidBrush(Color.Red), 20.0f);
            if (state.Counter % 2 == 0) pen = new Pen(new SolidBrush(Color.Black), 20.0f);
            state.Graphic.DrawRectangle(pen, new Rectangle(0, 0, 1920, 1080));

            Interlocked.Increment(ref state.Counter);
        }
    }
}