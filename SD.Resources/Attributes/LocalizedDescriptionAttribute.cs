using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SD.Resources.Attributes
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string resourceKey;
        private static ResourceManager _resourceManager { get; set; }

        public LocalizedDescriptionAttribute(string resourceKey)
        {
            this.resourceKey = resourceKey;
        }

        public override string Description
        {
            get => this.resourceKey != null ? _resourceManager.GetString(this.resourceKey) : null;
        }

        public static void Setup (ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
    }
}
