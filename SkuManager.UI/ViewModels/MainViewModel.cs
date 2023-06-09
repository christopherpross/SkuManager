using SkuManager.UI.Models;
using System.Collections.Generic;
using System;

using SkuManager.UI.Utils;
using SkuManager.UI.Resources.Strings;
using System.Text;
using SkuManager.Update;
using System.Windows;
using Ookii.Dialogs.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkuManager.UI.Properties;
using SkuManager.AddonManifests;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using SkuManager.UI.Views;

namespace SkuManager.UI.ViewModels;
public class MainViewModel : PropertyChangedBase
{
    private List<MainPageAction> _actions;
    private string _currentActionDescription;
    private IServiceProvider _serviceProvider;
    private AddonManifestManager _addonManifestManager;

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


    public MainViewModel(IServiceProvider serviceProvider, AddonManifestManager addonManifestManager)
    {
        _actions = new List<MainPageAction>
        {
            new MainPageAction(LocalizableStrings.mainwindow_action_update_title, LocalizableStrings.mainwindow_action_update_description, MainPageActions.UPDATE),
            new MainPageAction(LocalizableStrings.mainwindow_action_install_title, LocalizableStrings.mainwindow_action_install_description, MainPageActions.INSTALL)
        };
        _currentActionDescription = LocalizableStrings.mainwindow_actionDescription_placeholder;
        this._serviceProvider = serviceProvider;
        _addonManifestManager = addonManifestManager;
    }

    public string CheckSettings()
    {
        if (string.IsNullOrEmpty(Properties.Settings.Default.AddonManifest))
            return LocalizableStrings.settings_error_manifestIsNull;

        if (string.IsNullOrEmpty(Properties.Settings.Default.WoWInterfaceFolderPath))
            return LocalizableStrings.settings_error_wowInterfacePathIsNull;

        if (string.IsNullOrEmpty(Properties.Settings.Default.WoWMenuPath))
            return LocalizableStrings.settings_error_wowMenuPathIsNull;

        if (!Properties.Settings.Default.WoWInterfaceFolderPath.EndsWith("interface", StringComparison.OrdinalIgnoreCase))
            return LocalizableStrings.settings_error_wowInterfacePathEndsNotWithInterface;

        return string.Empty;
    }

    public void ExecuteUpdate(Window parentWindow)
    {
        ILogger<GithubAddonUpdater>? updaterLogger = _serviceProvider.GetService(typeof(ILogger<GithubAddonUpdater>)) as ILogger<GithubAddonUpdater>;
        var updaterOptions = new GithubAddonUpdaterOptions(
           AddonManifest: _addonManifestManager.GetAddonManifestByName(Settings.Default.AddonManifest, PathHelper.ManifestDirectory),
           WoWPathInterface: Settings.Default.WoWInterfaceFolderPath,
           WoWPathAddons: PathHelper.WoWPathAddonFolder,
           WoWPathWTF: PathHelper.WoWPathWTFFolder,
           WoWMenuPath: Settings.Default.WoWMenuPath);

if (updaterLogger is null) updaterLogger = new NullLogger<GithubAddonUpdater>();

        GithubAddonUpdater updater = new GithubAddonUpdater(updaterLogger,
                                                            new Octokit.GitHubClient(new Octokit.ProductHeaderValue("SkuManager")),
                                                            updaterOptions)
            ;
        var updateCheckProgressDialog = new ProgressDialog
        {
            WindowTitle = LocalizableStrings.updateCheckDialog_title,
            Text = LocalizableStrings.updateCheckDialog_text,
            ShowTimeRemaining = true,
            ShowCancelButton = true
        };
        
        bool updatesAvaiable = false;
        updateCheckProgressDialog.DoWork += async (sender, e) =>
        {
            if (updateCheckProgressDialog.IsBusy == false) return;
            var results = updater.CheckForUpdates(sender as IProgress<int>, CancellationToken.None).GetAwaiter().GetResult();
            var updateCheckResults = results.ToList();
            e.Result = updateCheckResults;

            return;
        };

        updateCheckProgressDialog.RunWorkerCompleted += (sender, e) =>
        {
            var updateCheckResults = e.Result as List<UpdateCheckResult>;
            if (updateCheckResults is null || updateCheckResults.Count < 1)
            {
                MessageBox.Show(LocalizableStrings.updateCheck_noUpdatesAvailable);
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var updateCheckResult in updateCheckResults)
            {
                sb.AppendLine(updateCheckResult.ToString());
            }
            
            string? summary = sb.ToString();
            if (summary is null) summary = string.Empty;

            ILogger<ConfirmActionView>? confirmViewLogger = _serviceProvider.GetService(typeof(ILogger<ConfirmActionView>)) as ILogger<ConfirmActionView>;
            if (confirmViewLogger == null) confirmViewLogger = new NullLogger<ConfirmActionView>();

            var confirmationView = new ConfirmActionView(confirmViewLogger, new ConfirmActionViewModel(summary));
            var result = confirmationView.ShowDialog();
            if (result is not null && result == true)
            {
                MessageBox.Show("Würde updates durchführen");
            }

        };
        updateCheckProgressDialog.Show();
    }

    
}
