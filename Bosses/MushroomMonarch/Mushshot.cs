using System;
using AAMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x0200047D RID: 1149
	internal class Mushshot : ModProjectile
	{
		// Token: 0x06001B32 RID: 6962 RVA: 0x00136E32 File Offset: 0x00135032
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Spore Blast");
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x00136E44 File Offset: 0x00135044
		public override void SetDefaults()
		{
			base.projectile.width = 10;
			base.projectile.height = 10;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.ignoreWater = true;
			base.projectile.penetrate = 1;
			base.projectile.alpha = 60;
			base.projectile.timeLeft = 180;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x001216C3 File Offset: 0x0011F8C3
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(AAColor.Glow);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00136EB8 File Offset: 0x001350B8
		public override void AI()
		{
			for (int i = 0; i < 1; i++)
			{
				int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, ModContent.DustType<ShroomDust>(), 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].scale *= 1.3f;
				Main.dust[num].fadeIn = 1f;
				Main.dust[num].noGravity = true;
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00136F6C File Offset: 0x0013516C
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i <= 3; i++)
			{
				Dust.NewDust(base.projectile.position + base.projectile.velocity, base.projectile.width, base.projectile.height, ModContent.DustType<ShroomDust>(), base.projectile.oldVelocity.X * 0.5f, base.projectile.oldVelocity.Y * 0.5f, 0, default(Color), 1f);
			}
		}
	}
}
