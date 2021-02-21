using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class ERROR : ModProjectile
    {
    	
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ERR0R");
            Main.projFrames[projectile.type] = 4;
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -11;
            projectile.extraUpdates = 1;
            projectile.penetrate = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.Oblivion;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 0.05f / 255f, (255 - projectile.alpha) * 0.05f / 255f);
            projectile.velocity *= 0.98f;
            projectile.alpha += 2;
            if (projectile.alpha > 255)
            {
                projectile.Kill();
            }

            int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height + 10, ModContent.DustType<VoidDust>(), projectile.velocity.X * 0.2f,
					projectile.velocity.Y * 0.2f, 100);
				Main.dust[dustId].noGravity = true;
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 4) 
                    projectile.frame = 0; 
            }
            return true;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (Main.rand.Next(7) == 0)
			{
				target.AddBuff(mod.BuffType("Unstable"), 180, true);
			}
		}
    }
}