using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004B5 RID: 1205
	public class AthenaMagic : ModProjectile
	{
		// Token: 0x06001CE3 RID: 7395 RVA: 0x00151791 File Offset: 0x0014F991
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Varian Burst");
			Main.projFrames[base.projectile.type] = 3;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x001517B8 File Offset: 0x0014F9B8
		public override void SetDefaults()
		{
			base.projectile.width = 32;
			base.projectile.height = 36;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.melee = true;
			base.projectile.penetrate = 1;
			base.projectile.timeLeft = 600;
			base.projectile.alpha = 20;
			base.projectile.tileCollide = false;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = true;
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x00151850 File Offset: 0x0014FA50
		public override void AI()
		{
			base.projectile.rotation = Utils.ToRotation(base.projectile.velocity) + MathHelper.ToRadians(90f);
			Projectile projectile = base.projectile;
			int frameCounter = projectile.frameCounter;
			projectile.frameCounter = frameCounter + 1;
			if (frameCounter > 5)
			{
				base.projectile.frameCounter = 0;
				base.projectile.frame++;
				if (base.projectile.frame > 2)
				{
					base.projectile.frame = 0;
				}
			}
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x001518D8 File Offset: 0x0014FAD8
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height / 3, 0, 0);
			BaseDrawing.DrawAfterimage(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile, 0.5f, 1f, 10, false, 0f, 0f, new Color?(new Color(35, 23, 87)), new Rectangle?(frame), 3);
			BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, 0, 3, frame, new Color?(Color.White), false, default(Vector2));
			return false;
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x001519DC File Offset: 0x0014FBDC
		public override void Kill(int timeleft)
		{
			Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 14, 1f, 0f);
			int num = Projectile.NewProjectile((float)((int)base.projectile.Center.X), (float)((int)base.projectile.Center.Y), 0f, 0f, 443, base.projectile.damage, base.projectile.knockBack, Main.myPlayer, 0f, 0f);
			Main.projectile[num].Center = base.projectile.Center;
			Main.projectile[num].friendly = false;
			Main.projectile[num].hostile = true;
			for (int i = 0; i < 10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), base.projectile.width, base.projectile.height, 226, -base.projectile.velocity.X * 0.2f, -base.projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
				Main.dust[num2].noGravity = true;
				Main.dust[num2].velocity *= 2f;
			}
		}
	}
}
