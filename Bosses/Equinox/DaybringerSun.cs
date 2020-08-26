using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Equinox
{
    public class DaybringerSun : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybringer Sun");
		}

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.hostile = true;
            projectile.scale = 1f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			projectile.timeLeft = 700;
        }

        public override void AI()
        {
            if (projectile.timeLeft <= 0)
            {
                projectile.Kill();
            }

            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            
            const int aislotHomingCooldown = 0;
            const int homingDelay = 15;
            const float desiredFlySpeedInPixelsPerFrame = 14f;
            const float amountOfFramesToLerpBy = 30; // minimum of 1, please keep in full numbers even though it's a float!

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay;

                int foundTarget = HomeOnTarget();
                if(projectile.ai[1] == 0)
                {
                    if (foundTarget != -1)
                    {
                        Player target = Main.player[foundTarget];
                        Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * desiredFlySpeedInPixelsPerFrame;
                        projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                    }
                }
                else if(projectile.ai[1] == 1)
                {
                    if (foundTarget != -1)
                    {
                        NPC n = Main.npc[foundTarget];
                        Vector2 desiredVelocity = projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
                        projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 244, 171, 200);
        }

        private int HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 3000;
            
            int selectedTarget = -1;

            if(projectile.ai[1] == 0)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player target = Main.player[i];
                    if (target.active && (!target.wet || homingCanAimAtWetEnemies))
                    {
                        float distance = projectile.Distance(target.Center);
                        if (distance <= homingMaximumRangeInPixels &&
                            (
                                selectedTarget == -1 || //there is no selected target
                                projectile.Distance(Main.player[selectedTarget].Center) > distance) 
                        )
                            selectedTarget = i;
                    }
                }
            }
            else if(projectile.ai[1] == 1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n.CanBeChasedBy(projectile) && (!n.wet || homingCanAimAtWetEnemies))
                    {
                        float distance = projectile.Distance(n.Center);
                        if (distance <= homingMaximumRangeInPixels &&
                            (
                                selectedTarget == -1 || //there is no selected target
                                projectile.Distance(Main.npc[selectedTarget].Center) > distance) 
                        )
                            selectedTarget = i;
                    }
                }
            }
            

            return selectedTarget;
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 16; i++)
            {
                Vector2 shoot = new Vector2((float)Math.Sin(i * 0.125f * (float)Math.PI), (float)Math.Cos(i * 0.125f * (float)Math.PI));
                shoot *= 12f;
                int ball = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shoot.X, shoot.Y, 258, projectile.damage, 5, Main.myPlayer);
                Main.projectile[ball].timeLeft = 1500;
                Main.projectile[ball].tileCollide = false;
            }
            projectile.active = false;
        }
    }
}