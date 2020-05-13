using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;using AAMod;

namespace AAModEXAI.Bosses.Shen.Projectiles
{
    public class FireballAccelR : ModProjectile
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/Projectiles/FireballAccelR";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
            Main.projFrames[projectile.type] = 4;
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.timeLeft = 720;
            projectile.aiStyle = -1;
            projectile.extraUpdates = 1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            projectile.velocity *= 1f + Math.Abs(projectile.ai[0]);

            Vector2 acceleration = projectile.velocity.RotatedBy(Math.PI / 2);
            acceleration *= projectile.ai[1];
            projectile.velocity += acceleration;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 180);
        }
    }

    public class FireballAccelB : ModProjectile
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/Projectiles/FireballAccelB";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
            Main.projFrames[projectile.type] = 4;
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.timeLeft = 360;
            projectile.aiStyle = -1;
            projectile.extraUpdates = 1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            projectile.velocity *= 1f + Math.Abs(projectile.ai[0]);

            Vector2 acceleration = projectile.velocity.RotatedBy(Math.PI / 2);
            acceleration *= projectile.ai[1];
            projectile.velocity += acceleration;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataDust"), 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 180);
        }
    }
}