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
            // Find the window with the name of the main form
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
                    // Create the data structure and fill with data
                    NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT
                    {
                        dwData = new IntPtr(2),
                        cbData = message.Length + 1,
                        //lpData = Marshal.StringToHGlobalAnsi(message)
                        lpData = Marshal.StringToHGlobalAnsi(message)
                    };

                    // Allocate memory for the data and copy
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);

                    // Send the message
                    NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // Free the allocated memory after the contol has been returned
                    if (ptrCopyData != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(ptrCopyData);
                    }
                }
            }
        }
    }
}