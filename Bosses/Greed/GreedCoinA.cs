using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Greed
{
    public class GreedCoinA : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Platinum Coin");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GoldCoin);
            projectile.friendly = false;
            projectile.hostile = true;
        }
    }
}