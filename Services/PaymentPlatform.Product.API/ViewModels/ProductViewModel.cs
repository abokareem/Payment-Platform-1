using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.ViewModels
{
	public class ProductViewModel
	{
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		public Guid Id { get; set; }
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
	}
}
