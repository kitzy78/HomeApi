using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HomeApi.WindowsCommander
{
    public static class WindowProcessService
    {
        public static IEnumerable<WindowProcessHandle> GetWindows(string processName)
        {
            var processList = Process.GetProcesses();
            return processList
                .Where(p => p.ProcessName.Equals(processName, StringComparison.InvariantCultureIgnoreCase))
                .Select(p => new WindowProcessHandle(p.ProcessName, p.MainWindowTitle, p.Handle));
        }
    }
}