using System.ComponentModel;
using System;
using System.Windows;

using Microsoft.Extensions.Logging;

using Serilog.Core;

using SkuManager.UI.ViewModels;

namespace SkuManager.UI.Views;
/// <summary>
/// Interaktionslogik für ConfirmActionView.xaml
/// </summary>
public partial class ConfirmActionView : Window
{
    private readonly ILogger<ConfirmActionView> logger;
    private ConfirmActionViewModel _viewModel;

    public ConfirmActionView(ILogger<ConfirmActionView> logger, ConfirmActionViewModel viewModel)
    {
        this.logger = logger;
        _viewModel = viewModel;
        InitializeComponent();
        DataContext = _viewModel;
        this.ContentRendered += OnContentRendered;
        this.Closed += OnClosed;
        this.Closing += OnClosing;
        this.IsVisibleChanged += SettingsView_IsVisibleChanged;
    }

    private void SettingsView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        logger.LogInformation("{windowname} visibility changed from {old} to {new}", Name, e.OldValue, e.NewValue);
        if ((Visibility)e.NewValue == Visibility.Visible)
        {
            summaryTextbox.Focus();
        }
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // hide only window, that we have the possibility to open a second time
        this.Visibility = Visibility.Collapsed;
        e.Cancel = true;
    }

    private void OnContentRendered(object? sender, EventArgs e)
    {
        logger.LogInformation("{windowname} loaded", Name);
        summaryTextbox.Focus();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        logger.LogInformation("{windowname} closing", Name);
    }

    private void yesButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        this.Close();
    }

    private void noButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        this.Close();   
    }
}
