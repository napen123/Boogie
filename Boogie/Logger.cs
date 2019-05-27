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
    }
}
