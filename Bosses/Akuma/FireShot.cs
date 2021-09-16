using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma
{
    public class FireShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;      
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.LaserMachinegunLaser);
            projectile.width = 5;
            projectile.height = 5;
            projectile.aiStyle = 1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 50;
            projectile.scale = 1f;
            projectile.timeLeft = 100;
            projectile.magic = true;
            projectile.ignoreWater = true;
            
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 2)
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(-projectile.velocity.Y, -projectile.velocity.X);
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            }

            int dustId = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height + 5, ModContent.DustType<InfinityOverloadB>(), projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height + 5, ModContent.DustType<InfinityOverloadB>(), projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188), 2f);
            Main.dust[dustId3].noGravity = true;
        }

        public override void Kill(int timeleft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<AkumaDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188), 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<AkumaDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188));
                Main.dust[num469].velocity *= 2f;
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<Flare>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
        }

        
    }
}
