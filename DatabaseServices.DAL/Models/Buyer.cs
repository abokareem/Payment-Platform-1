using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	public class Buyer
	{
		public int Id { get; set; }
		public string Billing { get; set; }
		public decimal Balance { get; set; }
	}
}
