using System;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x02000434 RID: 1076
	public class FireballSpreadR : ModProjectile
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x00120DC1 File Offset: 0x0011EFC1
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballSpreadR";
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00120DC8 File Offset: 0x0011EFC8
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

		// Token: 0x06001976 RID: 6518 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00120E28 File Offset: 0x0011F028
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

		// Token: 0x06001978 RID: 6520 RVA: 0x00120E98 File Offset: 0x0011F098
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

		// Token: 0x06001979 RID: 6521 RVA: 0x000D2B90 File Offset: 0x000D0D90
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<HydraToxin>(), 180, true);
		}
	}
}
