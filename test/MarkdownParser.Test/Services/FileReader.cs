using System.Reflection;

namespace MarkdownParser.Test.Services;

static class FileReader
{
    public static string ReadFile(string filepath)
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MarkdownParser.Test.Resources.Examples." + filepath))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
