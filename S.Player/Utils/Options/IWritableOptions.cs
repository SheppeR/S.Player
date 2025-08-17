using Microsoft.Extensions.Options;

namespace S.Player.Utils.Options;

public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
{
    void Update(Action<T> applyChanges);
}