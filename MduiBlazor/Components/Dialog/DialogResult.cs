namespace MduiBlazor
{
    public class DialogResult
    {
        public object? Data { get; }
        public Type? DataType { get; }
        public bool Cancelled { get; set; }
        public bool Confirmed => !Cancelled;

        private DialogResult(object? data, Type? resultType, bool cancelled)
        {
            Data = data;
            DataType = resultType;
            Cancelled = cancelled;
        }

        public static DialogResult Ok<T>(T result)
            => Ok(result, typeof(T));

        public static DialogResult Ok<T>(T result, Type? dataType)
            => new(result, dataType, false);

        public static DialogResult Ok()
            => new(null, null, false);

        public static DialogResult Cancel()
            => new(null, null, true);

        public static DialogResult Cancel<T>(T payload)
            => new(payload, null, true);
    }
}
