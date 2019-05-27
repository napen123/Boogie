#nullable enable

using System;

namespace Boogie
{
    public static class Program
    {
        private static void PrintHelp()
        {
            Console.WriteLine("Usage : Boogie <input encoding> [optional output encoding] <input file> <output file>");
            Console.WriteLine("     - Convert the encoding of a file.");
            Console.WriteLine("Usage : Boogie -z <input encoding> [optional output encoding] <input zip file> <output zip file>");
            Console.WriteLine("     - Convert the encoding of a zip file and the names of its entries.");
            Console.WriteLine("     - The output zip is compressed fast (non-optimally).");
            Console.WriteLine("Usage : Boogie -zo <input encoding> [optional output encoding] <input zip file> <output zip file>");
            Console.WriteLine("     - Convert the encoding of a zip file and the names of its entries.");
            Console.WriteLine("     - The output zip is compressed optimally (non-fast).");
            Console.WriteLine("Example : Boogie 932 japanese_recovered.txt japanese.txt");
            Console.WriteLine("Example : Boogie shift_jis japanese_recovered.txt japanese.txt");
            Console.WriteLine("Example : Boogie shift_jis utf-8 japanese_recovered.txt japanese.txt");
        }

        // TODO: Make the handling of arguments better!
        [STAThread]
        public static int Main(string[] args)
        {
            var argLength = args.Length;

            if (argLength == 0)
            {
                PrintHelp();
                return 0;
            }
            
            var firstArgument = args[0];
            
            var argumentShift = 0;
            var translateType = TranslateType.File;

            if (firstArgument == "-z")
            {
                argumentShift = 1;
                translateType = TranslateType.ZipFast;
            }
            else if (firstArgument == "-zo")
            {
                argumentShift = 1;
                translateType = TranslateType.ZipOptimal;
            }
            
            if (argLength == 3 + argumentShift)
            {
                return Translator.Translate(
                    args[argumentShift], null, 
                    args[argumentShift + 1], args[argumentShift + 2],
                    translateType);
            }

            if (argLength == 4 + argumentShift)
            {
                return Translator.Translate(
                    args[argumentShift], args[argumentShift + 1],
                    args[argumentShift + 2], args[argumentShift + 3],
                    translateType);
            }

            Console.WriteLine("Error : Invalid number of arguments given.");
            Console.WriteLine();
            PrintHelp();

            return 1;
        }
    }
}
