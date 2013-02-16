using System;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EventBooking.Controllers.ViewModels
{
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