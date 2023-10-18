using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MduiBlazor
{
    public partial class MduiDateTime<TValue> : MduiInputBase<TValue>
    {
        protected string Classname =>
        new ClassBuilder("mdui-textfield-input")
            .AddClass(Class)
            .Build();

        [Parameter]
        public DateInputType Type { get; set; }

        [Parameter]
        public string? Pattern { get; set; }

        [Parameter]
        public string ParsingErrorMessage { get; set; } = "The {0} field must be a datetime.";

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
            else
            {
                validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
                return false;
            }
        }

        private string Format => Type switch
        {
            DateInputType.Date => "yyyy-MM-dd",
            DateInputType.Time => "HH:mm:ss",
            DateInputType.DateTime => "yyyy-MM-dd HH:mm:ss",
            DateInputType.Month => "yyyy-MM",
            _ => throw new InvalidOperationException($"Unsupported type {typeof(TValue)}")
        };

        private string? FormatDateValueAsString()
        {
            return Value switch
            {
                null => null,
                DateTime dateTime => dateTime.ToString(Format),
                DateOnly dateOnly => dateOnly.ToString(Format),
                TimeOnly timeOnly => timeOnly.ToString(Format),
                DateTimeOffset dateTimeOffset => dateTimeOffset.ToString(Format),
                _ => throw new InvalidOperationException($"Unsupported type {typeof(TValue)}")
            };
        }

        private void OnFocus()
        {
            Field?.SetFocus();
        }

        private void OnBlur()
        {
            Field?.SetBlur();
        }
    }
}
