using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace HomeApi.WindowsCommander
{
    public class User32
    {
        [DllImport("user32.dll")]
        static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int FindWindow(string lpClassName, string lpWindowName);

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
                //SendMessage(handle, (int)WinMessage.WM_SYSCOMMAND, (int)command, 0);
            }
        }
    }
}