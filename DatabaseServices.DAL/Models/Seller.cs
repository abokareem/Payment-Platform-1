﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	public class Seller
	{
		public int Id { get; set; }
		public string OrganisationName { get; set; }
		public string OrganisationNumber { get; set; }
		public string ResponsiblePerson { get; set; }
		public string Billing { get; set; }
		public decimal Balance { get; set; }
	}
}
