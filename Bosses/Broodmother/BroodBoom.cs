using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Broodmother
{
    public class BroodBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Explosion");
			Main.projFrames[projectile.type] = 4;
        }
		
        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 100;
        }

		bool playedSound = false;
        public override void AI()
        {
			if(!playedSound)
			{
				playedSound = true;
				Main.PlaySound(SoundID.Item88, (int)projectile.Center.X, (int)projectile.Center.Y);				
			}
			projectile.velocity = Vector2.Zero;
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame > 3)
                {
					projectile.frame = 3;
                    if(Main.netMode != NetmodeID.MultiplayerClient) 
						projectile.Kill();
                }
            }			
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}		
    }
}