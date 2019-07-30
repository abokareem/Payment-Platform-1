using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	public class Product
	{
		public int Id { get; set; }
		public int SellerId { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public string MeasureUnit { get; set; }
		public string Category { get; set; }
		public int Amount { get; set; }
		public decimal Price { get; set; }
		public string QrCode { get; set; }
	}
}
