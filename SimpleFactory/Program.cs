// See https://aka.ms/new-console-template for more information

using SimpleFactory;

Console.WriteLine("Hello, World!");


async Task<string> SimpleFunc()
{
    await Task.Delay(200);
    return DateTime.Now.ToString("hh:mm:ss");
}

#region Example1

Console.WriteLine("First variant");
var autoRefreshCache = await SimpleAutorefresh<string>.AsynchronousFactory(SimpleFunc, 5000);

for (int i = 0; i < 20; i++)
{
    await Task.Delay(1000);
    Console.WriteLine(autoRefreshCache.StoredValue);
}

#endregion

#region Example2

Console.WriteLine("And now second variant");

var refreshCacheWhenOld = await SimpleRefreshWhenOld<string>.AsynchronousFactory(SimpleFunc, 5000);

for (int i = 0; i < 20; i++)
{
    await Task.Delay(1000);
    Console.WriteLine($"async {await refreshCacheWhenOld.GetAsync()}");
    //Console.WriteLine($"sync {refreshCacheWhenOld.Get()}");

    Console.WriteLine($"universal {await refreshCacheWhenOld.GetUniversal()}");
}

#endregion