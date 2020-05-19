using Terraria.ModLoader;using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Hydra
{
    [AutoloadBossHead]
    public class HydraHead8 : HydraHead1
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Head = 7;
        }
    }
}