using System;
using System.Runtime.InteropServices;

namespace UnityFramework.Runtime
{
    public class WindowControlHelper
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}  

        /// <summary>
        /// 最小化软件窗体
        /// </summary>
        public static void WindowMini()
        { 
            ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        }
    }
}