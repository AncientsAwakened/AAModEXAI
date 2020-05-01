using System;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x0200042E RID: 1070
	public class FireballAccelR : ModProjectile
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x0011FF80 File Offset: 0x0011E180
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballAccelR";
			}
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0011FF88 File Offset: 0x0011E188
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

		// Token: 0x06001942 RID: 6466 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0011FFE8 File Offset: 0x0011E1E8
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

		// Token: 0x06001944 RID: 6468 RVA: 0x00120064 File Offset: 0x0011E264
		public override void AI()
		{
			base.projectile.velocity *= 1f + Math.Abs(base.projectile.ai[0]);
			Vector2 vector = Utils.RotatedBy(base.projectile.velocity, 1.5707963267948966, default(Vector2));
			vector *= base.projectile.ai[1];
			base.projectile.velocity += vector;
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x000D2B90 File Offset: 0x000D0D90
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<HydraToxin>(), 180, true);
		}
	}
}
