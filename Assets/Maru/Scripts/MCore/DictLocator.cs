using System.Collections.Generic;

namespace Maru.MCore
{
    public class DictLocator: ILocator
    {
        private readonly Dictionary<string, object> items;
        
        public DictLocator()
        {
            items = new Dictionary<string, object>();
        }
        
        public void Set(string key, object value)
        {
            items[key] = value;
        }

        public object Get(string key)
        {
            if (!items.ContainsKey(key))
            {
                throw new KeyNotFoundException("No value entered for key: " + key);
            }
            return items[key];
        }

        public bool Remove(string key)
        {
            return items.Remove(key);
        }
    }
}
