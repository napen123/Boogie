#nullable enable

using System;
using System.Text;

using System.IO;
using System.IO.Compression;

namespace Boogie
{
    public enum TranslateType
    {
        File,
        ZipFast,
        ZipOptimal
    }

    public static class Translator
    {
        public static Encoding? GetEncoding(string encoding)
        {
            try
            {
                return int.TryParse(encoding, out var codePage)
                    ? Encoding.GetEncoding(codePage,
                        EncoderFallback.ReplacementFallback,
                        DecoderFallback.ReplacementFallback)
                    : Encoding.GetEncoding(encoding.ToLower(),
                        EncoderFallback.ReplacementFallback,
                        DecoderFallback.ReplacementFallback);
            }
            catch
            {
                return null;
            }
        }

        private static int TranslateFile(
            Encoding inputEncoding, Encoding outputEncoding,
            string inputFile, string outputFile)
        {
            try
            {
                // TODO: Is reading the entire file really a good idea?
                var inputText = File.ReadAllText(inputFile, inputEncoding);
                File.WriteAllText(outputFile, inputText, outputEncoding);

                return 0;
            }
            catch (Exception e)
            {
                return Logger.LogError("Failed to convert the input file's encoding : " + e.Message);
            }
        }

        private static int TranslateZip(
            Encoding inputEncoding, Encoding outputEncoding,
            string inputFile, string outputFile,
            bool optimal)
        {
            try
            {
                using var inputZip = new ZipArchive(
                    File.Open(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read),
                    ZipArchiveMode.Read, false, inputEncoding);
                using var outputZip = new ZipArchive(
                    File.Create(outputFile),
                    ZipArchiveMode.Create, false, outputEncoding);

                var compressionLevel = optimal ? CompressionLevel.Optimal : CompressionLevel.Fastest;

                foreach (var inputEntry in inputZip.Entries)
                {
                    var outputEntry = outputZip.CreateEntry(inputEntry.FullName, compressionLevel);
                    outputEntry.LastWriteTime = inputEntry.LastWriteTime;

                    using var inputStream = inputEntry.Open();
                    using var outputStream = outputEntry.Open();

                    inputStream.CopyTo(outputStream);
                }

                return 0;
            }
            catch (Exception e)
            {
                return Logger.LogError("Failed to convert the input file's encoding : " + e.Message);
            }
        }

        public static int Translate(
            string inputEncodingValue, string? outputEncodingValue,
            string inputFile, string outputFile,
            TranslateType translateType)
        {
            var inputEncoding = GetEncoding(inputEncodingValue);

            if (inputEncoding == null)
                return Logger.LogError("Invalid input encoding specified : " + inputEncodingValue);

            var outputEncoding = outputEncodingValue == null
                ? Encoding.UTF8
                : GetEncoding(outputEncodingValue);

            if (outputEncoding == null)
                return Logger.LogError("Invalid output encoding specified : " + outputEncodingValue);

            if (!File.Exists(inputFile))
                return Logger.LogError("Could not find input file : " + inputFile);

            switch (translateType)
            {
                case TranslateType.File:
                    return TranslateFile(inputEncoding, outputEncoding, inputFile, outputFile);
                case TranslateType.ZipFast:
                    return TranslateZip(inputEncoding, outputEncoding, inputFile, outputFile, false);
                case TranslateType.ZipOptimal:
                    return TranslateZip(inputEncoding, outputEncoding, inputFile, outputFile, true);
                default:
                    return 1;
            }
        }
    }
}
