using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Greed
{
    public class GreedLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desire Blast");
        }
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 200;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            int DustID2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 57, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default, 1f);
            Main.dust[DustID2].noGravity = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 57, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
        }
    }
}