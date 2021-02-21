using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class AnubisFireball : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anubis Fireball");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 1;
			projectile.alpha = 50;
			projectile.timeLeft = 150;
			cooldownSlot = 1;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > 4)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame > 3)
			{
				projectile.frame = 0;
			}
			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0f) / 255f, ((255 - projectile.alpha) * 0.9f) / 255f, ((255 - projectile.alpha) * 0.2f) / 255f);

            if (projectile.ai[0]++ > 180)
            {
                projectile.Kill();
            }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void Kill(int timeLeft)
		{
			float spread = 45f * 0.0174f;
			double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
			double deltaAngle = spread / 6f;
            for (int i = 0; i < 6; i++)
            {
                double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 7f), (float)(Math.Cos(offsetAngle) * 7f), mod.ProjectileType("CurseFlame"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 1f);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 7f), (float)(-Math.Cos(offsetAngle) * 7f), mod.ProjectileType("CurseFlame"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 1f);
            }
            for (int dust = 0; dust < 5; dust++)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModContent.DustType<ForsakenDust>(), 0f, 0f);
			}
		}
	}
}
