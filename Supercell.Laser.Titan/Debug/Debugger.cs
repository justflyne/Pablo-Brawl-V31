global using System;
global using System.Collections.Generic;
global using System.Linq;


namespace Supercell.Laser.Titan.Debug
{
    public static class Debugger
    {
        private static IDebuggerListener DebuggerListener;

        public static void SetListener(IDebuggerListener listener)
        {
            DebuggerListener = listener;
        }

        public static void Print(string log)
        {
            DebuggerListener?.Print(log);
        }

        public static void Warning(string log)
        {
            DebuggerListener?.Warning(log);
        }

        public static void Error(string log)
        {
            DebuggerListener?.Error(log);
        }

        public static bool DoAssert(bool assertion, string assertionError)
        {
            if (!assertion)
            {
                DebuggerListener?.Error(assertionError);
            }

            return assertion;
        }
    }

    public interface IDebuggerListener
    {
        void Print(string message);
        void Warning(string message);
        void Error(string message);
    }
}
