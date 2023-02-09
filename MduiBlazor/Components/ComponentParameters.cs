using System.Collections;

namespace MduiBlazor
{
    public class ComponentParameters : IEnumerable<KeyValuePair<string, object>>
    {
        internal readonly Dictionary<string, object> Parameters;

        public ComponentParameters()
        {
            Parameters = new Dictionary<string, object>();
        }

        public ComponentParameters Add(string parameterName, object value)
        {
            Parameters[parameterName] = value;
            return this;
        }

        public T Get<T>(string parameterName)
        {
            if (Parameters.TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"{parameterName} does not exist in the component parameters");
        }

        public T? TryGet<T>(string parameterName)
        {
            if (Parameters.TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            return default;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => Parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();
    }
}
