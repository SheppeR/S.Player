using System.IO;

namespace S.Player.Utils.Extensions;

public static class StringExtensions
{
    public static string Validate(this string folderName)
    {
        if (string.IsNullOrEmpty(folderName))
        {
            return folderName;
        }

        foreach (var c in Path.GetInvalidFileNameChars())
        {
            folderName = folderName.Replace(c.ToString(), string.Empty);
        }

        foreach (var c in Path.GetInvalidPathChars())
        {
            folderName = folderName.Replace(c.ToString(), string.Empty);
        }

        return folderName;
    }
}