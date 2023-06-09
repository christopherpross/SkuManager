namespace SkuManager.UI.Models;

public class MainPageAction
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public MainPageActions Action { get; set; }

    public MainPageAction(string? title, string? description, MainPageActions action)
    {
        Title = title;
        Description = description;
        Action = action;
    }    
}
