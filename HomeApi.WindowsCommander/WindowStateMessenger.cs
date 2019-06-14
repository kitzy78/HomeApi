using System.Linq;
using System.Runtime.InteropServices;

namespace HomeApi.WindowsCommander
{
    public static class WindowStateMessenger
    {
        private const WinMessage WindowsSystemCommand = WinMessage.WM_SYSCOMMAND;
        private const SysCommand CloseCommand = SysCommand.SC_CLOSE;
        private const SysCommand MinimizeCommand = SysCommand.SC_MINIMIZE;

        [DllImport("user32.dll")]
        static extern int PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        private static void PostMessage(string processName, int command)
        {
            var windows = WindowProcessService.GetWindows(processName).ToList();
            foreach (var window in windows)
            {
                PostMessage((int)window.Handle, (int)WindowsSystemCommand, command, 0);
            }

            WindowDrawingService.DrawPanicBorder(10);
        }

        public static void Close(string processName)
        {
            PostMessage(processName, (int)CloseCommand);
        }

        public static void Minimize(string processName)
        {
            PostMessage(processName, (int)MinimizeCommand);
        }
    }
}
