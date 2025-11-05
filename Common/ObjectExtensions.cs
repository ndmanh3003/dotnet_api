using System.Collections;
using System.Reflection;

namespace dotnet.Common
{
    public static class ObjectExtensions
    {
        public static TDestination? To<TDestination>(this object? source)
            where TDestination : class
        {
            if (source == null) return null;
            return Map(source, typeof(TDestination)) as TDestination;
        }

        private static object? Map(object? src, Type destType)
        {
            if (src == null) return null;

            // base type / enum / string
            if (IsSimpleType(destType))
                return Convert.ChangeType(src, Nullable.GetUnderlyingType(destType) ?? destType);

            // Collection (List<T>, IEnumerable<T>)
            if (typeof(IEnumerable).IsAssignableFrom(destType) && destType.IsGenericType)
            {
                var itemType = destType.GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
                foreach (var item in (IEnumerable)src)
                    list.Add(Map(item, itemType)!);
                return list;
            }

            // Object
            var dest = Activator.CreateInstance(destType)!;
            var srcType = src.GetType();
            var srcProps = srcType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => p.CanRead)
                                   .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (var destProp in destType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!destProp.CanWrite || !srcProps.TryGetValue(destProp.Name, out var srcProp))
                    continue;

                var value = srcProp.GetValue(src);
                if (value == null) continue;

                var destPropType = Nullable.GetUnderlyingType(destProp.PropertyType) ?? destProp.PropertyType;
                var srcPropType = Nullable.GetUnderlyingType(srcProp.PropertyType) ?? srcProp.PropertyType;

                if (destPropType.IsAssignableFrom(srcPropType))
                {
                    destProp.SetValue(dest, value);
                }
                else if (!IsSimpleType(destPropType))
                {
                    destProp.SetValue(dest, Map(value, destPropType));
                }
                else
                {
                    try { destProp.SetValue(dest, Convert.ChangeType(value, destPropType)); }
                    catch { }
                }
            }

            return dest;
        }

        private static bool IsSimpleType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type) ?? type;
            return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(Guid);
        }
    }
}
