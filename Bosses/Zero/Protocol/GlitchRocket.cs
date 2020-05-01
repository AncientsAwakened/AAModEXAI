using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class GlitchRocket : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R0CKET");
            Main.projFrames[projectile.type] = 3;
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            cooldownSlot = 1;
            projectile.damage = 10;
            projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.Oblivion;
        }

        public override void AI()
        {
        	Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
        	projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        	if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 20);
			}
        	int num103 = Player.FindClosest(projectile.Center, 1, 1);
			projectile.ai[1] += 1f;
			if (projectile.ai[1] < 220f && projectile.ai[1] > 20f)
			{
				float scaleFactor2 = projectile.velocity.Length();
				Vector2 vector11 = Main.player[num103].Center - projectile.Center;
				vector11.Normalize();
				vector11 *= scaleFactor2;
				projectile.velocity = (projectile.velocity * 24f + vector11) / 25f;
				projectile.velocity.Normalize();
				projectile.velocity *= scaleFactor2;
			}
			if (projectile.ai[0] < 0f)
			{
				if (projectile.velocity.Length() < 18f)
				{
					projectile.velocity *= 1.02f;
				}
			}
			
			int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height + 10, ModLoader.GetMod("AAMod").DustType("VoidDust"), projectile.velocity.X * 0.2f,
					projectile.velocity.Y * 0.2f, 100);
				Main.dust[dustId].noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Glitch"), (int)projectile.Center.X, (int)projectile.Center.Y);
            float spread = 12f * 0.0174f;
			double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y)- spread/2;
	    	double Angle = spread/4f;
	    	double offsetAngle;
	    	int i;
	    	if (projectile.owner == Main.myPlayer)
	    	{
		    	for (i = 0; i < 4; i++ )
		    	{
		   			offsetAngle = startAngle + Angle * ( i + i * i ) / 2f  + 32f * i;
		        	Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)( Math.Sin(offsetAngle) * 2f ), (float)( Math.Cos(offsetAngle) * 6f ), mod.ProjectileType("GlitchBlast"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
		        	Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)( -Math.Sin(offsetAngle) * 2f ), (float)( -Math.Cos(offsetAngle) * 6f ), mod.ProjectileType("GlitchBlast"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
		    	}
	    	}
        }
    }
}