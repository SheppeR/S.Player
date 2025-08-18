using CodingSeb.Localization;
using CodingSeb.Localization.Loaders;

namespace S.Player.Utils.Helpers;

public class LocalizerHelper
{
    public static void ConfigureLocalizer(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Loc.Instance.CurrentLanguage = value;
        }

        LocalizationLoader.Instance.FileLanguageLoaders.Add(new JsonFileLoader());

        LocalizationLoader.Instance.AddFile("Resources/Localization.loc.json");
    }
}