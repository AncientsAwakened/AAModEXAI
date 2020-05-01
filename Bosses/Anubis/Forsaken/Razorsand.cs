using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    class Razorsand : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 54;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (projectile.ai[1] > 0f)
            {
                int num611 = (int)projectile.ai[1] - 1;
                if (num611 < 255)
                {
                    projectile.localAI[0] += 1f;
                    if (projectile.localAI[0] > 10f)
                    {
                        int num612 = 6;
                        for (int num613 = 0; num613 < num612; num613++)
                        {
                            Vector2 vector43 = Vector2.Normalize(projectile.velocity) * new Vector2(projectile.width / 2f, projectile.height) * 0.75f;
                            vector43 = vector43.RotatedBy((num613 - (num612 / 2 - 1)) * 3.1415926535897931 / (float)num612, default) + projectile.Center;
                            Vector2 value15 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - 1.57079637f).ToRotationVector2() * Main.rand.Next(3, 8);
                            int num614 = Dust.NewDust(vector43 + value15, 0, 0, ModLoader.GetMod("AAMod").DustType("SandDust"), value15.X * 2f, value15.Y * 2f, 100, default, 1.4f);
                            Main.dust[num614].noGravity = true;
                            Main.dust[num614].noLight = true;
                            Main.dust[num614].velocity /= 4f;
                            Main.dust[num614].velocity -= projectile.velocity;
                        }
                        if (projectile.alpha <= 0)
                        {
                            projectile.alpha = 0;
                        }
                        else
                        {
                            projectile.alpha -= 5;
                        }
                        projectile.rotation += projectile.velocity.X * 0.1f;
                        projectile.frame = (int)(projectile.localAI[0] / 3f) % 3;
                    }
                    Vector2 value16 = Main.player[num611].Center - projectile.Center;
                    float num615 = 4f;
                    num615 += projectile.localAI[0] / 20f;
                    projectile.velocity = Vector2.Normalize(value16) * num615;
                    if (value16.Length() < 50f)
                    {
                        projectile.Kill();
                    }
                }
            }
            else
            {
                float num616 = 0.209439516f;
                float num617 = 4f;
                float num618 = (float)(Math.Cos(num616 * projectile.ai[0]) - 0.5) * num617;
                projectile.velocity.Y = projectile.velocity.Y - num618;
                projectile.ai[0] += 1f;
                num618 = (float)(Math.Cos(num616 * projectile.ai[0]) - 0.5) * num617;
                projectile.velocity.Y = projectile.velocity.Y + num618;
                projectile.localAI[0] += 1f;
                if (projectile.localAI[0] > 10f)
                {
                    projectile.alpha -= 5;
                    if (projectile.alpha < 100)
                    {
                        projectile.alpha = 100;
                    }
                    projectile.rotation += projectile.velocity.X * 0.1f;
                    projectile.frame = (int)(projectile.localAI[0] / 3f) % 3;
                }
            }
            if (projectile.wet)
            {
                projectile.position.Y = projectile.position.Y - 16f;
                projectile.Kill();
                return;
            }
        }

        public override void Kill(int timeleft)
        {

            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, 658, 40, 0f, Main.myPlayer, 0f, 0f);
        }
    }
}