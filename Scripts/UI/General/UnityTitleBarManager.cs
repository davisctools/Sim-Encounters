#if UNITY_STANDALONE_WIN
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class UnityTitleBarManager : MonoBehaviour
    {
        // https://answers.unity.com/questions/148723/how-can-i-change-the-title-of-the-standalone-playe.html
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        protected virtual void Start()
        {
            var proccess = Process.GetCurrentProcess();
            var proccessWindows = GetProcessWindows(proccess.Id);

            foreach (var proccessWindow in proccessWindows) {
                if (proccessWindow == IntPtr.Zero)
                    continue;

                SetWindowText(proccessWindow, $"{Application.productName} - {Application.version}");
                return;
            }
        }


        // https://stackoverflow.com/a/25152035/13984008
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);

        private IntPtr[] GetProcessWindows(int process)
        {
            IntPtr[] apRet = (new IntPtr[256]);
            int iCount = 0;
            IntPtr pLast = IntPtr.Zero;
            do {
                pLast = FindWindowEx(IntPtr.Zero, pLast, null, Application.productName);
                GetWindowThreadProcessId(pLast, out int iProcess_);
                if (iProcess_ == process) apRet[iCount++] = pLast;
            } while (pLast != IntPtr.Zero);
            Array.Resize(ref apRet, iCount);
            return apRet;
        }
    }
}
#endif