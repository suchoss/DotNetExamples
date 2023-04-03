using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IEnumerableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            const int count = 100;
            var me = new MyEnumerable
            {
                HowMany = count
            };

            var range = Enumerable.Range(0, count);
            var list = range.ToList();
            var myEnum = me.Enumerate();


            Console.WriteLine("Range (enum)");
            StopWatchTime(range);
            Console.WriteLine("List");
            StopWatchTime(list);
            Console.WriteLine("CustomEnum Yielding");
            StopWatchTime(myEnum);


            // test nad linq dotazem
            var linqRange = range.Select(m =>
            {
                Console.WriteLine($"lr{m}");
                return m;
            });
            var linqList = list.Select(m =>
            {
                Console.WriteLine($"ll{m}");
                return m;
            });
            var linqMyEnum = myEnum.Select(m =>
            {
                Console.WriteLine($"lme{m}");
                return m;
            });

            Console.WriteLine("---------------------");
            Console.WriteLine("Range linq");
            StopWatchTime(linqRange);
            Console.WriteLine("List linq");
            StopWatchTime(linqList);
            Console.WriteLine("CustomEnum linq");
            StopWatchTime(linqMyEnum);

            Console.WriteLine("---------------------");
            Console.WriteLine("Range linq");
            StopWatchTime(linqRange.ToList());
            Console.WriteLine("List linq");
            StopWatchTime(linqList.ToList());
            Console.WriteLine("CustomEnum linq");
            StopWatchTime(linqMyEnum.ToList());

        }

        private static void StopWatchTime(IEnumerable<int> collection)
        {
            long elapsedTime;
            var sw = new Stopwatch();
            sw.Start();
            var memberCount = collection.Count();
            sw.Stop();
            elapsedTime = sw.ElapsedMilliseconds;
            sw.Restart();
            var memberCount2 = collection.Count();
            sw.Stop();
            elapsedTime += sw.ElapsedMilliseconds;
            
            Console.WriteLine($"Celkem běželo [{elapsedTime}], počet členů 1: {memberCount}, počet členů 2: {memberCount2}.");
        }
    }
    
    

    class MyEnumerable
    {
        public int HowMany { get; set; }
        
        public IEnumerable<int> Enumerate()
        {
            for (var i = 0; i < HowMany; i++)
            {
                Console.WriteLine($"Vracim z enumerable {i}");
                yield return i;
            }
        }
    }

}