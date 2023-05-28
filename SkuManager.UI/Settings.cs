using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkuManager.UI;

public class Settings
{
    public string? PathWoWInterfaceFolder { get; set; }
    public string? PathWoWMenu { get; set; }

    public async Task SaveAsync(string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            await JsonSerializer.SerializeAsync<Settings>(sw.BaseStream, this, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            sw.Flush();
        }
    }

    public ValueTask<Settings?> Read(string filename)
    {
        if (File.Exists(filename))
        {
            using (StreamReader sw = new StreamReader(filename))
            {
                return JsonSerializer.DeserializeAsync<Settings>(sw.BaseStream, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
            }
        }
        else
        {
            // return default settings
            return new ValueTask<Settings?>(GetDefault());

        }
    }

    public Settings GetDefault()
    {
        var settings = new Settings
        {
            PathWoWInterfaceFolder = @"C:\Program Files (x86)\World of Warcraft\_classic_\Interface",
        };
        return settings;
    }
}
