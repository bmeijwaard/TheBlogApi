namespace TheBlogApi.Helpers.Extensions
{
    public static class PropertyExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public static void SetPropertyValue<T>(this object obj, string propertyName, object value)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            prop.SetValue(obj, (T)value, null);
        }
    }
}
