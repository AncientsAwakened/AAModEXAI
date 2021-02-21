using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero
{
    public class GenocideRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Genocide Rocket");

            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 240;
            projectile.tileCollide = true;
            projectile.aiStyle = 0;
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
                if (projectile.frame > 2)
                {
                    projectile.frame = 0;
                }
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            const int aislotHomingCooldown = 0;
            const int homingDelay = 0;
            const float desiredFlySpeedInPixelsPerFrame = 10;
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
            for (int num230 = 0; num230 < 2; num230++)
            {
                float num231 = 0f;
                float num232 = 0f;
                if (num230 == 1)
                {
                    num231 = projectile.velocity.X * 0.5f;
                    num232 = projectile.velocity.Y * 0.5f;
                }
                if (projectile.localAI[1] > 9f)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        int num233 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num231, projectile.position.Y + 3f + num232) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, ModContent.DustType<VoidDust>(), 0f, 0f, 100, default, 1f);
                        Main.dust[num233].scale *= 1.4f + Main.rand.Next(10) * 0.1f;
                        Main.dust[num233].velocity *= 0.2f;
                        Main.dust[num233].noGravity = true;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        int num234 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num231, projectile.position.Y + 3f + num232) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, ModContent.DustType<VoidDust>(), 0f, 0f, 100, default, 0.5f);
                        Main.dust[num234].fadeIn = 0.5f + Main.rand.Next(5) * 0.1f;
                        Main.dust[num234].velocity *= 0.05f;
                    }
                }
            }
        }

        int a = 0;

        public override void PostAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) a++;
            if (a == 40)
            {
                projectile.tileCollide = true;
                projectile.netUpdate = true;
            }
            if (a < 40)
            {
                projectile.tileCollide = false;
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
                            selectedTarget == -1 || //there is no selected target
                            projectile.Distance(Main.npc[selectedTarget].Center) > distance) 
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                for (int m = 0; m < 6; m++)
                {
                    int dustID = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<VoidDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                }
                Main.PlaySound(4, (int)projectile.Center.X, (int)projectile.Center.Y, 3);
            }

            Main.PlaySound(new LegacySoundStyle(2, 14, Terraria.Audio.SoundType.Sound));
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("GenocideBoom"), projectile.damage, 1, projectile.owner);
        }


    }
}
