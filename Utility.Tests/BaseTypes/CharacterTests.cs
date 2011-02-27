using System;
using Utility.BaseTypes;

namespace Utility.Tests.BaseTypes
{
    /// <summary>
    /// These are NOT unit tests; they should be run from a console.
    /// </summary>
    public static class CharacterTests
    {
        public static void ShowChar()
        {
            Console.WriteLine('\''.Show());
            Console.WriteLine('\\'.Show());
            Console.WriteLine('\n'.Show());
            Console.WriteLine('\a'.Show());
            Console.WriteLine('\0'.Show());
            Console.WriteLine('\x00'.Show());
            Console.WriteLine('\x02'.Show());
            Console.WriteLine('"'.Show());
            Console.WriteLine(' '.Show());
            Console.WriteLine('\x10'.Show());
            Console.WriteLine('\x1a'.Show());
        }

        public static void ShowString()
        {
            string s1 = "abc\r\ndef.\r\nDon't use double quotes (\"\"),\r\nor single quotes ('') or \\ or \x00 or \x1A.";
            string s2 = @"abc
def.
Don't use double quotes (""""),
or single quotes ('') or \ or "
                +
                "\x00 or \x1A.";

            Console.WriteLine(s1 == s2);
            Console.WriteLine("\n");

            Console.WriteLine(s1.Show());
            Console.WriteLine();
            Console.WriteLine(s2.Show());
            Console.WriteLine("\n");

            Console.WriteLine(s1.ShowVerbatim());
            Console.WriteLine();
            Console.WriteLine(s2.ShowVerbatim());
            Console.WriteLine("\n");
        }
    }
}
