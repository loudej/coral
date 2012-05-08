using System;
using System.Runtime.InteropServices;

namespace Coral.Util
{
    [Flags]
    public enum ErrorModes
    {
        Default = 0x0,
        FailCriticalErrors = 0x1,
        NoGpFaultErrorBox = 0x2,
        NoAlignmentFaultExcept = 0x4,
        NoOpenFileErrorBox = 0x8000
    }

    public struct ChangeErrorMode : IDisposable
    {
        private readonly int _oldMode;

        public ChangeErrorMode(ErrorModes mode)
        {
            _oldMode = SetErrorMode((int)mode);
        }

        void IDisposable.Dispose() { SetErrorMode(_oldMode); }

        [DllImport("kernel32.dll")]
        private static extern int SetErrorMode(int newMode);
    }

}
