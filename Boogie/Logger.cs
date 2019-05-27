#nullable enable

using System;

namespace Boogie
{
    public static class Logger
    {
        public static int LogError(string message)
        {
            Console.WriteLine("Error : " + message);

            return 1;
        }

        public static int LogError(string message, params object[] args)
        {
            Console.WriteLine(message, args);

            return 1;
        }
    }
}
