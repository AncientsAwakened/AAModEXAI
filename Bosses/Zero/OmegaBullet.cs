using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero
{
    public class OmegaBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omega Bullet");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Bullet);
            projectile.friendly = false;
            projectile.hostile = true;
            aiType = ProjectileID.Bullet;
            projectile.tileCollide = false;
        }

        int a = 0;

        public override void PostAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) a++;
            if (a == 40)
            {
                projectile.tileCollide = true;
                projectile.netUpdate = true;
            }
            if (a < 40)
            {
                projectile.tileCollide = false;
            }
        }
    }
}
