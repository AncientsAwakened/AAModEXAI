using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AAMod;

namespace AAModEXAI.Bosses.Greed
{
    public class WarningPro : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warning");
            Main.projFrames[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 42;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 0;
            projectile.timeLeft = 120;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (++projectile.frameCounter >= 2)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 60f)
            {
                projectile.alpha = 255;
                if (projectile.ai[0] == 0)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        int A = Main.rand.Next(-50, 50);
                        int B = Main.rand.Next(-200, 200) - 1000;

                        int p = Projectile.NewProjectile(projectile.Center.X + A, projectile.Center.Y + B, 0f, 12f, mod.ProjectileType("CovStalactitePro"), 43, 1);
                        Main.projectile[p].netUpdate = true;
                    }
                }
                else
                {
                    if (Main.rand.Next(10) == 0)
                    {
                        int A = Main.rand.Next(-80, 80);
                        int B = Main.rand.Next(-200, 200) - 1000;

                        int p = Projectile.NewProjectile(projectile.Center.X + A, projectile.Center.Y + B, 0f, 10f, mod.ProjectileType("DesireBeam"), 43, 1);
                        Main.projectile[p].netUpdate = true;
                    }
                }
            }
        }
    }
}