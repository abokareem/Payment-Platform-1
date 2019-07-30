using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	public class Transaction
	{
		public int Id { get; set; }
		public string UniqueHashNumber { get; set; }
		public int SellerId { get; set; }
		public int BuyerId { get; set; }
		public int ProductId { get; set; }
		public DateTime TransactionTime { get; set; }
		public int TransactionStatus { get; set; }

		public Seller Seller { get; set; }
	}
}
