using NLog;
using System;
using System.Diagnostics;
using System.Reflection;

namespace AnalogDropbox.Log
{
    public static class Logger
    {
        public static NLog.Logger ServiceLog = LogManager.GetCurrentClassLogger();
    }
}
