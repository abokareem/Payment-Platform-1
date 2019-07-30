using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	public class Customer
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? BuyerId { get; set; }
		public int? SellerId { get; set; }
		public int Role { get; set; }
		public bool Activity { get; set; }
	}
}
