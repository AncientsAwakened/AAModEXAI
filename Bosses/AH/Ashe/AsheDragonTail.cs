using System;
using AAMod.Dusts;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.AH.Ashe
{
	// Token: 0x020004EF RID: 1263
	public class AsheDragonTail : AsheDragon
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x0016CD0A File Offset: 0x0016AF0A
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/AH/Ashe/AsheDragonTail";
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0016B5CA File Offset: 0x001697CA
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Ashen Dragon");
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0016B5DC File Offset: 0x001697DC
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.dontCountMe = true;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0016CD14 File Offset: 0x0016AF14
		public override void HitEffect(int hitDirection, double damage)
		{
			if (base.npc.life <= 0)
			{
				base.npc.position.X = base.npc.Center.X;
				base.npc.position.Y = base.npc.Center.Y;
				base.npc.width = 44;
				base.npc.height = 78;
				base.npc.position.X = base.npc.Center.X;
				base.npc.position.Y = base.npc.Center.Y;
				int num = ModContent.DustType<AkumaDust>();
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, num, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 0.5f;
				Main.dust[num].scale *= 1.3f;
				Main.dust[num].fadeIn = 1f;
				Main.dust[num].noGravity = false;
				int num2 = ModContent.DustType<AkumaDust>();
				Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, num2, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num2].velocity *= 0.5f;
				Main.dust[num2].scale *= 1.3f;
				Main.dust[num2].fadeIn = 1f;
				Main.dust[num2].noGravity = true;
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0016CF00 File Offset: 0x0016B100
		public override bool PreAI()
		{
			if (base.npc.ai[3] > 0f)
			{
				base.npc.realLife = (int)base.npc.ai[3];
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead)
			{
				base.npc.TargetClosest(true);
			}
			if (Main.player[base.npc.target].dead && base.npc.timeLeft > 300)
			{
				base.npc.timeLeft = 300;
			}
			if (Main.netMode != 1 && !Main.npc[(int)base.npc.ai[1]].active)
			{
				base.npc.life = 0;
				base.npc.HitEffect(0, 10.0);
				base.npc.active = false;
				base.npc.netUpdate = true;
			}
			if (Main.npc[(int)base.npc.ai[1]].alpha < 128)
			{
				if (base.npc.alpha != 0)
				{
					for (int i = 0; i < 2; i++)
					{
						int num = Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, base.mod.DustType("AkumaDust"), 0f, 0f, 100, default(Color), 2f);
						Main.dust[num].noGravity = false;
						Main.dust[num].noLight = false;
					}
				}
				base.npc.alpha -= 42;
				if (base.npc.alpha < 0)
				{
					base.npc.alpha = 0;
				}
			}
			base.npc.direction = ((base.npc.velocity.X < 0f) ? -1 : 1);
			if ((double)base.npc.ai[1] < (double)Main.npc.Length)
			{
				float num2 = Main.npc[(int)base.npc.ai[1]].Center.X - base.npc.Center.X;
				float num3 = Main.npc[(int)base.npc.ai[1]].Center.Y - base.npc.Center.Y;
				base.npc.rotation = (float)Math.Atan2((double)num3, (double)num2) + 1.57f;
				float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
				float num5 = (num4 - (float)base.npc.width) / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				base.npc.direction = ((num2 < 0f) ? 1 : -1);
				NPC npc = base.npc;
				npc.position.X = npc.position.X + num6;
				NPC npc2 = base.npc;
				npc2.position.Y = npc2.position.Y + num7;
			}
			return false;
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0016D220 File Offset: 0x0016B420
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[base.npc.type], 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.direction, 1, base.npc.frame, new Color?(new Color((int)Color.White.R, (int)Color.White.G, (int)Color.White.B, 100)), true, default(Vector2));
			return false;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0016D2D4 File Offset: 0x0016B4D4
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Main.player[base.npc.target].vortexStealthActive && projectile.ranged)
			{
				damage /= 2;
				crit = false;
			}
			if (projectile.penetrate == -1 && !projectile.minion)
			{
				int damage2 = projectile.damage;
				projectile.damage = 0;
				return;
			}
			if (projectile.penetrate >= 1)
			{
				int damage3 = projectile.damage;
				projectile.damage = 0;
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0016BC42 File Offset: 0x00169E42
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			base.npc.lifeMax = (int)((float)base.npc.lifeMax * 0.8f * bossLifeScale);
			base.npc.damage = (int)((float)base.npc.damage * 0.8f);
		}
	}
}
