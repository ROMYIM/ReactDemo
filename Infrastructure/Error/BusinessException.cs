using System;
using System.ComponentModel;
using System.Reflection;

namespace ReactDemo.Infrastructure.Error
{
    [System.Serializable]
    public class BusinessException : System.Exception
    {
        public BusinessException() { }
        public BusinessException(ErrorCode code) : base(GetErrorCodeDescription(code)) { Code = code; }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, System.Exception inner) : base(message, inner) { }
        protected BusinessException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string GetErrorCodeDescription(ErrorCode code)
        {
            if (!Enum.IsDefined(typeof(ErrorCode), code))
            {
                throw new ArgumentException(nameof(code));
            }

            FieldInfo field = code.GetType().GetField(code.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute?.Description;
        }

        public ErrorCode? Code { get; }
    }
}