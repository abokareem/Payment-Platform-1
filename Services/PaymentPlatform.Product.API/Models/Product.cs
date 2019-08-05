using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Product.API.Models
{
	/// <summary>
	/// Модель товара.
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
