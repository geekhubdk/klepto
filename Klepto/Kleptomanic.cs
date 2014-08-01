    using System;
    using System.Collections;
    using System.Collections.Generic;
using System.Linq;
using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

namespace Klepto
{
    public class Kleptomanic
    {
        public static SchemaRegistation<T> RegisterSchema<T>() where T:class
        {
            return new SchemaRegistation<T>();
        }
    }

    public class SchemaRegistation<T> where T:class
    {
        private readonly List<ISchemaProvider<T>> _providers;

        public SchemaRegistation()
        {
            _providers = new List<ISchemaProvider<T>>();
        }

        public SchemaRegistation<T> AddProvider(ISchemaProvider<T> provider)
        {
            _providers.Add(provider);
            return this;
        }

        public T GetResult(string url)
        {
            foreach (var provider in _providers) {
                var result = provider.GetResult(url);
                
                if (result != null) {
                    return result;
                }
            }

            return null;
        }
    }

    public interface ISchemaProvider<T>
    {
        T GetResult(string url);
    }
}
