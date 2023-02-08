namespace MduiBlazor
{
    public class SnackbarParameters
    {
        internal readonly Dictionary<string, object> Parameters;

        public SnackbarParameters()
        {
            Parameters = new Dictionary<string, object>();
        }
        public void Add(string parameterName, object value)
        {
            Parameters[parameterName] = value;
        }

        public T Get<T>(string parameterName)
        {
            if (Parameters.TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"{parameterName} does not exist in snackbar parameters");
        }

        public T? TryGet<T>(string parameterName)
        {
            if (Parameters.TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            return default;
        }
    }
}
