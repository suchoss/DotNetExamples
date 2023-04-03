namespace SimpleFactory;

public class SimpleAutorefresh<T> where T : class
{
    private readonly Func<Task<T>> _asyncFunc;
    private readonly Func<T> _syncFunc;

    private readonly bool _isAsynchronous;

    public T? StoredValue { get; set; }

    private PeriodicTimer AutoRefreshTimer { get; }

    private Task AutoRefreshTask { get; set; }
    

    private SimpleAutorefresh(Func<Task<T>>? asyncFunc, Func<T>? syncFunc, bool isAsynchronous, int? refreshIntervalInMs)
    {
        _isAsynchronous = isAsynchronous;
        
        if (_isAsynchronous)
            _asyncFunc = asyncFunc ?? throw new ArgumentNullException("function");
        else
            _syncFunc = syncFunc ?? throw new ArgumentNullException("function");

        if (refreshIntervalInMs is not null && refreshIntervalInMs > 0)
        {
            AutoRefreshTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(refreshIntervalInMs.Value));
            AutoRefreshTask = AutoRefreshTaskBody();
        }
    }


    public static SimpleAutorefresh<T> SynchronousFactory(Func<T>? function, int? refreshIntervalInMs)
    {
        var simpleClass = new SimpleAutorefresh<T>(asyncFunc: null, syncFunc: function, isAsynchronous: false,
            refreshIntervalInMs);

        simpleClass.Populate();
        return simpleClass;
    }

    public static async Task<SimpleAutorefresh<T>> AsynchronousFactory(Func<Task<T>>? function, int? refreshIntervalInMs)
    {
        var simpleClass = new SimpleAutorefresh<T>(asyncFunc: function, syncFunc: null, true, refreshIntervalInMs);

        await simpleClass.PopulateAsync();
        return simpleClass;
    }

    private async Task PopulateAsync() => StoredValue = await _asyncFunc();
    private void Populate() => StoredValue = _syncFunc();


    private async Task AutoRefreshTaskBody()
    {
        while (await AutoRefreshTimer.WaitForNextTickAsync().AsTask())
        {
            if (_isAsynchronous)
                await PopulateAsync();
            else
                Populate();
        }
    }

    

    
}