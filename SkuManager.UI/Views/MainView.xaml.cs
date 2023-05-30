using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Microsoft.Extensions.Logging;

using SkuManager.UI.Models;
using SkuManager.UI.Resources.Strings;
using SkuManager.UI.Utils;
using System;

namespace SkuManager.UI.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainView : Window
{
    private ILogger<MainView> logger;
    public List<MainPageAction> Actions { get; set; }

    public MainView(ILogger<MainView> logger)
    {
        this.logger = logger;
        this.Loaded += MainView_Loaded;
        this.Closed += MainView_Closed;
        InitializeComponent();
        DataContext = this;
       
        Actions = new List<MainPageAction>
        {
            new MainPageAction(LocalizableStrings.mainwindow_action_update_title, LocalizableStrings.mainwindow_action_update_description, MainPageActions.UPDATE),
            new MainPageAction(LocalizableStrings.mainwindow_action_install_title, LocalizableStrings.mainwindow_action_install_description, MainPageActions.INSTALL)
        };
    }

    private void MainView_Loaded(object sender, RoutedEventArgs e)
    {
        logger.LogInformation("MainView loaded");
    }

    private void MainView_Closed(object? sender, System.EventArgs e)
    {
        logger.LogInformation("close MainView");
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
                actionDescription.Text = action.Description;
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
}
