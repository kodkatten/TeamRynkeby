using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EventBooking.Data;

namespace EventBooking.Controllers.ViewModels
{
	public class CreateActivityModel
	{
		[Display( Name = "Beskrivning" )]
		[Required( ErrorMessage = "*" )]
		public string Description { get; set; }

		[Display( Name = "Datum" )]
		[Required( ErrorMessage = "*" )]
		[FutureDate( ErrorMessage = "Du måste ange ett datum i framtiden" )]
		public DateTime Date { get; set; }

		[Display( Name = "Rubrik" )]
		[Required( ErrorMessage = "*" )]
		public string Name { get; set; }

		[Display( Name = "Typ" )]
		[Required( ErrorMessage = "*" )]
		public ActivityType Type { get; set; }

		[Display( Name = "Mer information" )]
		public string Summary { get; set; }
	}

	public static class HtmlExtensions
	{
		public static MvcHtmlString RadioButtonForEnum<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression
		)
		{
			var metaData = ModelMetadata.FromLambdaExpression( expression, htmlHelper.ViewData );
			var names = Enum.GetNames( metaData.ModelType );
			var sb = new StringBuilder();
			foreach ( var name in names )
			{
				var id = string.Format(
					"{0}_{1}_{2}",
					htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix,
					metaData.PropertyName,
					name
				);

				var radio = htmlHelper.RadioButtonFor( expression, name, new { id = id } ).ToHtmlString();
				sb.AppendFormat(
					"{0} <label for=\"{1}\">{2}</label>",
					radio,
					id,
					HttpUtility.HtmlEncode( name )
				);
			}
			return MvcHtmlString.Create( sb.ToString() );
		}
	}
}