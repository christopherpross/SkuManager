using SkuManager.UI.Utils;
using Microsoft.Extensions.Logging;
using System.Windows.Documents;
using System.Collections.Generic;
using SkuManager.AddonManifests;

namespace SkuManager.UI.ViewModels;

public class SettingsViewModel : PropertyChangedBase
{
    private readonly ILogger<SettingsViewModel> _logger;
    
    public AddonManifestManager addonManifestManager;

    private List<AddonManifest> _addonManifests;

    public List<AddonManifest> AddonManifests
    {
        get
        {
            return _addonManifests;
        }
        set
        {
            if (_addonManifests == value)
            {
                return;
            }
            _addonManifests = value;
            NotifyOfPropertyChange();
        }
    }

    public SettingsViewModel(ILogger<SettingsViewModel> logger, AddonManifestManager addonManifestManager)
    {
        _logger = logger;
        this.addonManifestManager = addonManifestManager;
        _addonManifests = this.addonManifestManager.LoadAddonManifests(PathHelper.ManifestDirectory);
        if (_addonManifests is null)
        {
            _addonManifests = new List<AddonManifest>();
        }
        

    }
}
