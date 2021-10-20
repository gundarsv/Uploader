using Microsoft.Extensions.Configuration;
using System;

namespace Uploader.Core.Configurations
{
    public abstract class ConfigurationBase
    {
        protected readonly IConfiguration configuration;

        protected ConfigurationBase(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected abstract string SectionName { get; }

        private string GetValue(string key)
        {
            return string.IsNullOrEmpty(SectionName) ? configuration[key] : configuration.GetSection(SectionName)[key];
        }

        public string GetString(string key)
        {
            return GetValue(key);
        }
    }
}
