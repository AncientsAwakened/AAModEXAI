using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Yamata
{
    public class Yanker : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Chomper");
            Main.projFrames[projectile.type] = 4;
        }
    	
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 4; //
            projectile.timeLeft = 300;
            projectile.aiStyle = 1; //
            aiType = ProjectileID.Bullet;
        }

        public override void AI()
        {

            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(-projectile.velocity.Y, -projectile.velocity.X);
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            }
            int num557 = 8;
            //dust!
            int dustId = Dust.NewDust(new Vector2(projectile.position.X + num557, projectile.position.Y + num557), projectile.width - num557 * 2, projectile.height - num557 * 2, 6, 0f, 0f, 0);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(new Vector2(projectile.position.X + num557, projectile.position.Y + num557), projectile.width - num557 * 2, projectile.height - num557 * 2, 6, 0f, 0f, 0);
            Main.dust[dustId3].noGravity = true;

            const int aislotHomingCooldown = 0;
            const int homingDelay = 10;
            const float desiredFlySpeedInPixelsPerFrame = 10;
            const float amountOfFramesToLerpBy = 20; // minimum of 1, please keep in full numbers even though it's a float!

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay; 

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player n = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
        }

        private int HomeOnTarget()
        {
            const float homingMaximumRangeInPixels = 5000;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player n = Main.player[i];
                if (!n.immune)
                {
                    float distance = projectile.Distance(n.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                        (
                            selectedTarget == -1 ||  projectile.Distance(Main.player[selectedTarget].Center) > distance)
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }
        

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int num579 = 0; num579 < 20; num579++)
            {
                int num580 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<AAMod.Dusts.YamataDust>(), -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num580].noGravity = true;
                Main.dust[num580].velocity *= 2f;
                num580 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<AAMod.Dusts.YamataDust>(), -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100);
                Main.dust[num580].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Yanked"), 120);
        }
    }
}