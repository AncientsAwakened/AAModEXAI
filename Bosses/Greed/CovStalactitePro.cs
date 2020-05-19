using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Greed
{
    public class CovStalactitePro : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Covitite Stalactites");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 34;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.alpha = 0;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                int changeChoice = Main.rand.Next(3);
                if (changeChoice == 0)
                {
                    projectile.frame = 0;
                }
                if (changeChoice == 1)
                {
                    projectile.frame = 1;
                }
                if (changeChoice == 2)
                {
                    projectile.frame = 2;
                }
                projectile.ai[0] = 1;
            }
        }
    }
}