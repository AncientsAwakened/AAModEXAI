using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x02000476 RID: 1142
	public class FungusCloud : ModProjectile
	{
		// Token: 0x06001B0B RID: 6923 RVA: 0x00135696 File Offset: 0x00133896
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fungus Cloud");
			Main.projFrames[base.projectile.type] = 5;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x001356BC File Offset: 0x001338BC
		public override void SetDefaults()
		{
			base.projectile.width = 10;
			base.projectile.height = 10;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.ignoreWater = true;
			base.projectile.penetrate = -11;
			base.projectile.extraUpdates = 1;
			base.projectile.scale = 1.1f;
			base.projectile.penetrate = -1;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x001216C3 File Offset: 0x0011F8C3
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(AAColor.Glow);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0013573C File Offset: 0x0013393C
		public override void AI()
		{
			base.projectile.velocity *= 0.98f;
			base.projectile.alpha += 2;
			if (base.projectile.alpha > 255)
			{
				base.projectile.Kill();
			}
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x00135794 File Offset: 0x00133994
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			base.projectile.frameCounter++;
			if (base.projectile.frameCounter >= 5)
			{
				base.projectile.frame++;
				base.projectile.frameCounter = 0;
				if (base.projectile.frame > 4)
				{
					base.projectile.frame = 0;
				}
			}
			return true;
		}
	}
}
