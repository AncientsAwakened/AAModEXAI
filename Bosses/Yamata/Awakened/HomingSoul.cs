using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    internal class HomingSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Bomb");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 88;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale = 1.1f;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 60;
            projectile.timeLeft = 300;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 0)
            {
                projectile.timeLeft--;
            }
            if (projectile.timeLeft == 0)
            {
                projectile.Kill();
            }

            projectile.frameCounter++;
            if (projectile.frameCounter > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = 1;

            }
            else
            {
                projectile.spriteDirection = -1;
            }

            const int aislotHomingCooldown = 0;
            const int homingDelay = 00;
            const float desiredFlySpeedInPixelsPerFrame = 6;
            const float amountOfFramesToLerpBy = 20; // minimum of 1, please keep in full numbers even though it's a float!

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay; 

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player target = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
            for (int num189 = 0; num189 < 1; num189++)
            {
                int num190 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataADust"), 0f, 0f, 0);

                Main.dust[num190].scale *= 1.3f;
                Main.dust[num190].fadeIn = 1f;
                Main.dust[num190].noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("HydraToxin"), 600);
        }

        public override void Kill(int timeleft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataADust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 0);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataADust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 0);
                Main.dust[num469].velocity *= 2f;
            }
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("SoulBoomY"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
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
                            selectedTarget == -1 || //there is no selected target
                            projectile.Distance(Main.npc[selectedTarget].Center) > distance) 
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }
    }
}