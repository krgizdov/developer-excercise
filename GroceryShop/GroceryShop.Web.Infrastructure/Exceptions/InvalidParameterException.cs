namespace GroceryShop.Web.Infrastructure.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidParameterException : Exception
    {
        protected InvalidParameterException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public InvalidParameterException() { }

        public InvalidParameterException(string message) : base(message) { }
    }
}
