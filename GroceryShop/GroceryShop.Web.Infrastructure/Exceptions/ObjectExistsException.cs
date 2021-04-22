namespace GroceryShop.Web.Infrastructure.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ObjectExistsException : Exception
    {
        protected ObjectExistsException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public ObjectExistsException() { }

        public ObjectExistsException(string message) : base(message) { }
    }
}
