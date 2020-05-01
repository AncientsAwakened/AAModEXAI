using System;
using AAMod.Buffs;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
	// Token: 0x02000433 RID: 1075
	public class FireballHomingB : ModProjectile
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x001209C1 File Offset: 0x0011EBC1
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/Projectiles/FireballHomingB";
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00119223 File Offset: 0x00117423
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x001209C8 File Offset: 0x0011EBC8
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

		// Token: 0x0600196C RID: 6508 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00120A28 File Offset: 0x0011EC28
		public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.hostile = true;
			base.projectile.scale = 4f;
			base.projectile.aiStyle = -1;
			this.cooldownSlot = 1;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00120A98 File Offset: 0x0011EC98
		public override void AI()
		{
			base.projectile.velocity = base.projectile.DirectionTo(Main.player[(int)base.projectile.ai[0]].Center) * base.projectile.ai[1];
			float[] localAI = base.projectile.localAI;
			int num = 0;
			float num2 = localAI[num] + 1f;
			localAI[num] = num2;
			if (num2 > 60f)
			{
				base.projectile.localAI[0] = 0f;
				if (Main.netMode != 1)
				{
					Vector2 vector = Vector2.Normalize(base.projectile.velocity);
					for (int i = 0; i < 16; i++)
					{
						vector = Utils.RotatedBy(vector, 0.39269908169872414, default(Vector2));
						Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelB"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
						Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelB"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
					}
				}
			}
			base.projectile.scale -= 0.01f;
			if (base.projectile.scale <= 1f)
			{
				base.projectile.Kill();
			}
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00120C28 File Offset: 0x0011EE28
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != 1)
			{
				Vector2 vector = Vector2.Normalize(base.projectile.velocity);
				for (int i = 0; i < 16; i++)
				{
					vector = Utils.RotatedBy(vector, 0.39269908169872414, default(Vector2));
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelB"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
					Projectile.NewProjectile(base.projectile.Center, vector, base.mod.ProjectileType("FireballAccelB"), base.projectile.damage, 0f, Main.myPlayer, Math.Abs(0.015f), 0f);
				}
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x000D2EE4 File Offset: 0x000D10E4
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<DragonFire>(), 180, true);
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00120D0C File Offset: 0x0011EF0C
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height / 4, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, 0, 4, frame, new Color?(Color.White), true, default(Vector2));
			return false;
		}
	}
}
