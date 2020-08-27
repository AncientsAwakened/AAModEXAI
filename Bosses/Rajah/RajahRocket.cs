using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Rajah
{
    public class RajahRocket : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Rocket");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.scale = 0.9f;
            projectile.penetrate = 1;
            projectile.timeLeft = 120;
        }

        public override void AI()
        {
            if (projectile.timeLeft <= 0)
            {
                Kill(projectile.timeLeft);
            }
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(-projectile.velocity.Y, -projectile.velocity.X) - 1.57f;
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            Projectile.NewProjectile(projectile.position, new Vector2(0, 0), mod.ProjectileType("RabbitRocketBoomR"), projectile.damage, projectile.knockBack, projectile.owner);
        }
    }
}
