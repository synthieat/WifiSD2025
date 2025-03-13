using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Base
{
    public abstract class BaseHandler
    {
        protected void MapEntityProperties<TSource, TTarget>(TSource source, TTarget target, List<string> excludeProperties = default)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();  

            if(sourceType.BaseType.FullName != targetType.BaseType.FullName)
            {
                throw new ApplicationException("Base types are not matching");
            }

            var targetPropertyInfos = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            targetPropertyInfos.ForEach(p =>
            {
                if(p.CanWrite && !(excludeProperties ?? []).Contains(p.Name))
                {
                    /* Passendes Property aus Quelle (Source) lesen */
                    var sourceProperty = sourceType.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (sourceProperty != null)
                    {
                        /* Property Wert aus Quelle lesen */
                        var sourcePropertyValue = sourceProperty.GetValue(source, null);
                        /* Ausgelesene Wert in Ziel (Target) schreiben */
                        p.SetValue(target, sourcePropertyValue, null);
                    }                    
                }
            });
        }

    }
}
