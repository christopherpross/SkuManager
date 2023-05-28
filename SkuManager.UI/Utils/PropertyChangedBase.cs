﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SkuManager.UI.Utils;
public abstract class PropertyChangedBase : INotifyPropertyChanged
{
    protected void NotifyOfPropertyChange([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
