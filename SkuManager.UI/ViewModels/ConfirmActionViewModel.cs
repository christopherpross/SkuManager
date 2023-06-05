using SkuManager.UI.Utils;

namespace SkuManager.UI.ViewModels;

public class ConfirmActionViewModel : PropertyChangedBase
{
    private string _confirmationText;

    public string ConfirmationText
    {
        get
        {
            return _confirmationText;
        }

        set
        {
            if (_confirmationText == value)
            {
                return;
            }
            _confirmationText = value;
            NotifyOfPropertyChange();
        }
    }

    public ConfirmActionViewModel(string confirmationText)
    {
        _confirmationText = confirmationText ?? string.Empty;
    }
}
