using System;
using AAMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Yamata
{
	// Token: 0x020003F4 RID: 1012
	public class Yanker : ModProjectile
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x001029C4 File Offset: 0x00100BC4
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Soul Chomper");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x001029E8 File Offset: 0x00100BE8
		public override void SetDefaults()
		{
			base.projectile.width = 30;
			base.projectile.height = 30;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.melee = true;
			base.projectile.tileCollide = false;
			base.projectile.ignoreWater = true;
			base.projectile.penetrate = 4;
			base.projectile.timeLeft = 300;
			base.projectile.aiStyle = 1;
			this.aiType = 14;
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00102A7C File Offset: 0x00100C7C
		public override void AI()
		{
			base.projectile.frameCounter++;
			if (base.projectile.frameCounter > 5)
			{
				base.projectile.frame++;
				base.projectile.frameCounter = 0;
				if (base.projectile.frame > 3)
				{
					base.projectile.frame = 0;
				}
			}
			if (base.projectile.velocity.X < 0f)
			{
				base.projectile.spriteDirection = -1;
				base.projectile.rotation = (float)Math.Atan2((double)(-(double)base.projectile.velocity.Y), (double)(-(double)base.projectile.velocity.X));
			}
			else
			{
				base.projectile.spriteDirection = 1;
				base.projectile.rotation = (float)Math.Atan2((double)base.projectile.velocity.Y, (double)base.projectile.velocity.X);
			}
			int num = 8;
			int num2 = Dust.NewDust(new Vector2(base.projectile.position.X + (float)num, base.projectile.position.Y + (float)num), base.projectile.width - num * 2, base.projectile.height - num * 2, 6, 0f, 0f, 0, default(Color), 1f);
			Main.dust[num2].noGravity = true;
			int num3 = Dust.NewDust(new Vector2(base.projectile.position.X + (float)num, base.projectile.position.Y + (float)num), base.projectile.width - num * 2, base.projectile.height - num * 2, 6, 0f, 0f, 0, default(Color), 1f);
			Main.dust[num3].noGravity = true;
			base.projectile.ai[0] += 1f;
			if (base.projectile.ai[0] > 10f)
			{
				base.projectile.ai[0] = 10f;
				int num4 = this.HomeOnTarget();
				if (num4 != -1)
				{
					NPC npc = Main.npc[num4];
					Vector2 value = base.projectile.DirectionTo(npc.Center) * 50f;
					base.projectile.velocity = Vector2.Lerp(base.projectile.velocity, value, 0.05f);
				}
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00102D04 File Offset: 0x00100F04
		private int HomeOnTarget()
		{
			int num = -1;
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.CanBeChasedBy(base.projectile, false))
				{
					bool wet = npc.wet;
					float num2 = base.projectile.Distance(npc.Center);
					if (num2 <= 5000f && (num == -1 || base.projectile.Distance(Main.npc[num].Center) > num2))
					{
						num = i;
					}
				}
			}
			return num;
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00102D7C File Offset: 0x00100F7C
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, base.projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, ModContent.DustType<YamataDust>(), -base.projectile.velocity.X * 0.2f, -base.projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= 2f;
				num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, ModContent.DustType<YamataDust>(), -base.projectile.velocity.X * 0.2f, -base.projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
				Main.dust[num].velocity *= 2f;
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00102EFF File Offset: 0x001010FF
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(base.mod.BuffType("Yanked"), 120, false);
		}
	}
}
