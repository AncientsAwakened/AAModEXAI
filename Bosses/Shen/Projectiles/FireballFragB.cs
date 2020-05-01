using System;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x02000431 RID: 1073
	public class FireballFragB : ModProjectile
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06001957 RID: 6487 RVA: 0x0012040C File Offset: 0x0011E60C
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballFragB";
			}
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00120414 File Offset: 0x0011E614
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

		// Token: 0x0600195A RID: 6490 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00120474 File Offset: 0x0011E674
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

		// Token: 0x0600195C RID: 6492 RVA: 0x001204E0 File Offset: 0x0011E6E0
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != 1)
			{
				Vector2 vector = Vector2.Normalize(base.projectile.velocity);
				for (int i = 0; i < 8; i++)
				{
					vector = Utils.RotatedBy(vector, 0.7853981633974483, default(Vector2));
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelB"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.01f), 0.01f);
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelB"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.01f), -0.01f);
				}
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x000D2EE4 File Offset: 0x000D10E4
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<DragonFire>(), 180, true);
		}
	}
}
