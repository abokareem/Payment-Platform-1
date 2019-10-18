using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Framework.Constants.Logger
{
    /// <summary>
    /// Константы для Identity Logger.
    /// </summary>
    public class ProductLoggerConstants
    {
        /// <summary>
        /// N кол-во продуктов.
        /// </summary>
        public const string GET_PRODUCTS_ALL = "products received.";

        /// <summary>
        /// N кол-во продуктов пользователей.
        /// </summary>
        public const string GET_PRODUCTS_USERS = "users products received.";

        /// <summary>
        /// N кол-во продуктов конкретного пользователя.
        /// </summary>
        public const string GET_PRODUCTS_BY_USER_ID = "products by current user received.";

        /// <summary>
        /// Продукт найден.
        /// </summary>
        public const string GET_PRODUCT_FOUND = "product found.";

        /// <summary>
        /// Продукт не найден.
        /// </summary>
        public const string GET_PRODUCT_NOT_FOUND = "product not found.";

        /// <summary>
        /// Продукт обновлен.
        /// </summary>
        public const string UPDATE_PRODUCT_OK = "product successfully updated.";

        /// <summary>
        /// Продукт не обновлен.
        /// </summary>
        public const string UPDATE_PRODUCT_CONFLICT = "product not updated.";

        /// <summary>
        /// Продукт добавлен.
        /// </summary>
        public const string ADD_PRODUCT_OK = "product successfully added.";

        /// <summary>
        /// Продукт не добавлен.
        /// </summary>
        public const string ADD_PRODUCT_CONFLICT = "product not added.";

        /// <summary>
        /// Продукт удален.
        /// </summary>
        public const string DELETE_PRODUCT_OK = "product successfully deleted.";

        /// <summary>
        /// Продукт не удален.
        /// </summary>
        public const string DELETE_PRODUCT_CONFLICT = "product not deleted.";
    }
} 
