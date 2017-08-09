using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityPlusCore
{
    public class BaseSettings<T>
    {
        protected string settingsPath;

        public BaseSettings(string settingsPath)
        {
            if (null == settingsPath)
            {
                throw new ArgumentNullException(nameof(settingsPath));
            }

            if (string.IsNullOrWhiteSpace(settingsPath))
            {
                throw new ArgumentException("Settings path cannot be empty", nameof(settingsPath));
            }

            this.settingsPath = settingsPath;
        }

        public virtual T LoadSettings()
        {
            return XmlHelper.Deserialize<T>(File.ReadAllText(this.settingsPath));
        }

        public virtual void SaveSettings(T settings)
        {
            if(null == settings)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            File.WriteAllText(this.settingsPath, XmlHelper.Serialize(settings));
        }
    }
}
