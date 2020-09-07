
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
    public class AthenaMagic : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Varian Burst");
            Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 36;
			projectile.friendly = false; 
			projectile.hostile = true;
			projectile.melee = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.alpha = 20;
            projectile.tileCollide = false;
			projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = 1;
            }
            else
            {
                projectile.spriteDirection = -1;
            }
            projectile.rotation = (float)Math.Atan2(-projectile.velocity.Y, -projectile.velocity.X);

            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 2)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3, 0, 0);
            BaseDrawing.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, .5f, 1f, 10, false, 0f, 0f, Color.White, frame, 3);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 3, frame, Color.White, false);
            return false;
        }

        public override void Kill(int timeleft)
        {
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
            int p = Projectile.NewProjectile((int)projectile.Center.X, (int)projectile.Center.Y, 0, 0, ProjectileID.Electrosphere, 20, projectile.knockBack, Main.myPlayer);
            Main.projectile[p].Center = projectile.Center;
            Main.projectile[p].friendly = false;
            Main.projectile[p].hostile = true;
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Electric, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
            }
            
        }
    }
}
