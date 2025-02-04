﻿namespace GroceryShop.Common
{
    public static class GlobalConstants
    {
        public const string DefaultConnection = "DefaultConnection";
        public const string AppSettingsJson = "appsettings.json";

        public const string AutoMapperAssemblyName = "GroceryShop.Web.ViewModels";
        public const string AutoMapperReflectionProfile = "ReflectionProfile";

        public const string NotFoundUri = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
        public const string InternalServerErrorUri = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        public const string BadRequestUri = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public const string NotFoundTitle = "Not Found";
        public const string InternalServerErrorTitle = "Internal Server Error";
        public const string BadRequestTitle = "Bad Request";

        public const string ApplicationProblemJson = "application/problem+json";

        public const string InvalidQueryCount = "Amount of items to show cannot be less than 0 and more than 50.";

        public const string ObjectWithIdNotFound = "{0} with id {1} not found.";

        public const string InvalidProductAmount = "Amount of products must be at least 1.";
        public const string InvalidProductName = "Product name cannot be null or more than 50 characters long.";
        public const string InvalidProductPrice = "Product price cannot be less than 0.";
        public const string ProductNotFound = "Product {0} not found.";
        public const string ProductAlreadyExists = "Product {0} already exists.";
        public const string ProductAlreadyInDeal = "Product {0} is already in deal.";
    }
}
