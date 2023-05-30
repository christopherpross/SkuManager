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
                services.AddSingleton<MainView>();
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
        DeleteOldLogFiles();
        base.OnExit(e);
    }

    private LoggerConfiguration setupSerilog(LoggerConfiguration loggerConfiguration)
    {
        var flushInterval = new TimeSpan(0, 0, 1);
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var file = Path.Combine(PathHelper.LogDirectory, $"{timestamp}.log");

        return loggerConfiguration
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.File(file, encoding: Encoding.UTF8);
    }

    private void DeleteOldLogFiles()
    {
        string logDirectory = PathHelper.LogDirectory;
        DirectoryInfo directoryInfo = new DirectoryInfo(logDirectory);
        FileInfo[] logFiles = directoryInfo.GetFiles("*.log");

        int filesToKeep = 5;

        // Check if the number of log files is less than or equal to the desired number of files to keep
        if (logFiles.Length <= filesToKeep)
        {
            return; // There are no more log files than the number to be kept, so return without performing any further actions
        }

        // Sort the log files based on their file names in ascending order
        Array.Sort(logFiles, (f1, f2) => f1.Name.CompareTo(f2.Name));

        // Delete the excess log files before the last "filesToKeep" files
        for (int i = 0; i < logFiles.Length - filesToKeep; i++)
        {
            logFiles[i].Delete();
        }
    }

}
