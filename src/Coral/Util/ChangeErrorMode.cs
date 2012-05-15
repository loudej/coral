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
            _oldMode = (int)ErrorModes.Default;
            
            if (!IsLinux)
            {
                _oldMode = SetErrorMode((int)mode);
            }
        }

        void IDisposable.Dispose() 
        {
            if (!IsLinux)
            {
                SetErrorMode(_oldMode); 
            }
        }

        bool IsLinux
        {
            get 
            {
                var platform = (int) Environment.OSVersion.Platform;
                return (platform == 4) || (platform == 6) || (platform == 128);
            }
        }
        
        [DllImport("kernel32.dll")]
        private static extern int SetErrorMode(int newMode);
    }

}
