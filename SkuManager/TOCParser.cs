using System.IO.Abstractions;

namespace SkuManager;

public class TOCParser
{
    private IFileSystem fileSystem;
    public string Interface { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Version { get; private set; }
    public List<string> LuaFiles { get; private set; }

    public TOCParser(string filePath, IFileSystem? fileSystem = null)
    {
        this.fileSystem = fileSystem ?? new FileSystem();
        Interface = "";
        Title = "";
        Author = "";
        Version = "";
        LuaFiles = new List<string>();

        ParseTOCFile(filePath);
    }

    private void ParseTOCFile(string filePath)
    {
        try
        {

            using (StreamReader reader = new StreamReader(fileSystem.FileStream.New(filePath, FileMode.Open)))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("## Interface:"))
                        Interface = line.Substring(14).Trim();
                    else if (line.StartsWith("## Title:"))
                        Title = line.Substring(10).Trim();
                    else if (line.StartsWith("## Author:"))
                        Author = line.Substring(11).Trim();
                    else if (line.StartsWith("## Version:"))
                        Version = line.Substring(12).Trim();
                    else if (!line.StartsWith("##") && !string.IsNullOrWhiteSpace(line))
                        LuaFiles.Add(line.Trim());
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error while parsing {filePath}", e);
        }
    }
}
