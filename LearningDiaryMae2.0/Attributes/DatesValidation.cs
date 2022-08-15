using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using ControllerContext = System.Web.Mvc.ControllerContext;
using ModelMetadata = System.Web.Mvc.ModelMetadata;

namespace LearningDiaryMae2.Attributes
{
    public class DateCompareValidationAttribute : ValidationAttribute, IClientValidatable
    {

        public enum CompareType
        {
            GreaterThan,
            GreaterThanOrEqualTo,
            EqualTo,
            LessThanOrEqualTo,
            LessThan
        }

        private readonly CompareType _compareType;
        private readonly string _propertyNameToCompare;

        public DateCompareValidationAttribute(CompareType compareType, string message, string compareWith)
        {
            _compareType = compareType;
            _propertyNameToCompare = compareWith;
            ErrorMessage = message;
        }


        #region IClientValidatable Members
        // Generates client validation rules
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ValidateAndGetCompareToProperty(metadata.ContainerType);
            var rule = new ModelClientValidationRule();

            rule.ErrorMessage = ErrorMessage;
            rule.ValidationParameters.Add("comparetodate", _propertyNameToCompare);
            rule.ValidationParameters.Add("comparetype", _compareType);
            rule.ValidationType = "compare";

            yield return rule;
        }

        #endregion

        //Overrides IsValid-function, compares two given attributes with compare type and returns success or error message
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var basePropertyInfo = validationContext.ObjectType.GetProperty(_propertyNameToCompare);

            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valThis = (IComparable)value;

            if (valThis == null)
            {
                return ValidationResult.Success;
            }

            if ((_compareType == CompareType.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
                (_compareType == CompareType.GreaterThanOrEqualTo && valThis.CompareTo(valOther) < 0) ||
                (_compareType == CompareType.LessThan && valThis.CompareTo(valOther) >= 0) ||
                (_compareType == CompareType.LessThanOrEqualTo && valThis.CompareTo(valOther) > 0) ||
                (_compareType == CompareType.EqualTo && valThis.CompareTo(valOther) != 0))
            {
                return new ValidationResult(base.ErrorMessage);
            }
            return ValidationResult.Success;

        }

        // verifies that the compare-to property exists and of the right types and returns this property
        private PropertyInfo ValidateAndGetCompareToProperty(Type containerType)
        {
            var compareToProperty = containerType.GetProperty(_propertyNameToCompare);
            if (compareToProperty == null)
            {
                string msg = string.Format($"Invalid design time usage of {this.GetType().FullName}. Property {_propertyNameToCompare} is not found in the {containerType.FullName}");
                throw new ArgumentException(msg);
            }
            if (compareToProperty.PropertyType != typeof(DateTime) && compareToProperty.PropertyType != typeof(DateTime?))
            {
                string msg = string.Format($"Invalid design time usage of {this.GetType().FullName}. The type of property {_propertyNameToCompare} of the {containerType.FullName} is not DateType");
                throw new ArgumentException(msg);
            }

            return compareToProperty;
        }
    }
}
