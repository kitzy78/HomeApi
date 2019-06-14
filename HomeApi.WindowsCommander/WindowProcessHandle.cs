using System;

namespace HomeApi.WindowsCommander
{
    public class WindowProcessHandle
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public IntPtr Handle { get; set; }

        public WindowProcessHandle(string name, string title, IntPtr handle)
        {
            Name = name;
            Title = title;
            Handle = handle;
        }
    }
}