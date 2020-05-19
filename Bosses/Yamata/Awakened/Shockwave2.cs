using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class Shockwave2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shockwave");     
            Main.projFrames[projectile.type] = 6;     
        }

        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 202;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool CanDamage()
        {
            return projectile.frame == 1 || projectile.frame == 2;
        }

        public override void AI()
        {
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 6)
                {
                    projectile.Kill();

                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y *= 0.00f;
            if (++projectile.localAI[0] == 6)
                if (Main.netMode != NetmodeID.MultiplayerClient && projectile.ai[0] != 0)
                {
                    projectile.ai[0] -= projectile.ai[0] > 0 ? 1 : -1; //approach 0
                    Projectile.NewProjectile(projectile.Center + Vector2.UnitX * Math.Sign(projectile.ai[0]) * projectile.width, Vector2.Zero, mod.ProjectileType("Shockwave2"), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0]);
                }
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

    }
}
