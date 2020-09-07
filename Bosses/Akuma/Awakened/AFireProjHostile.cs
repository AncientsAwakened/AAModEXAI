using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    internal class AFireProjHostile : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            DisplayName.SetDefault("Blazing Fury");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale = 1.1f;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 60;
            projectile.timeLeft = 180;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 0)
            {
                projectile.timeLeft--;
            }
            if (projectile.timeLeft == 0)
            {
                projectile.Kill();
            }

            projectile.frameCounter++;
            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            if (projectile.frameCounter > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            for (int num189 = 0; num189 < 1; num189++)
            {
                int num190 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<AkumaADust>(), 0f, 0f, 0);

                Main.dust[num190].scale *= 1.3f;
                Main.dust[num190].fadeIn = 1f;
                Main.dust[num190].noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 600);
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            int shader = Terraria.Graphics.Shaders.GameShaders.Armor.GetShaderIdFromItemId(Terraria.ID.ItemID.LivingOceanDye);

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 2);

            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], shader, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, Color.White, true);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
            float spread = 12f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 15;
            double offsetAngle;
            int i;
            for (i = 0; i < 15; i++)
            {
                offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 6f), (float)(Math.Cos(offsetAngle) * 6f), mod.ProjectileType("FireshotA"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 6f), (float)(-Math.Cos(offsetAngle) * 6f), mod.ProjectileType("FireshotA"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            }
        }
    }
}