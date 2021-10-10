using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using static AAModEXAI.AAModEXAI;

namespace AAModEXAI.Loaders
{
    public class Unloader
    {
        public static void Unload()
        {
            IEnumerable<Type> allTypes = instance.Code.GetTypes();
            foreach (Type type in allTypes)
            {
                IEnumerable<MethodInfo> allMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in allMethods)
                {
                    UnloadThis loadAttribute = method.GetCustomAttribute<UnloadThis>();
                    if (loadAttribute != null && !loadAttribute.clientOnly)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            if (!Main.dedServ)
            {
                UnloadClient();
            }
        }

        private static void UnloadClient()
        {
            IEnumerable<Type> allTypes = instance.Code.GetTypes();
            foreach (Type type in allTypes)
            {
                IEnumerable<MethodInfo> allMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in allMethods)
                {
                    UnloadThis loadAttribute = method.GetCustomAttribute<UnloadThis>();
                    if (loadAttribute != null && loadAttribute.clientOnly)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            instance = null; // Unloading process has finished.
        }
    }
}
