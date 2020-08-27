using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Rajah.Supreme
{
    public class RajahRocketEXR : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Rocket");
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.scale = 0.9f;
            projectile.penetrate = 1;
            projectile.timeLeft = 120;
            projectile.extraUpdates = 1;
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
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
            int p = Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), mod.ProjectileType("RabbitBoomEXR"), projectile.damage, projectile.knockBack, projectile.owner);
            Main.projectile[p].Center = projectile.Center;
            float spread = 12f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 3;
            for (int i = 0; i < 3; i++)
            {
                double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 3f) * 5, (float)(Math.Cos(offsetAngle) * 3f) * 5, mod.ProjectileType("CarrotEXR"), projectile.damage / 6, projectile.knockBack, projectile.owner, 0f, 0f);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f) * 5, (float)(-Math.Cos(offsetAngle) * 3f) * 5, mod.ProjectileType("CarrotEXR"), projectile.damage / 6, projectile.knockBack, projectile.owner, 0f, 0f);
            }
        }
    }
}
