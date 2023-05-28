using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using SkuManager.UI.Models;
using SkuManager.UI.Resources.Strings;

namespace SkuManager.UI.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainView : Window
{
    public List<MainPageAction> Actions { get; set; }

    public MainView()
    {
        InitializeComponent();
        DataContext = this;
       
        Actions = new List<MainPageAction>
        {
            new MainPageAction(LocalizableStrings.mainwindow_action_update_title, LocalizableStrings.mainwindow_action_update_description, MainPageActions.UPDATE),
            new MainPageAction(LocalizableStrings.mainwindow_action_install_title, LocalizableStrings.mainwindow_action_install_description, MainPageActions.INSTALL)
        };
    }

    private void actionPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        ComboBox sourceComboBox = (ComboBox)sender;
        int selectedIndex = sourceComboBox.SelectedIndex;
        MainPageAction? action = sourceComboBox.SelectedItem as MainPageAction;
        if (action != null)
        {
            actionDescription.Text = action.Description;
        }
    }
}
