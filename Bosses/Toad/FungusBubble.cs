/*
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Toad
{
    public class FungusBubble : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fungus Bubble");
		}
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 300;
            projectile.noEnchantments = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            const int homingDelay = 45;
            const float desiredFlySpeedInPixelsPerFrame = 3;
            const float amountOfFramesToLerpBy = 30;

            projectile.ai[0]++;
            if (projectile.ai[0] > homingDelay)
            {
                projectile.ai[0] = homingDelay;

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player target = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
        }

        private int HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 500;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                Player target = Main.player[i];
                if (target.active && (!target.wet || homingCanAimAtWetEnemies))
                {
                    float distance = projectile.Distance(target.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                    (
                        selectedTarget == -1 || projectile.Distance(Main.player[selectedTarget].Center) > distance)
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("Shroomed"), 180);
        }

        public override void Kill(int timeLeft)
        {
            for (int dust = 0; dust <= 5; dust++)
            {
                int dustType = ModLoader.GetMod("AAMod").DustType("ShroomDust");
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, dustType, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            return true;
        }

		public override bool OnTileCollide (Vector2 oldVelocity)
		{
			projectile.ai[0] = 1f;
			return false;
		}
    }
}
*/