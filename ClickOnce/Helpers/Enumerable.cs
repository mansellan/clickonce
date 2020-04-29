using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClickOnce
{
    public abstract class Enumerable<T> : IEnumerable<T>
    {
        private readonly Lazy<IEnumerable<Func<T>>> values;

        protected Enumerable()
        {
            values = new Lazy<IEnumerable<Func<T>>>(() =>
            {
                var list = new List<Func<T>>();
                foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    if (typeof(T).IsAssignableFrom(property.PropertyType))
                    {
                        list.Add(() => (T)property.GetValue(this));
                    }
                }
                return list;
            });
        }
        public IEnumerator<T> GetEnumerator() => values.Value.Select(func => func.Invoke()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

