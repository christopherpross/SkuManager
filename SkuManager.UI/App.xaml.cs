using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using SkuManager.UI.Utils;
using SkuManager.UI.ViewModels;
using SkuManager.UI.Views;

using System;
using System.IO;
using System.Windows;
using System.Text;
using System.Reflection;

namespace SkuManager.UI;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .UseSerilog((host, loggerConfiguration) =>
            {
                setupSerilog(loggerConfiguration);
            })
            .ConfigureServices(services =>
            {              
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainView>(s => new MainView()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();
        Log.Information("starting SkuManager {version} on {os}.", Assembly.GetExecutingAssembly().GetName().Version?.ToString(), Environment.OSVersion);

        MainWindow = _host.Services.GetRequiredService<MainView>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        Log.CloseAndFlush();
        base.OnExit(e);
    }

    private LoggerConfiguration setupSerilog(LoggerConfiguration loggerConfiguration)
    {
        var flushInterval = new TimeSpan(0, 0, 1);
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
        var file = Path.Combine(PathHelper.LogDirectory, $"{timestamp}.log");

        return loggerConfiguration
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.File(file, encoding: Encoding.UTF8);
    }
}
