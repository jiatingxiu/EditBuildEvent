using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Test
{
    // IDisposable
    public class SerialPortFixer : IDisposable
    {
        public static void Execute(string portName)
        {
            using (new SerialPortFixer(portName))
            {
            }
        }
        #region "IDisposable Members"

        public void Dispose()
        {
            if ((m_Handle != null))
            {
                m_Handle.Close();
                m_Handle = null;
            }
        }

        #endregion

        #region "Implementation"

        private const int DcbFlagAbortOnError = 14;
        private const int CommStateRetries = 10;

        private SafeFileHandle m_Handle;

        private SerialPortFixer(string portName)
        {
            const int dwFlagsAndAttributes = 0x40000000;
            const int dwAccess = 0x00000000;

            if ((portName == null) || !(portName.StartsWith("COM", StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Invalid Serial Port", "portName");

            SafeFileHandle hFile = CreateFile("\\\\.\\" + portName, dwAccess, 0, IntPtr.Zero, 3, dwFlagsAndAttributes, IntPtr.Zero);
            if ((hFile.IsInvalid))
                WinIoError();

            try
            {
                int fileType = GetFileType(hFile);
                if (((fileType != 2) & (fileType != 0)))
                    throw new ArgumentException("Invalid Serial Port", "portName");
                m_Handle = hFile;
                InitializeDcb();
            }
            catch
            {
                hFile.Close();
                m_Handle = null;
                throw;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr arguments);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetCommState(SafeFileHandle hFile, ref Dcb lpDcb);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetCommState(SafeFileHandle hFile, ref Dcb lpDcb);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ClearCommError(SafeFileHandle hFile, ref int lpErrors, ref Comstat lpStat);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GetFileType(SafeFileHandle hFile);


        private void InitializeDcb()
        {
            Dcb Dcb = new Dcb();
            GetCommStateNative(ref Dcb);
            Dcb.Flags = DcbFlagAbortOnError;
            SetCommStateNative(ref Dcb);
        }

        private string GetMessage(int errorCode)
        {
            StringBuilder lpBuffer = new StringBuilder(0x200);
            if ((FormatMessage(0x3200, new HandleRef(null, IntPtr.Zero), errorCode, 0, lpBuffer, lpBuffer.Capacity, IntPtr.Zero) != 0))
            {
                return lpBuffer.ToString();
            }
            return "Unknown Error";
        }

        private void WinIoError()
        {
            int errorCode = Marshal.GetLastWin32Error();
            throw new IOException(GetMessage(errorCode), errorCode);
        }

        private void GetCommStateNative(ref Dcb lpDcb)
        {
            int commErrors = 0;
            Comstat Comstat = new Comstat();
            for (int i = 0; i <= CommStateRetries - 1; i++)
            {
                if (!(ClearCommError(m_Handle, ref commErrors, ref Comstat)))
                    WinIoError();
                if ((GetCommState(m_Handle, ref lpDcb)))
                    return;
                if ((i == CommStateRetries - 1))
                    WinIoError();
            }
        }

        private void SetCommStateNative(ref Dcb lpDcb)
        {
            int commErrors = 0;
            Comstat Comstat = new Comstat();
            for (int i = 0; i <= CommStateRetries - 1; i++)
            {
                if (!(ClearCommError(m_Handle, ref commErrors, ref Comstat)))
                    WinIoError();
                if ((SetCommState(m_Handle, ref lpDcb)))
                    return;
                if ((i == CommStateRetries - 1))
                    WinIoError();
            }
        }

        #region "Nested type: COMSTAT"

        private struct Comstat
        {

            public readonly uint Flags;
            public readonly uint cbInQue;

            public readonly uint cbOutQue;
        }
        #endregion

        #region "Nested type: DCB"

        private struct Dcb
        {

            public readonly uint DCBlength;
            public readonly uint BaudRate;
            public uint Flags;
            public readonly ushort wReserved;
            public readonly ushort XonLim;
            public readonly ushort XoffLim;
            public readonly byte ByteSize;
            public readonly byte Parity;
            public readonly byte StopBits;
            public readonly byte XonChar;
            public readonly byte XoffChar;
            public readonly byte ErrorChar;
            public readonly byte EofChar;
            public readonly byte EvtChar;
            public readonly ushort wReserved1;
        }
        #endregion

        #endregion
    }
}
