using Terraria.ModLoader;using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Hydra
{
    [AutoloadBossHead]
    public class HydraHead6 : HydraHead1
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Head = 5;
        }
    }
}