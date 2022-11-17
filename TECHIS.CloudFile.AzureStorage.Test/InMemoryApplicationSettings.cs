using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TECHIS.Core;

namespace Test.Cloud.AzureStorage
{
    internal class InMemoryApplicationSettings : IApplicationSettings
    {
        private readonly ReadOnlyDictionary<string, string> _Entries;

        internal InMemoryApplicationSettings(params (string key, string value)[] entries) 
        {
            if (entries is not null && entries.Any())
            {
                _Entries = new ReadOnlyDictionary<string, string>( new Dictionary<string, string>(entries.Select(entry => new KeyValuePair<string, string>(entry.key, entry.value))));
            }

            InputValidator.ArgumentNullOrEmptyCheck(entries, nameof(entries));
        }
        public string Get(string key)
        {
            if (_Entries.TryGetValue(key, out string value))
            {
                return value;
            }

            return default;
        }

        public TConfig Get<TConfig>(string sectionName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(Get(key));
        }

        public Task<TConfig> GetAsync<TConfig>(string sectionName)
        {
            throw new System.NotImplementedException();
        }
    }
}