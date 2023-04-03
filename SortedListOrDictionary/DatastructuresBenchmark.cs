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
    public class DatastructuresBenchmark
    {
        [Params(10000, 100000)]
        public int N;
        
        private List<KeyVal> _randomWords = new();

        private SortedList<string, string> _sortedList = new();
        private SortedDictionary<string, string> _sortedDictionary = new();

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
           
            
            _randomWords = faker.Generate(N - 3);
            _randomWords.Add(new KeyVal(_firstKey,"first"));
            _randomWords.Add(new KeyVal(_secondKey,"second"));
            _randomWords.Add(new KeyVal(_thirdKey,"third"));
            
            _sortedList = CreateSortedListOneByOneWithOrderedData(); //faster
            _sortedDictionary = CreateSortedDictionaryOneByOne(); //faster

        }
        
        [Benchmark]
        public SortedList<string, string> CreateSortedListOneByOne()
        {
            var sortedList = new SortedList<string, string>();
            foreach (var word in _randomWords)
            {
                sortedList.TryAdd(word.Key, word.Value);
            }

            return sortedList;
        }
        
        [Benchmark]
        public SortedDictionary<string, string> CreateSortedDictionaryOneByOne()
        {
            var sortedDict = new SortedDictionary<string, string>();
            foreach (var word in _randomWords)
            {
                sortedDict.TryAdd(word.Key, word.Value);
            }

            return sortedDict;
        }
        
        //potential speedup
        [Benchmark]
        public SortedList<string, string> CreateSortedListOneByOneWithOrderedData()
        {
            var sortedList = new SortedList<string, string>();
            foreach (var word in _randomWords.OrderBy(rw => rw.Key))
            {
                sortedList.TryAdd(word.Key, word.Value);
            }

            return sortedList;
        }
        
        [Benchmark]
        public SortedDictionary<string, string> CreateSortedDictionaryOneByOneWithOrderedData()
        {
            var sortedDict = new SortedDictionary<string, string>();
            foreach (var word in _randomWords.OrderBy(rw => rw.Key))
            {
                sortedDict.TryAdd(word.Key, word.Value);
            }

            return sortedDict;
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
        
        
        [Benchmark] //compare against ordinary list
        public int StartsWithSearchInList()
        {
            return _randomWords.Count(kvp => kvp.Key.StartsWith("s"));
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
        public string ExactKeyStartsWithSearchInSortedDictionary()
        {
            _sortedDictionary.TryGetValue(_firstKey, out var first);
            _sortedDictionary.TryGetValue(_secondKey, out var second);
            _sortedDictionary.TryGetValue(_thirdKey, out var third);

            return first + second + third;
        }
        
    }
}