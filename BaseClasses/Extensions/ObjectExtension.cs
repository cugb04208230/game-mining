using System;
using System.Collections.Generic;
using System.Linq;
using BaseClasses.Filters;
using Newtonsoft.Json;

namespace BaseClasses.Extensions
{
	public static class ObjectExtension
	{

		public static Dictionary<string, object> ToDictionary(this object obj)
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			var type = obj.GetType();
			foreach (var fieldInfo in type.GetProperties())
			{
				var value = fieldInfo.GetValue(obj);
				result.Add(fieldInfo.Name, value);
			}
			return result;
		}

		public static string SerializeObject(this object obj)
		{
			var result = string.Empty;
			if (obj != null)
			{
				result = JsonConvert.SerializeObject(obj);
			}
			return result;
		}

		public static void SetPropertyValue(this object obj, string propertyName, object value)
		{
			var targetTypeProperties = obj.GetType().GetProperties();
			var targetPropertyInfo = targetTypeProperties.FirstOrDefault(t => t.Name == propertyName);
			if (targetPropertyInfo != null&& targetPropertyInfo.CanWrite)
			{
				try
				{
					value = Convert.ChangeType(value, targetPropertyInfo.PropertyType);
				}
				catch 
				{
					throw new PlatformException(ErrorCode.ErrorValueType);
				}
				if (targetPropertyInfo.CanWrite)
					targetPropertyInfo.SetValue(obj, value);
			}
		}

		public static object GetPropertyValue(this object obj, string propertyName)
		{
			var targetTypeProperties = obj.GetType().GetProperties();
			var targetPropertyInfo = targetTypeProperties.FirstOrDefault(t => t.Name == propertyName);
			if (targetPropertyInfo == null||!targetPropertyInfo.CanRead)
			{
				throw new PlatformException(ErrorCode.PropertyNameInvalid);
			}
			return targetPropertyInfo.GetValue(obj);
		}
	}
}
