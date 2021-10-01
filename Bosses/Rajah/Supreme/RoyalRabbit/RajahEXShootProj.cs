using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RajahEXShootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Hydra Bullet");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.scale = 1.2f;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.extraUpdates = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.timeLeft = 500;
            projectile.usesLocalNPCImmunity = true;

        }

        public override void AI()
        {
			if(projectile.timeLeft < 60)
			{
				projectile.velocity.Y += projectile.velocity.Y > 0f ? 0.04f : -0.04f;
				if(projectile.velocity.Y <= -8f) projectile.velocity.Y = -8f;
				if(projectile.velocity.Y >= 8f) projectile.velocity.Y = 8f;
			}
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			for (int i = 0; i < 3; i++)
			{
				int d = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<InfinityOverloadG>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
				if (Main.rand.Next(6) != 0)
				{
					Main.dust[d].noGravity = true;
					Main.dust[d].velocity.X *= 2f;
					Main.dust[d].velocity.Y *= 2f;
				}else
				{
					Main.dust[d].noGravity = true;
					Main.dust[d].velocity.X *= 1.2f;
					Main.dust[d].velocity.Y *= 1.2f;
				}
			}
            const int aislotHomingCooldown = 0;
            const int homingDelay = 0;
            const float desiredFlySpeedInPixelsPerFrame = 15;
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
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 400;

            int selectedTarget = -1;
            for (int i = 0; i < 200; i++)
            {
                Player n = Main.player[i];
                if (n.active && n.statLife > 0 && homingCanAimAtWetEnemies)
                {
                    float distance = projectile.Distance(n.Center);
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("InfinityOverload"), 180);
            projectile.netUpdate = true;
        }	

        public override void Kill(int timeLeft)
        {
            projectile.position = projectile.Center;
            projectile.width = projectile.height = 80;
            projectile.Center = projectile.position;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.Damage();
            Main.PlaySound(SoundID.Item14, projectile.position);
            Vector2 position = projectile.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<InfinityOverloadR>(), 0f, 0f, 200, default, 3.7f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<InfinityOverloadY>(), 0f, 0f, 100, default, 1.5f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Crimson * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, ModContent.DustType<InfinityOverloadR>(), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = projectile.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += projectile.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModContent.DustType<InfinityOverloadY>(), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = projectile.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += projectile.DirectionTo(Main.dust[num92].position) * 3f;
            }

            if (projectile.ai[1] == 1)
            {
                int num3;
                for (int num622 = 0; num622 < 20; num622 = num3 + 1)
                {
                    int num623 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 184, 0f, 0f, 0);
                    Dust dust = Main.dust[num623];
                    dust.scale *= 1.1f;
                    Main.dust[num623].noGravity = true;
                    num3 = num622;
                }
                for (int num624 = 0; num624 < 30; num624 = num3 + 1)
                {
                    int num625 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 184, 0f, 0f, 0);
                    Dust dust = Main.dust[num625];
                    dust.velocity *= 2.5f;
                    dust = Main.dust[num625];
                    dust.scale *= 0.8f;
                    Main.dust[num625].noGravity = true;
                    num3 = num624;
                }
                if (projectile.owner == Main.myPlayer)
                {
                    int num626 = 3;
                    if (Main.rand.Next(10) == 0)
                    {
                        num626++;
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        num626++;
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        num626++;
                    }
                    for (int num627 = 0; num627 < num626; num627 = num3 + 1)
                    {
                        float num628 = Main.rand.Next(-35, 36) * 0.02f;
                        float num629 = Main.rand.Next(-35, 36) * 0.02f;
                        num628 *= 10f;
                        num629 *= 10f;
                        int p = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, num628, num629, mod.ProjectileType("CarrowSplitR"), projectile.damage / 4, (int)(projectile.knockBack * 0.35), Main.myPlayer, 0f, 0f);
                        num3 = num627;
                        Main.projectile[p].timeLeft = 240;
                    }
                }

            }
        }

        public override Color? GetAlpha(Color lightColor)
		{
			return new Color(Math.Max((int)Main.mouseTextColor, lightColor.R), Math.Max((int)Main.mouseTextColor, lightColor.G), Math.Max((int)Main.mouseTextColor, lightColor.B), Main.mouseTextColor);
		}
    }
}
