using System.Linq;
using System.Web.Mvc;

namespace EventBooking.Extensions
{
	public static class ModelStateExtensions
	{
		public static string Errors(this ModelStateDictionary modelStateDictionary)
		{
			return string.Join("; ", modelStateDictionary.Values
										.SelectMany(x => x.Errors)
										.Select(x => x.ErrorMessage));
		}
	}

}