using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Native
{
    internal static class Kernel32
    {
        private const string kernel32 = "kernel32.dll";

        [DllImport(kernel32)]
        public static extern IntPtr GetStdHandle(StdHandle nStdHandle);

        [DllImport(kernel32)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out ConsoleMode lpMode);

        [DllImport(kernel32)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, ConsoleMode dwMode);

        [DllImport(kernel32, CharSet = CharSet.Unicode)]
        public static extern bool WriteConsole(IntPtr hConsoleOutput, string lpBuffer, uint nNumberOfCharsToWrite, out uint lpNumberOfCharsWritten);

        [DllImport(kernel32)]
        public static extern bool CancelIoEx(IntPtr hFile, IntPtr lpOverlapped);

        public enum StdHandle
        {
            STD_INPUT_HANDLE = -10,
            STD_OUTPUT_HANDLE = -11,
            STD_ERROR_HANDLE = -12
        }

        [Flags]
        public enum ConsoleMode : uint
        {
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
            DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
            ENABLE_LVB_GRID_WORLDWIDE = 0x0010
        }
    }
}
