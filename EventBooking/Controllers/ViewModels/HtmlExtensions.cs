﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EventBooking.Data;
using EventBooking.Data.Entities;

namespace EventBooking.Controllers.ViewModels
{
	public static class HtmlExtensions
	{
        public static readonly Dictionary<ActivityType, string> ActivityTypeColors = new Dictionary<ActivityType, string>
            {
                {ActivityType.Preliminärt, ""},
                {ActivityType.Publikt, "badge-warning"},
                {ActivityType.Sponsor, "badge-important"},
                {ActivityType.Träning, "badge-info"},
                {ActivityType.Teammöte, "badge-info"},
            };

        public static string BadgeColorPicker<TModel>(this HtmlHelper<TModel> htmlHelper, ActivityType type)
        {
            return ActivityTypeColors[type];
        }
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