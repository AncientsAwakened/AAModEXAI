using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.AH.Haruka
{
    public class HarukaArrow : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 30;
			projectile.friendly = false;
            projectile.hostile = true;
			projectile.timeLeft = 1200;
			projectile.penetrate = 1;
            projectile.extraUpdates = 1;
            projectile.aiStyle = 1;
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = height = 10;
			return true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haruka Arrow");
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 180);
            projectile.netUpdate = true;
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

        public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 5; k++)
			{
			     Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("CthulhuAuraDust"), projectile.oldVelocity.X * 0.1f, projectile.oldVelocity.Y * 0.1f);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 0);
			
		}
	}
}