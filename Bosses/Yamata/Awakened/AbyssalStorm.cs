using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class AbyssalStorm : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Storm");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 90;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 120;
            cooldownSlot = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 0);
        }

        public override void AI()
        {
        	projectile.alpha -= 1;
        	if (projectile.alpha <= 0)
        	{
        		projectile.Kill();
        	}
        	Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
        	projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        	if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 20);
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
        }
        
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
        	target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 300);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
            float spread = 12f * 0.0174f;
			double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y)- spread/2;
	    	double Angle = spread/4;
	    	double offsetAngle;
	    	int i;
	    	if (projectile.owner == Main.myPlayer)
	    	{
		    	for (i = 0; i < 4; i++ )
		    	{
		   			offsetAngle = startAngle + Angle * ( i + i * i ) / 2f  + 32f * i;
		        	Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)( Math.Sin(offsetAngle) * 6f ), (float)( Math.Cos(offsetAngle) * 6f ), mod.ProjectileType("SoulRain"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
		        	Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)( -Math.Sin(offsetAngle) * 6f ), (float)( -Math.Cos(offsetAngle) * 6f ), mod.ProjectileType("SoulRain"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
		    	}
	    	}
        	for (int dust = 0; dust <= 10; dust++)
        	{
        		Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 235, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
        	}
        }
    }
}