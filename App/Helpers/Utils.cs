using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventApp.App.Helpers
{
	public class Utils
	{
		// Copied from https://stackoverflow.com/a/46080218
		public static List<string> GetChangedProperties<T>(object A, object B)
		{
			if (A != null && B != null)
			{
				var type = typeof(T);
				var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
				var allSimpleProperties = allProperties.Where(pi => IsSimpleType(pi.PropertyType));
				var unequalProperties =
					   from pi in allSimpleProperties
					   let AValue = type.GetProperty(pi.Name).GetValue(A, null)
					   let BValue = type.GetProperty(pi.Name).GetValue(B, null)
					   where AValue != BValue && (AValue == null || !AValue.Equals(BValue))
					   select pi.Name;
				return unequalProperties.ToList();
			}
			else
			{
				throw new ArgumentNullException("You need to provide 2 non-null objects");
			}
		}

		public static bool IsSimpleType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				// nullable type, check if the nested type is simple.
				return IsSimpleType(type.GetGenericArguments()[0]);
			}
			return type.IsPrimitive
			  || type.IsEnum
			  || type.Equals(typeof(string))
			  || type.Equals(typeof(decimal));
		}
	}
}
