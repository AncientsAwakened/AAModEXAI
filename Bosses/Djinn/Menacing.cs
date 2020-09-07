/*
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Djinn
{
    public class Menacing : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Djinnado");
		}

		public override void SetDefaults()
		{
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 127;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 1f)
            {
                projectile.alpha += 10;
                if (projectile.alpha >= 255)
                {
                    projectile.Kill();
                }
            }
            else
            {
                projectile.alpha -= 10;
                if (projectile.alpha <= 0)
                {
                    projectile.localAI[0] = 1f;
                }
            }
        }
    }
}
*/
