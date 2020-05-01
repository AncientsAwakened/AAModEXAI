using System;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x02000435 RID: 1077
	public class FireballSpreadB : ModProjectile
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600197B RID: 6523 RVA: 0x00120F7D File Offset: 0x0011F17D
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballSpreadB";
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00120F84 File Offset: 0x0011F184
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

		// Token: 0x0600197E RID: 6526 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00120FE4 File Offset: 0x0011F1E4
		public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.hostile = true;
			base.projectile.timeLeft = 600;
			base.projectile.aiStyle = -1;
			this.cooldownSlot = 1;
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00121054 File Offset: 0x0011F254
		public override void AI()
		{
			float[] ai = base.projectile.ai;
			int num = 0;
			float num2 = ai[num] - 1f;
			ai[num] = num2;
			if (num2 == 0f)
			{
				base.projectile.netUpdate = true;
				base.projectile.velocity = Vector2.Zero;
			}
			float[] ai2 = base.projectile.ai;
			int num3 = 1;
			num2 = ai2[num3] - 1f;
			ai2[num3] = num2;
			if (num2 == 0f)
			{
				base.projectile.netUpdate = true;
				Player player = Main.player[(int)Player.FindClosest(base.projectile.position, base.projectile.width, base.projectile.height)];
				base.projectile.velocity = base.projectile.DirectionTo(player.Center + player.velocity * 30f) * 30f;
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x000D2EE4 File Offset: 0x000D10E4
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<DragonFire>(), 180, true);
		}
	}
}
