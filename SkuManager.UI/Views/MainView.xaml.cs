using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Microsoft.Extensions.Logging;

using SkuManager.UI.Models;
using SkuManager.UI.Resources.Strings;
using SkuManager.UI.Utils;
using System;
using SkuManager.UI.ViewModels;
using Ookii.Dialogs.Wpf;
using System.Diagnostics;

namespace SkuManager.UI.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainView : Window
{
    private ILogger<MainView> logger;

    private MainViewModel _viewModel;
    private IServiceProvider _serviceProvider;

    public MainView(ILogger<MainView> logger, IServiceProvider serviceProvider, MainViewModel viewModel)
    {
        this.logger = logger;
        this._serviceProvider = serviceProvider;
        this._viewModel = viewModel;
        this.ContentRendered += OnContentRendered;
        this.Closed += MainView_Closed;
        this.IsVisibleChanged += MainView_IsVisibleChanged;
        InitializeComponent();
        DataContext = _viewModel;
    }

    private void MainView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        logger.LogInformation("{windowname} visibility changed from {old} to {new}", Name, e.OldValue, e.NewValue);
    }

    private void OnContentRendered(object? sender, EventArgs e)
    {
        logger.LogInformation("{windowname} loaded", Name);
    }

    private void MainView_Closed(object? sender, System.EventArgs e)
    {
        logger.LogInformation("{windowname} closing", Name);
        App.Current.Shutdown();
    }

    private void actionPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        try
        {
            ComboBox sourceComboBox = (ComboBox)sender;
            int selectedIndex = sourceComboBox.SelectedIndex;
            MainPageAction? action = sourceComboBox.SelectedItem as MainPageAction;
            if (action != null)
            {
                _viewModel.CurrentActionDescription = action.Description ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while trying to set action description in MainView.");
        }
    }

    private void moreButton_Click(object sender, RoutedEventArgs e)
    {
        moreButton.ContextMenu.IsOpen = true;
    }

    private void moreMenuItemSkuWebsite_Click(object sender, RoutedEventArgs e)
    {
            BrowserHelper.OpenBrowser("https://duugu.github.io/Sku/");
    }

    private void moreMenuItemSkuRepo_Click(object sender, RoutedEventArgs e)
    {
        BrowserHelper.OpenBrowser("https://github.com/Duugu/Sku");
    }

    private void moreMenuItemSkuManagerRepo_Click(object sender, RoutedEventArgs e)
    {
        BrowserHelper.OpenBrowser("https://github.com/christopherpross/SkuManager");
    }

    private void moreMenuItemSkuDiscord_Click(object sender, RoutedEventArgs e)
    {
        BrowserHelper.OpenBrowser("https://discord.gg/AzFwTDmpbk");
    }

    private void optionsButton_Click(object sender, RoutedEventArgs e)
    {
        var settingsView = _serviceProvider.GetService(typeof(SettingsView)) as SettingsView;
        if (settingsView is not null)
        {
            settingsView.ParentWindow = this;
            settingsView.Owner = this;
            settingsView?.ShowDialog();
        } else {
            logger.LogWarning("could not resolve SettingsView object");
        }  
    }

    private void okButton_Click(object sender, RoutedEventArgs e)
    {
        var settingsCheckResult = _viewModel.CheckSettings();
        if (settingsCheckResult != string.Empty)
        {
            MessageBox.Show(string.Format(LocalizableStrings.settings_error_mainMessage, settingsCheckResult), LocalizableStrings.settings_error_caption, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var selectedAction = actionPicker.SelectedItem as MainPageAction;
        if (selectedAction == null) return;
        if (selectedAction.Action == MainPageActions.UPDATE)
        {
            _viewModel.ExecuteUpdate(this);
        }
    }
}
