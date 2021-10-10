using Terraria;
using System;
using System.Collections.Generic;
using System.Reflection;
using static AAModEXAI.AAModEXAI;
using static Terraria.ModLoader.ModContent;

namespace AAModEXAI.Loaders
{
    public class Loader
    {
        public static void Load()
        {
            instance = GetInstance<AAModEXAI>();
            allTypesInAssembly = instance.Code.GetTypes();

            foreach (Type type in allTypesInAssembly)
            {
                IEnumerable<MethodInfo> allMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in allMethods)
                {
                    LoadThis loadAttribute = method.GetCustomAttribute<LoadThis>();
                    if (loadAttribute != null && !loadAttribute.clientOnly)
                    {
                        method?.Invoke(null, null);
                    }
                }
            }

            if (!Main.dedServ)
            {
                LoadClient();
            }
        }

        private static void LoadClient()
        {
            foreach (Type type in allTypesInAssembly)
            {
                IEnumerable<MethodInfo> allMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in allMethods)
                {
                    LoadThis loadAttribute = method.GetCustomAttribute<LoadThis>();
                    if (loadAttribute != null && loadAttribute.clientOnly)
                    {
                        method?.Invoke(null, null);
                    }
                }
            }
        }
    }
}
