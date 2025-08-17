using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace S.Player.Utils.Options;

public class WritableOptions<T>(
    IHostEnvironment environment,
    IOptionsMonitor<T> options,
    string section,
    string file)
    : IWritableOptions<T>
    where T : class, new()
{
    public T Value => options.CurrentValue;

    public T Get(string? name)
    {
        return options.Get(name);
    }

    public void Update(Action<T> applyChanges)
    {
        var fileProvider = environment.ContentRootFileProvider;
        var fileInfo = fileProvider.GetFileInfo(file);
        var physicalPath = fileInfo.PhysicalPath;

        if (physicalPath != null)
        {
            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            var sectionObject = jObject != null && jObject.TryGetValue(section, out var value)
                ? JsonConvert.DeserializeObject<T>(value.ToString())
                : Value;

            if (sectionObject != null)
            {
                applyChanges(sectionObject);

                if (jObject != null)
                {
                    jObject[section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
                    File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
                }
            }
        }
    }
}