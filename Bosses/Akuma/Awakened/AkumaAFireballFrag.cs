using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    public class AkumaAFireballFrag : ModProjectile
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
            projectile.timeLeft = 60;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
            projectile.extraUpdates = 1;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vel = Vector2.Normalize(projectile.velocity) * 5;
                for (int i = 0; i < 6; ++i)
                {
                    vel = vel.RotatedBy(Math.PI / 3);
                    Projectile.NewProjectile(projectile.Center, vel, ModContent.ProjectileType<AkumaABomb>(), projectile.damage, 0f, Main.myPlayer);
                }
            }
        }
    }
}