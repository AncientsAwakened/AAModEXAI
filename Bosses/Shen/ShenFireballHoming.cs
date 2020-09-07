using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Shen
{
    public class ShenFireballHoming : ModProjectile
    {
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
            projectile.scale = 4f;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            projectile.velocity = projectile.DirectionTo(Main.player[(int)projectile.ai[0]].Center) * projectile.ai[1];
            if (++projectile.localAI[0] > 60)
            {
                projectile.localAI[0] = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 vel = Vector2.Normalize(projectile.velocity);
                    const float ai = 0.015f;
                    for (int i = 0; i < 16; ++i)
                    {
                        vel = vel.RotatedBy(Math.PI / 8);
                        Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("ShenFireballAccel"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), 0f);
                        Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("ShenFireballAccel"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), 0f);
                    }
                }
            }
            projectile.scale -= 3f / 300f;
            if (projectile.scale <= 1)
                projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<DiscordDust>(), 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vel = Vector2.Normalize(projectile.velocity);
                const float ai = 0.015f;
                for (int i = 0; i < 16; ++i)
                {
                    vel = vel.RotatedBy(Math.PI / 8);
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("ShenFireballAccel"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), 0f);
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("ShenFireballAccel"), projectile.damage, 0f, Main.myPlayer, Math.Abs(ai), 0f);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DiscordInferno"), 180);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, Color.White, true);
            return false;
        }
    }
}