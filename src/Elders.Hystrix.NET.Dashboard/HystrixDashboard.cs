using Elders.Hystrix.NET.Dashboard.Logging;
using Microsoft.Owin.Hosting;
using System;
using System.Reflection;

namespace Elders.Hystrix.NET.Dashboard
{
    public static class HystrixDashboard
    {
        static readonly ILog log = LogProvider.GetLogger(typeof(OwinConfig));

        static IDisposable app;

        public static void Selfhost(string address)
        {
            try
            {
                address = address.TrimEnd('/');
                if (app != null)
                    throw new InvalidOperationException("Hysrix Dashboard is already hosted. HystrixDashboard.Selfhost can be called only once");
                log.Info($"Started dashboard on {address}");
                var listener = typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener);
                app = WebApp.Start<OwinConfig>(url: address);
            }
            catch (TargetInvocationException ex)
            {
                string command = $"netsh http add urlacl url={address}/ user=Everyone listen=yes";
                string exMessage = "Unable to start listener. Try: " + command;
                log.FatalException(exMessage, ex);
                throw new TargetInvocationException(exMessage, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void Stop()
        {
            if (app != null)
                app.Dispose();
        }
    }
}
