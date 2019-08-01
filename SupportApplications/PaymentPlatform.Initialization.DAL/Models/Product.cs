namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель товара.
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Идентификатор продавца.
		/// </summary>
		public string SellerId { get; set; }

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

		// TODO: Добавить связи через ICollection
	}
}
