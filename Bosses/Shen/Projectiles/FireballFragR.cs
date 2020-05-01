using System;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x02000430 RID: 1072
	public class FireballFragR : ModProjectile
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x00120259 File Offset: 0x0011E459
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballFragR";
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00120260 File Offset: 0x0011E460
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

		// Token: 0x06001952 RID: 6482 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x001202C0 File Offset: 0x0011E4C0
		public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.hostile = true;
			base.projectile.timeLeft = 30;
			base.projectile.aiStyle = -1;
			this.cooldownSlot = 1;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0012032C File Offset: 0x0011E52C
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != 1)
			{
				Vector2 vector = Vector2.Normalize(base.projectile.velocity);
				for (int i = 0; i < 8; i++)
				{
					vector = Utils.RotatedBy(vector, 0.7853981633974483, default(Vector2));
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelR"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.01f), 0.01f);
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelR"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.01f), -0.01f);
				}
			}
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x000D2B90 File Offset: 0x000D0D90
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<HydraToxin>(), 180, true);
		}
	}
}
