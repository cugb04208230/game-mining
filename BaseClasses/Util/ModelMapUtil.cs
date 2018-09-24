using System.Linq;

namespace Common.Util
{
	/// <summary>
	/// 模型转换
	/// </summary>
    public static class ModelMapUtil
    {
		/// <summary>
		/// 模型转换
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="orignalModel"></param>
		/// <param name="targetModel"></param>
		/// <returns></returns>
		public static T2 AutoMap<T1, T2>(T1 orignalModel, T2 targetModel) where T1:class  where T2:class 
	    {
		    if (orignalModel==null)
		    {
			    return null;
		    }
		    var orignalTypeProperties = orignalModel.GetType().GetProperties();
		    var targetTypeProperties = targetModel.GetType().GetProperties();
		    foreach (var propertyInfo in orignalTypeProperties.Where(e => targetTypeProperties.Select(t => t.Name).Contains(e.Name)))
		    {
			    var value = propertyInfo.GetValue(orignalModel);
			    var targetPropertyInfo = targetTypeProperties.First(t => t.Name == propertyInfo.Name);
			    if (targetPropertyInfo.CanWrite)
				    targetPropertyInfo.SetValue(targetModel, value);
		    }
		    return targetModel;
	    }
		/// <summary>
		/// 模型转换
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="orignalModel"></param>
		/// <param name="targetModel"></param>
		/// <param name="ignoreItems"></param>
		/// <returns></returns>
		public static T2 AutoMap<T1, T2>(T1 orignalModel, T2 targetModel, string[] ignoreItems)
	    {
		    var orignalTypeProperties = orignalModel.GetType().GetProperties();
		    var targetTypeProperties = targetModel.GetType().GetProperties();
		    foreach (var propertyInfo in orignalTypeProperties.Where(e => targetTypeProperties.Select(t => t.Name).Contains(e.Name)))
		    {
			    if (ignoreItems != null && ignoreItems.Contains(propertyInfo.Name))
			    {
				    continue;
			    }
			    var value = propertyInfo.GetValue(orignalModel);
			    var targetPropertyInfo = targetTypeProperties.First(t => t.Name == propertyInfo.Name);
			    if (targetPropertyInfo.CanWrite)
				    targetPropertyInfo.SetValue(targetModel, value);
		    }
		    return targetModel;
	    }
	}
}
