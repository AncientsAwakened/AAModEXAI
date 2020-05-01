using System;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x0200042F RID: 1071
	public class FireballAccelB : ModProjectile
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x001200ED File Offset: 0x0011E2ED
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballAccelB";
			}
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x001200F4 File Offset: 0x0011E2F4
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

		// Token: 0x0600194A RID: 6474 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00120154 File Offset: 0x0011E354
		public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.hostile = true;
			base.projectile.timeLeft = 720;
			base.projectile.aiStyle = -1;
			base.projectile.extraUpdates = 1;
			this.cooldownSlot = 1;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x001201D0 File Offset: 0x0011E3D0
		public override void AI()
		{
			base.projectile.velocity *= 1f + Math.Abs(base.projectile.ai[0]);
			Vector2 vector = Utils.RotatedBy(base.projectile.velocity, 1.5707963267948966, default(Vector2));
			vector *= base.projectile.ai[1];
			base.projectile.velocity += vector;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x000D2EE4 File Offset: 0x000D10E4
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<DragonFire>(), 180, true);
		}
	}
}
