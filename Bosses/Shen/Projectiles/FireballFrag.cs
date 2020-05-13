using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;using AAMod;

namespace AAModEXAI.Bosses.Shen.Projectiles
{
    public class FireballFragR : ModProjectile
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/Projectiles/FireballFragR";
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
            projectile.timeLeft = 30;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }

            if (Main.netMode != 1)
            {
                Vector2 vel = Vector2.Normalize(projectile.velocity);
                const float ai = 0.01f;
                for (int i = 0; i < 8; ++i)
                {
                    vel = vel.RotatedBy(Math.PI / 4);
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("FireballAccelR"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), ai);
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("FireballAccelR"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), -ai);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 180);
        }
    }

    public class FireballFragB : ModProjectile
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/Projectiles/FireballFragB";
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
            projectile.timeLeft = 30;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataDust"), 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }

            if (Main.netMode != 1)
            {
                Vector2 vel = Vector2.Normalize(projectile.velocity);
                const float ai = 0.01f;
                for (int i = 0; i < 8; ++i)
                {
                    vel = vel.RotatedBy(Math.PI / 4);
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("FireballAccelB"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), ai);
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("FireballAccelB"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), -ai);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 180);
        }
    }
}