using System;
using System.Collections.Generic;

namespace SortedSetSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var index = new SortedSet<string>(new StartsWithComparer())
            {
                "adéla",
                "adam",
                "adriana",
                "bedřich",
                "bárunka",
                "ba",
                "bb",
                "bx",
                "bz",
                "barunka",
                "bažant",
                "bbeďar",
                "berenika",
                "byvoj",
                "cabala",
                "candát",
                "canc",
                "cacziky",
                "dada",
                "daba",
                "daca",
                "dbaba"
            };

            var r1 = index.GetViewBetween("ba", "ba");
            var r2 = index.GetViewBetween("ba", "bb");
            var r3 = index.GetViewBetween("baa", "baz");
            var r4 = index.GetViewBetween("ba", "baa");
            
            Console.WriteLine("done");
            
        }
        
        private class StartsWithComparer :IComparer<string>
        {
            public int Compare(string? x, string? y)
            {
                
                if (x!.StartsWith(y!))
                    return 0;

                return string.CompareOrdinal(x, y);
            }
        }
    }
}