using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    public class AkumaAFireballAccel : ModProjectile
    {
        public override string Texture =>"AAModEXAI/Bosses/Akuma/Awakened/AkumaAFireballFrag";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireball");
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

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.timeLeft = 720;
			projectile.aiStyle = -1;
			projectile.extraUpdates = 1;
			cooldownSlot = 1;
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00119324 File Offset: 0x00117524
		public override void AI()
		{
			projectile.velocity *= 1f + Math.Abs(projectile.ai[0]);
			Vector2 vector = Utils.RotatedBy(projectile.velocity, 1.5707963267948966, default(Vector2));
			vector *= projectile.ai[1];
			projectile.velocity += vector;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<DeBuffs.DragonFire>(), 180);
        }
    }
}