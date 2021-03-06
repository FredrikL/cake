﻿using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    internal sealed class CakeLogAdapter : ICakeLog
    {
        private readonly ICakeLog _log;

        public Verbosity Verbosity { get; set; }

        public CakeLogAdapter(ICakeLog log)
        {
            _log = log;
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity)
            {
                return;
            }
            _log.Write(verbosity, level, format, args);
        }
    }
}
