using SkuManager.UI.Models;
using System.Collections.Generic;

using SkuManager.UI.Utils;
using SkuManager.UI.Resources.Strings;

namespace SkuManager.UI.ViewModels;
public class MainViewModel : PropertyChangedBase
{
    private List<MainPageAction> _actions;
    private string _currentActionDescription;

    public List<MainPageAction> Actions
    {
        get
        {
            return _actions;
        }
        set
        {
            if (_actions == value)
            {
                return;
            }
            _actions = value;
            NotifyOfPropertyChange();
        }
    }

    public string CurrentActionDescription
    {
        get
        {
            return _currentActionDescription;
        }
        set
        {
            if (_currentActionDescription == value)
            {
                return;
            }
            _currentActionDescription = value;
            NotifyOfPropertyChange();
        }
    }

    public MainViewModel()
    {
        _actions = new List<MainPageAction>
        {
            new MainPageAction(LocalizableStrings.mainwindow_action_update_title, LocalizableStrings.mainwindow_action_update_description, MainPageActions.UPDATE),
            new MainPageAction(LocalizableStrings.mainwindow_action_install_title, LocalizableStrings.mainwindow_action_install_description, MainPageActions.INSTALL)
        };
        _currentActionDescription = LocalizableStrings.mainwindow_actionDescription_placeholder;
    }
}
