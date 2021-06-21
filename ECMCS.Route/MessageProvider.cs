using ECMCS.Utilities;
using System;
using System.Runtime.InteropServices;

namespace ECMCS.Route
{
    public class MessageProvider
    {
        private readonly string _windowTitle;

        public MessageProvider(string windowTitle)
        {
            _windowTitle = windowTitle;
        }

        public void Send(string message)
        {
            string windowTitle = _windowTitle;
            IntPtr ptrWnd = NativeMethods.FindWindow(null, windowTitle);
            if (ptrWnd == IntPtr.Zero)
            {
                return;
            }
            else
            {
                IntPtr ptrCopyData = IntPtr.Zero;
                try
                {
                    NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT
                    {
                        dwData = new IntPtr(2),
                        cbData = message.Length + 1,
                        lpData = Marshal.StringToHGlobalAnsi(message)
                    };
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);
                    NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (ptrCopyData != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(ptrCopyData);
                    }
                }
            }
        }
    }
}