using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityPlusCore
{
    public class PairSettings<K, V> : BaseSettings<List<Pair<K, V>>>
    {
        private List<Pair<K, V>> records;

        public PairSettings(string settingsPath) : base(settingsPath)
        {
            this.records = new List<Pair<K, V>>();
        }

        public override List<Pair<K, V>> LoadSettings()
        {
            if (this.records.Any())
            {
                this.records.Clear();
            }

            var result = base.LoadSettings();
            if (null != result)
            {
                this.records.AddRange(result);
            }

            return this.records;
        }

        private Pair<K, V> GetItem(K key)
        {
            var records = base.LoadSettings();
            if (key is string)
            {
                return records.FirstOrDefault(p => (p.Key as string).Equals(key as string, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return records.FirstOrDefault(p => p.Key.Equals(key));
            }
        }
    }
}
