using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme
{
    public class CarrowR : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carrow");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
            projectile.ranged = true;
			projectile.width = 16; 
			projectile.height = 16; 
			projectile.aiStyle = 1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 1;  
			projectile.timeLeft = 600;  
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			aiType = ProjectileID.WoodenArrowFriendly;
            projectile.noDropItem = true;
		}

        public override void Kill(int timeleft)
        {
            int Split = Main.rand.Next(2, 4);
            Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
            float spread = 12f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / Split;
            for (int i = 0; i < Split; i++)
            {
                double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 3f) * 5, (float)(Math.Cos(offsetAngle) * 3f) * 5, mod.ProjectileType("CarrowSplitR"), projectile.damage / 6, projectile.knockBack, projectile.owner, 0f, 0f);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f) * 5, (float)(-Math.Cos(offsetAngle) * 3f) * 5, mod.ProjectileType("CarrowSplitR"), projectile.damage / 6, projectile.knockBack, projectile.owner, 0f, 0f);
            }
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<CarrotDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100);
                Main.dust[num469].velocity *= 2f;
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
