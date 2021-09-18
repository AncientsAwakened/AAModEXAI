using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    public class AkumaFireHeal : ModProjectile
	{
		public override string Texture => "AAModEXAI/BlankTex";
		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.extraUpdates = 3;
		}
		public override void AI()
		{
			int num492 = (int)projectile.ai[0];
			float num493 = 10f;
			Vector2 vector39 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
			float num494 = Main.npc[num492].Center.X - vector39.X;
			float num495 = Main.npc[num492].Center.Y - vector39.Y;
			float num496 = (float)Math.Sqrt(num494 * num494 + num495 * num495);
			if (num496 < 50f && projectile.position.X < Main.npc[num492].position.X + Main.npc[num492].width && projectile.position.X + projectile.width > Main.npc[num492].position.X && projectile.position.Y < Main.npc[num492].position.Y + Main.npc[num492].height && projectile.position.Y + projectile.height > Main.npc[num492].position.Y)
			{
				if (projectile.owner == Main.myPlayer)
				{
					int num497 = (int)projectile.ai[1];
					Main.npc[num492].life += num497;
					if (Main.npc[num492].life > Main.npc[num492].lifeMax)
					{
						Main.npc[num492].life = Main.npc[num492].lifeMax;
					}
					Main.npc[num492].netUpdate = true;
				}
				projectile.Kill();
			}
			num496 = num493 / num496;
			num494 *= num496;
			num495 *= num496;
			projectile.velocity.X = (projectile.velocity.X * 15f + num494) / 16f;
			projectile.velocity.Y = (projectile.velocity.Y * 15f + num495) / 16f;
			int num297 = ModContent.DustType<AkumaDust>();
			if (Main.rand.Next(2) == 0)
			{
				for (int num298 = 0; num298 < 3; num298++)
				{
					int num299 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, num297, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
					if (Main.rand.Next(3) == 0)
					{
						Main.dust[num299].noGravity = true;
						Main.dust[num299].scale *= 3f;
						Dust expr_DD5D_cp_0 = Main.dust[num299];
						expr_DD5D_cp_0.velocity.X *= 2f;
						Dust expr_DD7D_cp_0 = Main.dust[num299];
						expr_DD7D_cp_0.velocity.Y *= 2f;
					}
					Main.dust[num299].scale *= 1f;
					Dust expr_DDE2_cp_0 = Main.dust[num299];
					expr_DDE2_cp_0.velocity.X *= 1.2f;
					Dust expr_DE02_cp_0 = Main.dust[num299];
					expr_DE02_cp_0.velocity.Y *= 1.2f;
					Main.dust[num299].scale *= 0.5f;
					Main.dust[num299].velocity += projectile.velocity;
					if (!Main.dust[num299].noGravity)
					{
						Main.dust[num299].velocity *= 0.5f;
					}
				}
			}
			return;
		}
	}
}