using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Greed
{
    public class GreedCoin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Coin");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GoldCoin);
            projectile.friendly = false;
            projectile.hostile = true;
        }
    }
}