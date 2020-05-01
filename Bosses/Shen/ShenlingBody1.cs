using System;
using AAMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen
{
	// Token: 0x02000425 RID: 1061
	public class ShenlingBody1 : Shenling
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x0011B197 File Offset: 0x00119397
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/ShenlingBody1";
			}
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0011B19E File Offset: 0x0011939E
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Discordian Serpent");
			Main.npcFrameCount[base.npc.type] = 1;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0011B1C2 File Offset: 0x001193C2
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.dontCountMe = true;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0011B1D8 File Offset: 0x001193D8
		public override void HitEffect(int hitDirection, double damage)
		{
			if (base.npc.life <= 0)
			{
				base.npc.position.X = base.npc.position.X + (float)(base.npc.width / 2);
				base.npc.position.Y = base.npc.position.Y + (float)(base.npc.height / 2);
				base.npc.width = 44;
				base.npc.height = 78;
				base.npc.position.X = base.npc.position.X - (float)(base.npc.width / 2);
				base.npc.position.Y = base.npc.position.Y - (float)(base.npc.height / 2);
				int num = ModContent.DustType<DiscordLight>();
				int num2 = ModContent.DustType<DiscordLight>();
				Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, num, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 0.5f;
				Main.dust[num].scale *= 1.3f;
				Main.dust[num].fadeIn = 1f;
				Main.dust[num].noGravity = false;
				Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, num2, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num2].velocity *= 0.5f;
				Main.dust[num2].scale *= 1.3f;
				Main.dust[num2].fadeIn = 1f;
				Main.dust[num2].noGravity = true;
			}
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0011B434 File Offset: 0x00119634
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
						int num = Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, base.mod.DustType("DiscordLight"), 0f, 0f, 100, default(Color), 2f);
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
			if ((double)base.npc.ai[1] < (double)Main.npc.Length)
			{
				Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
				float num2 = Main.npc[(int)base.npc.ai[1]].position.X + (float)(Main.npc[(int)base.npc.ai[1]].width / 2) - vector.X;
				float num3 = Main.npc[(int)base.npc.ai[1]].position.Y + (float)(Main.npc[(int)base.npc.ai[1]].height / 2) - vector.Y;
				base.npc.rotation = (float)Math.Atan2((double)num3, (double)num2) + 1.57f;
				float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
				float num5 = (num4 - (float)base.npc.width) / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				if (num2 < 0f)
				{
					base.npc.spriteDirection = 1;
				}
				else
				{
					base.npc.spriteDirection = -1;
				}
				base.npc.position.X = base.npc.position.X + num6;
				base.npc.position.Y = base.npc.position.Y + num7;
			}
			return false;
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0011B7F0 File Offset: 0x001199F0
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
	}
}
