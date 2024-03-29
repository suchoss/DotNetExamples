using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Bogus;
using Perfolizer.Mathematics.OutlierDetection;

namespace SortedListOrDictionary
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [Outliers(OutlierMode.DontRemove)]
    public class DatastructuresFindBenchmark
    {
        private List<KeyVal> _randomWords = new();
        private SortedList<string, string> _sortedList = new();
        private SortedDictionary<string, string> _sortedDictionary = new();
        private SortedSet<string> _sortedSet = new();

        private string _firstKey = "custom_created_field";
        private string _secondKey = "next_control_field";
        private string _thirdKey = "sweet_potato";
        
        [GlobalSetup]
        public void GlobalSetup()
        {
            //generate random data with maximum possible unique keys
            var faker = new Faker<KeyVal>()
                .RuleFor(p => p.Key, f => $"{f.Address.City()}_{f.Company.CompanyName()}_{f.Lorem.Word()}"  )
                .RuleFor(p => p.Value, f => f.Address.City());
           
            
            _randomWords = faker.Generate(100000 - 3);
            _randomWords.Add(new KeyVal(_firstKey,"first"));
            _randomWords.Add(new KeyVal(_secondKey,"second"));
            _randomWords.Add(new KeyVal(_thirdKey,"third"));

            _sortedList = new SortedList<string, string>(
                _randomWords.ToDictionary(kv => kv.Key,
                    kv => kv.Value)); 
            _sortedDictionary = new SortedDictionary<string, string>(
                _randomWords.ToDictionary(kv => kv.Key,
                    kv => kv.Value)); 
            _sortedSet = new SortedSet<string>(_randomWords.Select(rw => rw.Key));
        }
        
        [Benchmark]
        public int StartsWithSearchInSortedList()
        {
            return _sortedList.Count(kvp => kvp.Key.StartsWith("s"));
        }
        
        [Benchmark]
        public int StartsWithSearchInSortedDictionary()
        {
            return _sortedDictionary.Count(kvp => kvp.Key.StartsWith("s"));
        }
        
        [Benchmark]
        public int StartsWithSearchInSortedDictionaryCustomSearch()
        {
            using var enumerator = _sortedDictionary.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key.StartsWith("s"))
                {
                    count++;
                }
                else
                {
                    if (count > 0)
                        break;
                }
            }
            return count;
        }
        
        [Benchmark] //compare against ordinary list
        public int StartsWithSearchInList()
        {
            return _randomWords.Count(kvp => kvp.Key.StartsWith("s"));
        }
        
        [Benchmark] //compare against ordinary list
        public int StartsWithSortedSet()
        {
            return _sortedSet.GetViewBetween("s", "t").Count();
        }
        
        [Benchmark]
        public int ContainsSearchInSortedList()
        {
            return _sortedList.Count(kvp => kvp.Key.Contains("control"));
        }
        
        [Benchmark]
        public int ContainsSearchInSortedDictionary()
        {
            return _sortedDictionary.Count(kvp => kvp.Key.Contains("control"));
        }
        
        [Benchmark] //compare against ordinary list
        public int ContainsSearchInList()
        {
            return _randomWords.Count(kvp => kvp.Key.Contains("control"));
        }
        
        [Benchmark]
        public string ExactKeySearchInSortedList()
        {
            _sortedList.TryGetValue(_firstKey, out var first);
            _sortedList.TryGetValue(_secondKey, out var second);
            _sortedList.TryGetValue(_thirdKey, out var third);

            return first + second + third;
        }
        
        [Benchmark]
        public string ExactKeySearchInSortedDictionary()
        {
            _sortedDictionary.TryGetValue(_firstKey, out var first);
            _sortedDictionary.TryGetValue(_secondKey, out var second);
            _sortedDictionary.TryGetValue(_thirdKey, out var third);

            return first + second + third;
        }
        
        [Benchmark]
        public string ExactKeySearchInList()
        {
            var first = _randomWords.FirstOrDefault(rw => rw.Key == _firstKey);
            var second = _randomWords.FirstOrDefault(rw => rw.Key == _secondKey);
            var third = _randomWords.FirstOrDefault(rw => rw.Key == _thirdKey);
            
            return first.Value + second.Value + third.Value;
        }
        
    }
}