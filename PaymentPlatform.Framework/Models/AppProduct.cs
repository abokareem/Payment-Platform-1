﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
	/// Модель товара.
	/// </summary>
    public class AppProduct
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
        /// QR-код.
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
		/// Активность.
		/// </summary>
		public bool IsActive { get; set; }
    }
}
