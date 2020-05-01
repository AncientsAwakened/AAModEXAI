using System;
using AAMod.Buffs;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x02000432 RID: 1074
	public class FireballHomingR : ModProjectile
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x001205C0 File Offset: 0x0011E7C0
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballHomingR";
			}
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x001205C8 File Offset: 0x0011E7C8
		public override void PostAI()
		{
			Projectile projectile = base.projectile;
			int frameCounter = projectile.frameCounter;
			projectile.frameCounter = frameCounter + 1;
			if (frameCounter > 5)
			{
				base.projectile.frame++;
				base.projectile.frameCounter = 0;
				if (base.projectile.frame > 3)
				{
					base.projectile.frame = 0;
				}
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00120628 File Offset: 0x0011E828
		public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.hostile = true;
			base.projectile.scale = 4f;
			base.projectile.aiStyle = -1;
			this.cooldownSlot = 1;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00120698 File Offset: 0x0011E898
		public override void AI()
		{
			base.projectile.velocity = base.projectile.DirectionTo(Main.player[(int)base.projectile.ai[0]].Center) * base.projectile.ai[1];
			float[] localAI = base.projectile.localAI;
			int num = 0;
			float num2 = localAI[num] + 1f;
			localAI[num] = num2;
			if (num2 > 60f)
			{
				base.projectile.localAI[0] = 0f;
				if (Main.netMode != 1)
				{
					Vector2 vector = Vector2.Normalize(base.projectile.velocity);
					for (int i = 0; i < 16; i++)
					{
						vector = Utils.RotatedBy(vector, 0.39269908169872414, default(Vector2));
						Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelR"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
						Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelR"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
					}
				}
			}
			base.projectile.scale -= 0.01f;
			if (base.projectile.scale <= 1f)
			{
				base.projectile.Kill();
			}
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00120828 File Offset: 0x0011EA28
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != 1)
			{
				Vector2 vector = Vector2.Normalize(base.projectile.velocity);
				for (int i = 0; i < 16; i++)
				{
					vector = Utils.RotatedBy(vector, 0.39269908169872414, default(Vector2));
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelR"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelR"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
				}
			}
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x000D2B90 File Offset: 0x000D0D90
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<HydraToxin>(), 180, true);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0012090C File Offset: 0x0011EB0C
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height / 4, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, 0, 4, frame, new Color?(Color.White), true, default(Vector2));
			return false;
		}
	}
}
