using System;
using System.ComponentModel.DataAnnotations;

namespace EventBooking.Controllers.ViewModels
{
	public class FutureDate : ValidationAttribute
	{
		public override bool IsValid( object value )
		{
			if ( !( value is DateTime) )
				return false;

			var dt = (DateTime)value;

			return DateTime.Now < dt;
		}
	}
}