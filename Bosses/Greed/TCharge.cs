using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Greed
{
	// Token: 0x0200049B RID: 1179
	public class TCharge : ModProjectile
	{
		// Token: 0x06001C19 RID: 7193 RVA: 0x0005DCCC File Offset: 0x0005BECC
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Demise Sphere");
			Main.projFrames[base.projectile.type] = 3;
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00145B7C File Offset: 0x00143D7C
		public override void SetDefaults()
		{
			base.projectile.width = 20;
			base.projectile.height = 20;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.penetrate = 1;
			base.projectile.timeLeft = 120;
			base.projectile.alpha = 20;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = true;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00145BFC File Offset: 0x00143DFC
		public override void AI()
		{
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

		// Token: 0x06001C1C RID: 7196 RVA: 0x00145C5C File Offset: 0x00143E5C
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height / 3, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, 0, 3, frame, new Color?(Color.White), false, default(Vector2));
			return false;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x00145D14 File Offset: 0x00143F14
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
