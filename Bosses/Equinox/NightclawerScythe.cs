using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Equinox
{
    public class NightclawerScythe : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightclawer Scythe");
		}

        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if(projectile.localAI[1] ++ == 5f)
            {
                SpawnDust();
            }
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), .37f, .8f, .89f);

            if(Main.rand.Next(10) == 0)
            {
                int dustId = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), projectile.velocity.X,
                    projectile.velocity.Y, 100, new Color(), 2f);
                Main.dust[dustId].noGravity = true;
            }

            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
                projectile.rotation = projectile.ai[0];
                projectile.spriteDirection = -(int)projectile.ai[1];
            }
            if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 20)
            {
                projectile.velocity *= 1.05f;
            }
            if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 40)
            {
                projectile.velocity *= 1.012f;
            }
            if (projectile.velocity.X < 0f)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
            projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.025f * projectile.direction;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(95, 205, 228, 200);
        }

        public void SpawnDust()
        {
            Vector2 position = projectile.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
        }
    }
}