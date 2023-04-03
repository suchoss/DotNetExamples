namespace SimpleFactory;

public class SimpleRefreshWhenOld<T> where T : class
{
    private readonly Func<Task<T>>? _asyncFunc;
    private readonly Func<T>? _syncFunc;

    private readonly bool _isAsynchronous;
    private readonly int _refreshIntervalInMs;

    private T? StoredValue { get; set; }
    private DateTime TimeStamp { get; set; }

    private bool IsOld => DateTime.Now > TimeStamp.AddMilliseconds(_refreshIntervalInMs);
 

    private SimpleRefreshWhenOld(Func<Task<T>>? asyncFunc, Func<T>? syncFunc, bool isAsynchronous, int refreshIntervalInMs)
    {
        _isAsynchronous = isAsynchronous;
        _refreshIntervalInMs = refreshIntervalInMs;
        
        if (_isAsynchronous)
            _asyncFunc = asyncFunc ?? throw new ArgumentNullException("function");
        else
            _syncFunc = syncFunc ?? throw new ArgumentNullException("function");

        
    }


    public static SimpleRefreshWhenOld<T> SynchronousFactory(Func<T>? function, int refreshIntervalInMs)
    {
        var simpleClass = new SimpleRefreshWhenOld<T>(asyncFunc: null, syncFunc: function, isAsynchronous: false,
            refreshIntervalInMs);

        simpleClass.Populate();
        return simpleClass;
    }

    public static async Task<SimpleRefreshWhenOld<T>> AsynchronousFactory(Func<Task<T>>? function, int refreshIntervalInMs)
    {
        var simpleClass = new SimpleRefreshWhenOld<T>(asyncFunc: function, syncFunc: null, true, refreshIntervalInMs);

        await simpleClass.PopulateAsync();
        return simpleClass;
    }

    private async Task PopulateAsync()
    {
        if (_asyncFunc != null)
        {
            StoredValue = await _asyncFunc();
            TimeStamp = DateTime.Now;
        }
    }

    private void Populate()
    {
        if (_syncFunc != null)
        {
            StoredValue = _syncFunc();
            TimeStamp = DateTime.Now;
        }
    }


    public async Task<T?> GetAsync()
    {
        if (IsOld)
        {
            await PopulateAsync();
        }

        return StoredValue;
    }
    
    public T? Get()
    {
        if (IsOld)
        {
            Populate();
        }

        return StoredValue;
    }

    public async ValueTask<T?> GetUniversal()
    {
        if (IsOld)
        {
            if (_isAsynchronous)
                await PopulateAsync();
            else
            {
                // ReSharper disable once MethodHasAsyncOverload
                Populate();
            }
        }

        return StoredValue;
    }

}