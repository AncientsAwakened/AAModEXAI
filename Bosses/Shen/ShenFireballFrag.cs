using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;using AAMod;

namespace AAModEXAI.Bosses.Shen
{
	// Token: 0x02000421 RID: 1057
	public class ShenFireballFrag : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fireball");
			Main.projFrames[projectile.type] = 4;
		}


		public override void PostAI()
		{
			 
			int frameCounter = projectile.frameCounter;
			projectile.frameCounter = frameCounter + 1;
			if (frameCounter > 5)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame > 3)
				{
					projectile.frame = 0;
				}
			}
		}


		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00119410 File Offset: 0x00117610
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.timeLeft = 30;
			projectile.aiStyle = -1;
			this.cooldownSlot = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DiscordInferno"), 180);
        }

		// Token: 0x060018DE RID: 6366 RVA: 0x0011947C File Offset: 0x0011767C
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != 1)
			{
				Vector2 vector = Vector2.Normalize(projectile.velocity);
				for (int i = 0; i < 8; i++)
				{
					vector = Utils.RotatedBy(vector, 0.7853981633974483, default(Vector2));
					Projectile.NewProjectile(projectile.Center, vector, mod.ProjectileType("ShenFireballAccel"), projectile.damage, 0f, Main.myPlayer, Math.Abs(0.01f), 0.01f);
					Projectile.NewProjectile(projectile.Center, vector, mod.ProjectileType("ShenFireballAccel"), projectile.damage, 0f, Main.myPlayer, Math.Abs(0.01f), -0.01f);
				}
			}
		}
	}
}
