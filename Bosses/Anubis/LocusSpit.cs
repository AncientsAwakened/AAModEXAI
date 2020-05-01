using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Anubis
{
    public class LocusSpit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spit");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.alpha = 20;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            int dustType = ModLoader.GetMod("AAMod").DustType("JudgementDust");
            if (Main.rand.Next(3) == 0)
            {
                int dustID2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID2].velocity = -projectile.velocity * 0.5f;
                Main.dust[dustID2].noLight = false;
                Main.dust[dustID2].noGravity = true;
            }

            if (projectile.frameCounter++ > 3)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeleft)
        {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("JudgementDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188), 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("JudgementDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188));
                Main.dust[num469].velocity *= 2f;
            }
        }
    }
}
