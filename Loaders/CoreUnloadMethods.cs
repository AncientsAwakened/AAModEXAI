using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using static AAModEXAI.AAModEXAI;

namespace AAModEXAI.Loaders
{
    public class CoreUnloadMethods
    {
        [UnloadThis]
        public static void UnloadAllStatics()
        {
			// Thanks to BaseLibrary for the static field unload code.
			foreach (Type type in allTypesInAssembly)
			{
				foreach (FieldInfo item in from field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
										   where !field.FieldType.IsValueType && !field.IsLiteral && field.GetCustomAttribute<BypassAutoUnload>() == null
										   select field)
				{
					if (item.FieldType.IsGenericType)
					{
						if (item.FieldType.GetGenericTypeDefinition() == typeof(List<>))
						{
							((IList)item.GetValue(null))?.Clear();
						}
						else if (item.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
						{
							((IDictionary)item.GetValue(null))?.Clear();
						}
					}
					else if (!item.FieldType.ContainsGenericParameters)
					{
						item.SetValue(null, null);
					}
				}
			}
		}
    }
}
