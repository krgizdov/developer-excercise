namespace GroceryShop.Web.Infrastructure.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public ObjectNotFoundException() { }

        public ObjectNotFoundException(string message) : base(message) { }
    }
}
