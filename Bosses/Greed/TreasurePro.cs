using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Greed
{
    public class TreasurePro : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Memories of Something Grand");
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.alpha = 255;
            projectile.tileCollide = false;
        }

        public bool offsetLeft = false;

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                int changeChoice = Main.rand.Next(8);
                if (changeChoice == 0)
                {
                    projectile.frame = 0;
                }
                if (changeChoice == 1)
                {
                    projectile.frame = 1;
                }
                if (changeChoice == 2)
                {
                    projectile.frame = 2;
                }
                if (changeChoice == 3)
                {
                    projectile.frame = 3;
                }
                if (changeChoice == 4)
                {
                    projectile.frame = 4;
                }
                if (changeChoice == 5)
                {
                    projectile.frame = 5;
                }
                if (changeChoice == 6)
                {
                    projectile.frame = 6;
                }
                if (changeChoice == 7)
                {
                    projectile.frame = 7;
                }
                projectile.ai[0] = 1;
            }
            projectile.alpha -= 4;
            if (projectile.alpha <= 0)
            {
                if (projectile.velocity.Y < 0)
                {
                    projectile.velocity.Y += 0.1f;
                }
            }
            if (projectile.velocity.Y >= 0)
            {
                if (offsetLeft)
                {
                    projectile.rotation -= 0.025f;
                    if (projectile.rotation <= -0.1f)
                    {
                        projectile.rotation = -0.1f;
                        offsetLeft = false;
                    }
                }
                else
                {
                    projectile.rotation += 0.025f;
                    if (projectile.rotation >= 0.1f)
                    {
                        projectile.rotation = 0.1f;
                        offsetLeft = true;
                    }
                }
                if (++projectile.localAI[1] >= 60)
                {
                    Vector2 vel = Vector2.Normalize(projectile.velocity);
                    for (int i = 0; i < 12; ++i)
                    {
                        vel = vel.RotatedBy(Math.PI / 6);
                        Projectile.NewProjectile(projectile.Center, vel * 2, mod.ProjectileType("DesireSparkPro"), projectile.damage / 2, 0f, Main.myPlayer);
                    }
                    projectile.Kill();
                }
            }
        }
    }
}