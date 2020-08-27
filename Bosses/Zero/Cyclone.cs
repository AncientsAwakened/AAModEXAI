
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Zero
{
    class Cyclone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Cyclone");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.timeLeft = 180;
            projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.scale = .05f;
        }

        public override void AI()
        {
            projectile.rotation += 0.03f;
            projectile.velocity *= 0;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                projectile.ai[1]++;
            }
            if (projectile.ai[0] == 0)
            {
                if (projectile.scale < 1)
                {
                    projectile.scale += .05f;
                }
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 5;
                }
                if (projectile.ai[1] > 240)
                {
                    projectile.ai[0] = 1;
                    projectile.ai[1] = 0;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                if (projectile.scale > 0)
                {
                    projectile.scale -= .05f;
                }
                if (projectile.alpha < 255)
                {
                    projectile.alpha += 5;
                }

                if (projectile.ai[1] > 30f)
                {
                    projectile.active = false;
                    projectile.netUpdate = true;
                }
            }
            for (int u = 0; u < Main.maxPlayers; u++)
            {
                Player target = Main.player[u];

                if (target.active && Vector2.Distance(projectile.Center, target.Center) < 600)
                {
                    float num3 = 10f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = projectile.Center.X - vector.X;
                    float num5 = projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 12;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Tex.Width, Tex.Height, 0, 0);
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}
