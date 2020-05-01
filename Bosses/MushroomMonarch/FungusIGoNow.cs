using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x02000478 RID: 1144
	public class FungusIGoNow : ModProjectile
	{
		// Token: 0x06001B15 RID: 6933 RVA: 0x001359C1 File Offset: 0x00133BC1
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Feudal Fungus");
			Main.projFrames[base.projectile.type] = 4;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x001359E8 File Offset: 0x00133BE8
		public override void SetDefaults()
		{
			base.projectile.damage = 24;
			base.projectile.width = 74;
			base.projectile.height = 80;
			base.projectile.penetrate = -1;
			base.projectile.hostile = true;
			base.projectile.friendly = false;
			base.projectile.tileCollide = false;
			base.projectile.ignoreWater = true;
			base.projectile.timeLeft = 900;
			base.projectile.alpha = 0;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00135A74 File Offset: 0x00133C74
		public override void AI()
		{
			Projectile projectile = base.projectile;
			int num = projectile.frameCounter + 1;
			projectile.frameCounter = num;
			if (num >= 14)
			{
				base.projectile.frameCounter = 0;
				Projectile projectile2 = base.projectile;
				num = projectile2.frame + 1;
				projectile2.frame = num;
				if (num >= 4)
				{
					base.projectile.frame = 0;
				}
			}
			base.projectile.velocity *= 0f;
			base.projectile.alpha += 4;
			if (base.projectile.alpha >= 255)
			{
				base.projectile.active = false;
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x00135B18 File Offset: 0x00133D18
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = base.mod.GetTexture("Glowmasks/FeudalFungusIGoNow_Glow");
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height / 4, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, 0, 4, frame, new Color?(base.projectile.GetAlpha(lightColor)), true, default(Vector2));
			BaseDrawing.DrawTexture(spriteBatch, texture, 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, 0, 4, frame, new Color?(base.projectile.GetAlpha(AAColor.Glow)), true, default(Vector2));
			return false;
		}
	}
}
