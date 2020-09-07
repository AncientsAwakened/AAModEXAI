using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena.Olympian
{
    public class SwiftwindStrikeSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Javelin");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = true;
            projectile.aiStyle = -1;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.extraUpdates = 2;
            projectile.timeLeft = 120;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int num579 = 0; num579 < 20; num579++)
            {
                int num580 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 76, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num580].noGravity = true;
                Main.dust[num580].velocity *= 2f;
                num580 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 76, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100);
                Main.dust[num580].velocity *= 2f;
            }
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;

            if (projectile.ai[0]++ > 10)
            {
                projectile.ai[0] = 0;
                Projectile.NewProjectile(projectile.position, Vector2.Zero, mod.ProjectileType("SwiftwindStrikeOrb"), projectile.damage / 2, 5, Main.myPlayer, 0, 0);
            }
        }
    }
}
