using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	/// <summary>
	/// Модель товара
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Идентификатор продавца
		/// </summary>
		public int SellerId { get; set; }
		/// <summary>
		/// Название товара
		/// </summary>
		public string ProductName { get; set; }
		/// <summary>
		/// Описание товара
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Единица измерения
		/// </summary>
		public string MeasureUnit { get; set; }
		/// <summary>
		/// Категория
		/// </summary>
		public string Category { get; set; }
		/// <summary>
		/// Количество
		/// </summary>
		public int Amount { get; set; }
		/// <summary>
		/// Цена
		/// </summary>
		public decimal Price { get; set; }
		/// <summary>
		/// QR-код
		/// </summary>
		public string QrCode { get; set; }

		public Seller Seller { get; set; }
		public List<Transaction> Transactions { get; set; }
	}
}
