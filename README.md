Hystrix.NET Dashboard
=====================



# Quick setup

## Setup the dashboard
1. Create a MVC app
2. Make Windows happy, as admin => `netsh http add urlacl url=http://+:9000/ user=Everyone listen=yes`
3. Use the following code in the `Startup`

```
public partial class Startup
{
    public void Configuration(IAppBuilder app)
    {
        try
        {
            HystrixDashboard.Selfhost("http://+:9000/");
        }
        catch (HttpListenerException ex)
        {
            string command = $"netsh http add urlacl url=http://+:9000/ user=Everyone listen=yes";
            string exMessage = "Unable to start listener. Try: " + command;
            log.FatalException(exMessage, ex);
            throw new TargetInvocationException(exMessage, ex);
        }

        ...
    }
}
```

## Setup Hystrix
1. In the app which uses Hystrix.NET install Hystrix.NET.MetricsEventStream nuget package
2. Make Windows happy, as admin => `netsh http add urlacl url=http://+:10900/ user=Everyone listen=yes`
3. Create Hystrix startup class

```
public static class HystrixStartup
{
    static HystrixMetricsStreamServer metricsServer;

    public static void Start()
    {
        try
        {
            metricsServer = new HystrixMetricsStreamServer(@"http://+:10900/my-app/", 20, TimeSpan.FromSeconds(1));
            metricsServer.Start();
        }
        catch (Exception ex)
        {
            //log.Error(ex);
            throw;
        }
    }

    public static void Stop()
    {
        try
        {
            metricsServer.Stop();
        }
        catch (Exception ex)
        {
            //log.Fatal("Unable to stop Hystrix properly. Exiting without crashing. There is a chance that some resources are not disposed properly.", ex);
        }
    }
}
```

4. Initialize hystrix in the app `Startup`
```
HystrixStartup.Start(null);
var properties = new AppProperties(app.Properties);
CancellationToken token = properties.OnAppDisposing;
if (token != CancellationToken.None)
{
    token.Register(() =>
    {
        HystrixStartup.Stop();
    });
}
```
