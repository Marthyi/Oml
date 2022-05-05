using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Oml.Memory
{
    public static class MemoryExtensions
    {
        public static void OptimizeStringMemory<T>(this IEnumerable<T> items, HashSet<string> hash)
        {
            foreach (T item in items)
            {
                item.InternalizeStringProperties(hash);
            }
        }

        public static void OptimizeStringMemory<T>(this IEnumerable<T> items, HashSet<string> hash, params Expression<Func<T, string>>[] properties)
        {
            foreach (T item in items)
            {
                foreach (Expression<Func<T, string>> property in properties)
                {
                    item.Internalize(property, hash);
                }
            }
        }

        public static void InternalizeStringProperties<T>(this T o, HashSet<string> hash)
        {
            if (o == null || hash == null)
            {
                return;
            }

            foreach (PropertyInfo property in
            o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                .Where(p => p.PropertyType == typeof(string)))
            {
                Internalize(property.GetValue(o) as string, val => property.SetValue(o, val), hash);
            }

            foreach (FieldInfo property in
            o.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.FieldType == typeof(string)))
            {
                Internalize(property.GetValue(o) as string, val => property.SetValue(o, val), hash);
            }
        }

        public static void Internalize<T>(this T o, Expression<Func<T, string>> memberAccessor, HashSet<string> hash)
        {
            if (o == null || hash == null || memberAccessor == null)
            {
                return;
            }

            var member = (memberAccessor.Body as MemberExpression)?.Member;

            string value = member switch
            {
                PropertyInfo p => (string)p.GetValue(o),
                FieldInfo f => (string)f.GetValue(o),
                _ => throw new NotImplementedException()
            };

            Action<string> setter = member switch
            {
                PropertyInfo p => val => p.SetValue(o, val),
                FieldInfo f => val => f.SetValue(o, val),
                _ => throw new NotImplementedException()
            };

            Internalize(value, setter, hash);
        }

        public static void Internalize(string? value, Action<string> setter, HashSet<string> hash)
        {
            if (hash.TryGetValue(value!, out string internValue))
            {
                setter(internValue);
            }
            else
            {
                hash.Add(value!);
            }
        }
    }
}