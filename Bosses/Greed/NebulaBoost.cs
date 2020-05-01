using System;
using AAMod.Dusts;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Greed
{
	// Token: 0x02000498 RID: 1176
	public class NebulaBoost : ModProjectile
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000263D8 File Offset: 0x000245D8
		public override string Texture
		{
			get
			{
				return "AAMod/BlankTex";
			}
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x0014560C File Offset: 0x0014380C
		public override void SetDefaults()
		{
			base.projectile.width = 18;
			base.projectile.height = 18;
			base.projectile.friendly = true;
			base.projectile.hostile = true;
			base.projectile.penetrate = -1;
			base.projectile.damage = 0;
			base.projectile.timeLeft = 130;
			base.projectile.alpha = 20;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.usesLocalNPCImmunity = true;
			base.projectile.localNPCHitCooldown = 0;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x001456B0 File Offset: 0x001438B0
		public override void AI()
		{
			Projectile projectile = base.projectile;
			int frameCounter = projectile.frameCounter;
			projectile.frameCounter = frameCounter + 1;
			if (frameCounter > 5)
			{
				base.projectile.frameCounter = 0;
				base.projectile.frame++;
				if (base.projectile.frame > 3)
				{
					base.projectile.frame = 0;
				}
			}
			base.projectile.ai[0] += 1f;
			if (base.projectile.ai[0] > 0f)
			{
				base.projectile.ai[0] = 0f;
				int npc = BaseAI.GetNPC(base.projectile.Center, ModContent.NPCType<GreedA>(), -1f, null);
				if (npc != -1)
				{
					NPC npc2 = Main.npc[npc];
					Vector2 value = base.projectile.DirectionTo(npc2.Center) * 30f;
					base.projectile.velocity = Vector2.Lerp(base.projectile.velocity, value, 0.05f);
				}
			}
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x001457B4 File Offset: 0x001439B4
		public void Setstuff()
		{
			if (base.projectile.ai[0] == 0f)
			{
				this.t = base.mod.GetTexture("NPCs/Bosses/Greed/NebBoostA");
				this.c = Color.HotPink;
				return;
			}
			if (base.projectile.ai[0] == 1f)
			{
				this.t = base.mod.GetTexture("NPCs/Bosses/Greed/NebBoostD");
				this.c = Color.Blue;
				return;
			}
			this.t = base.mod.GetTexture("NPCs/Bosses/Greed/NebBoostH");
			this.c = Color.Red;
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00145850 File Offset: 0x00143A50
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			this.Setstuff();
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, this.t.Width, this.t.Height / 3, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, this.t, 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, base.projectile.direction, 4, frame, new Color?(ColorUtils.COLOR_GLOWPULSE), false, default(Vector2));
			return false;
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x001458F4 File Offset: 0x00143AF4
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.type == ModContent.NPCType<GreedA>())
			{
				if (base.projectile.ai[0] == 0f)
				{
					target.AddBuff(ModContent.BuffType<BuffA>(), 180, false);
				}
				else if (base.projectile.ai[0] == 1f)
				{
					target.AddBuff(ModContent.BuffType<BuffD>(), 180, false);
				}
				else
				{
					target.life += 1000;
					CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height), CombatText.HealLife, 1000, false, false);
				}
			}
			base.projectile.Kill();
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x001459B8 File Offset: 0x00143BB8
		public override void Kill(int timeleft)
		{
			this.Setstuff();
			int num = 20;
			for (int i = 0; i < num; i++)
			{
				int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X - 1f, base.projectile.Center.Y - 1f), 2, 2, ModContent.DustType<AbyssDust>(), 0f, 0f, 100, this.c, 1.6f);
				Main.dust[num2].velocity = BaseUtility.RotateVector(default(Vector2), new Vector2(6f, 0f), (float)i / (float)num * 6.28f);
				Main.dust[num2].noLight = false;
				Main.dust[num2].noGravity = true;
			}
			for (int j = 0; j < num; j++)
			{
				int num3 = Dust.NewDust(new Vector2(base.projectile.Center.X - 1f, base.projectile.Center.Y - 1f), 2, 2, ModContent.DustType<AbyssDust>(), 0f, 0f, 100, this.c, 2f);
				Main.dust[num3].velocity = BaseUtility.RotateVector(default(Vector2), new Vector2(9f, 0f), (float)j / (float)num * 6.28f);
				Main.dust[num3].noLight = false;
				Main.dust[num3].noGravity = true;
			}
		}

		// Token: 0x0400056D RID: 1389
		private Texture2D t;

		// Token: 0x0400056E RID: 1390
		private Color c;
	}
}
