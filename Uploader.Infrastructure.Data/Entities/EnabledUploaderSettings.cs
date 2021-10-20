using System;
using Uploader.Infrastructure.Data.Entities.Base;

namespace Uploader.Infrastructure.Data.Entities
{
    public class EnabledUploaderSettings : Entity
    {
        public int Id { get; set; }

        public int EnabledSettingsId { get; set; }

        public UploaderSettings EnabledSettings { get; set; }

        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
