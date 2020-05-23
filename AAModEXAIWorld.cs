using Terraria;
using Terraria.ModLoader;
using HarmonyLib;

namespace AAModEXAI
{
    public class AAModEXAIWorld : ModWorld
    {
        public override void PostUpdate()
        {
            /*
            if(!Harmony.HasAnyPatches("ModifyDD2"))
            {
                AAModEXAI.instance.harmony.Patch(AAModEXAI.instance.DD2Invasion, new HarmonyMethod(AAModEXAI.instance.DD2InvasionPatch));
            }
            */
        }
    }
}