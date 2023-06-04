using System;
using System.Windows;

using Microsoft.Extensions.Logging;
using System.ComponentModel;
using SkuManager.UI.ViewModels;
using System.Windows.Controls;
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;
using SkuManager.UI.Resources.Strings;

namespace SkuManager.UI.Views;
/// <summary>
/// Interaktionslogik für SettingsView.xaml
/// </summary>
public partial class SettingsView : Window
{
    private ILogger<SettingsView> logger;
    private SettingsViewModel _settingsViewModel;

    public Window? ParentWindow { get; set; }

    public SettingsView(ILogger<SettingsView> logger, SettingsViewModel viewModel)
    {
        this.logger = logger;
        this._settingsViewModel = viewModel; ;
        InitializeComponent();
        this.DataContext = _settingsViewModel;
        this.ContentRendered += OnContentRendered;
        this.Closed += OnClosed;
        this.Closing += OnClosing;
        this.IsVisibleChanged += SettingsView_IsVisibleChanged;
        if (!string.IsNullOrEmpty(Properties.Settings.Default.AddonManifest))
        {
            string targetName = Properties.Settings.Default.AddonManifest;

            int index = _settingsViewModel.AddonManifests.FindIndex(manifest => manifest.Name == targetName);

            if (index != -1)
            {
                manifestPicker.SelectedIndex = index;
            }
        }
    }

    private void SettingsView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        logger.LogInformation("{windowname} visibility changed from {old} to {new}", Name, e.OldValue, e.NewValue);
        if (!string.IsNullOrEmpty(Properties.Settings.Default.AddonManifest))
        {
            string targetName = Properties.Settings.Default.AddonManifest;

            int index = _settingsViewModel.AddonManifests.FindIndex(manifest => manifest.Name == targetName);
            if (index != -1)
            {
                manifestPicker.SelectedIndex = index;
            }
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
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        logger.LogInformation("{windowname} closing", Name);
    }

    private void manifestPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        try
        {
            ComboBox source = (ComboBox)sender;
            var selectedIndex = source.SelectedIndex;
            var manifestName = _settingsViewModel.AddonManifests[selectedIndex].Name;
            Properties.Settings.Default.AddonManifest = manifestName;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while saving addonmanifest setting");
        }
    }

    private void okButton_Click(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.Save();
        this.Close();
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.Reload();
        this.Close();
    }

    private void wowInterfacePathSearchButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog();
        dialog.Description = LocalizableStrings.folderDialog_wowInterfacePath_description;
        dialog.UseDescriptionForTitle = true;
        if ((bool)dialog.ShowDialog(this))
        {
            Properties.Settings.Default.WoWInterfaceFolderPath = dialog.SelectedPath;
        }

    }

    private void wowMenuPathSearchButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog();
        dialog.Description = LocalizableStrings.folderDialog_wowMenuPath_description;
        dialog.UseDescriptionForTitle = true;
        if ((bool)dialog.ShowDialog(this))
        {
            Properties.Settings.Default.WoWMenuPath = dialog.SelectedPath;
        }

    }
}
