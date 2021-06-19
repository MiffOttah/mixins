using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiffTheFox
{
    // TODO: test this class

    /// <summary>
    /// Formats binary information for console output as hex. (Compare Unix xxd)
    /// </summary>
    public static class BinaryDump
    {
        public static void Dump(byte[] data, TextWriter target, Options options = null)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            bool showHeaders = options?.ShowHeaders ?? true;
            bool showAscii = options?.ShowASCII ?? true;

            string hformat = options?.Uppercase ?? true ? "X" : "x";
            string format = hformat + "2";
            string aformat = null;

            if (showHeaders)
            {
                int leftWidth = Math.Max(data.Length.ToString("X").Length, 2);
                aformat = $"{{0,{leftWidth}:{hformat}}}-   ";

                target.Write(new string(' ', leftWidth + 3));

                for (int i = 0; i < 16; i++)
                {
                    target.Write(" -");
                    target.Write(i.ToString(hformat));
                }

                target.WriteLine();
            }

            for (int i = 0; i < data.Length; i += 16)
            {
                if (showHeaders)
                {
                    target.Write(aformat, i >> 4);
                }

                for (int j = 0; j < 16; j++)
                {
                    target.Write((i + j < data.Length) ? data[i + j].ToString(format) : "  ");
                    target.Write(' ');
                }

                if (showAscii)
                {
                    target.Write("   ");
                    for (int j = 0; j < 16 && (i + j) < data.Length; j++)
                    {
                        byte b = data[i + j];
                        target.Write(b >= ' ' && b <= '~' ? (char)b : '.');
                    }
                }

                target.WriteLine();
            }
        }

        public static void DumpToConsole(byte[] data, Options options = null)
        {
            Dump(data, Console.Out, options);
        }

        public class Options
        {
            public bool ShowHeaders { get; set; } = true;
            public bool ShowASCII { get; set; } = true;
            public bool Uppercase { get; set; } = true;
        }
    }
}
