using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class StaticSphere : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.hostile = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 320;
            projectile.tileCollide = true;
            projectile.aiStyle = 0;
            projectile.damage = 22;
            projectile.damage = 100;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.Oblivion;
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
            const float amountOfFramesToLerpBy = 30; // minimum of 1, please keep in full numbers even though it's a float!

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
        }

        private int HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 500;

            int selectedTarget = -1;
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

            return selectedTarget;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Glitch"), (int)projectile.Center.X, (int)projectile.Center.Y);
            float spread = 12f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
            double Angle = spread / 3f;
            double offsetAngle;
            int i;
            if (projectile.owner == Main.myPlayer)
            {
                for (i = 0; i < 3; i++)
                {
                    offsetAngle = startAngle + Angle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 6f), mod.ProjectileType("Static"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 6f), mod.ProjectileType("Static"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                }
            }

        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 4)
                    projectile.frame = 0;
            }
            return true;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (Main.rand.Next(7) == 0)
			{
				target.AddBuff(mod.BuffType("Unstable"), 180, true);
			}
		}
    }
}