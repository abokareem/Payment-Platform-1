﻿using PaymentPlatform.Framework.Interfaces;
using System;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
    /// ViewModel продукта.
    /// </summary>
    public class ProductViewModel : IHasGuidIdentity
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор профиля.
        /// </summary>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание товара.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Категория.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Активность.
        /// </summary>
        public bool IsActive { get; set; }
    }
}