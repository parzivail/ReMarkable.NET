using System;
using System.Reflection;

namespace ReMarkable.NET.Util
{
    /// <summary>
    ///     Provides extension methods that assist in reflecting assemblies
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Gets the value of a public, static field from a class using reflection
        /// </summary>
        /// <typeparam name="T">The type of the field</typeparam>
        /// <param name="t">The containing class type</param>
        /// <param name="field">The field name</param>
        /// <param name="fieldValue">The found field value</param>
        /// <remarks>
        ///     This could have a return value instead of an out parameter but
        ///     then it would not infer the type of the field based on the value
        ///     target.
        /// </remarks>
        public static void ReadStaticField<T>(this Type t, string field, out T fieldValue)
        {
            var displayDev = t.GetField(field, BindingFlags.Public | BindingFlags.Static);
            if (displayDev == null)
                throw new Exception($"Could not find field {field}");

            var value = displayDev.GetValue(displayDev);

            if (!(value is T obj))
                throw new Exception($"Field {field} is not of type {t}, was {value?.GetType()}");

            fieldValue = obj;
        }
    }
}