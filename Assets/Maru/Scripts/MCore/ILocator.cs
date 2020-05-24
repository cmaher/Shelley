namespace Maru.MCore {
    public interface ILocator {
        void Set(string key, object value);
        object Get(string key);
        bool Remove(string key);
    }
}
