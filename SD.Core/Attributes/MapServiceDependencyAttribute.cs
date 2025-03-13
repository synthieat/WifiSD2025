using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapServiceDependencyAttribute : Attribute
    {
        protected string name;

        public MapServiceDependencyAttribute(string name)
        {
            this.name = name;
        }
    }
}
