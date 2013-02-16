﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EventBooking.Controllers.ViewModels
{
	public class FutureDate : ValidationAttribute
	{
		public override bool IsValid( object value )
		{
			if ( !( value is DateTime ) )
				return false;

			var dt = (DateTime)value;

			return DateTime.Now < dt;
		}
	}

	public sealed class IsDateAfter : ValidationAttribute, IClientValidatable
	{
		private readonly string testedPropertyName;
		private readonly bool allowEqualDates;

		public IsDateAfter( string testedPropertyName, bool allowEqualDates = false )
		{
			this.testedPropertyName = testedPropertyName;
			this.allowEqualDates = allowEqualDates;
		}

		protected override ValidationResult IsValid( object value, ValidationContext validationContext )
		{
			var propertyTestedInfo = validationContext.ObjectType.GetProperty( this.testedPropertyName );
			if ( propertyTestedInfo == null )
			{
				return new ValidationResult( string.Format( "unknown property {0}", this.testedPropertyName ) );
			}

			var propertyTestedValue = propertyTestedInfo.GetValue( validationContext.ObjectInstance, null );

			if ( value == null || !( value is DateTime ) )
			{
				return ValidationResult.Success;
			}

			if ( propertyTestedValue == null || !( propertyTestedValue is DateTime ) )
			{
				return ValidationResult.Success;
			}

			// Compare values
			if ( (DateTime)value >= (DateTime)propertyTestedValue )
			{
				if ( this.allowEqualDates )
				{
					return ValidationResult.Success;
				}
				if ( (DateTime)value > (DateTime)propertyTestedValue )
				{
					return ValidationResult.Success;
				}
			}

			return new ValidationResult( FormatErrorMessage( validationContext.DisplayName ) );
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules( ModelMetadata metadata,
																			   ControllerContext context )
		{
			var rule = new ModelClientValidationRule
				{
					ErrorMessage = this.ErrorMessageString,
					ValidationType = "isdateafter"
				};
			rule.ValidationParameters["propertytested"] = this.testedPropertyName;
			rule.ValidationParameters["allowequaldates"] = this.allowEqualDates;
			yield return rule;
		}
	}
}