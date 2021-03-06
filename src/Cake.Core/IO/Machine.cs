﻿using System;

namespace Cake.Core.IO
{
    public static class Machine
    {
        public static bool Is64BitOperativeSystem()
        {
            return Environment.Is64BitOperatingSystem;
        }

        public static bool IsUnix()
        {
            var platform = (int)Environment.OSVersion.Platform;
            return (platform == 4) || (platform == 6) || (platform == 128);
        }
    }
}
