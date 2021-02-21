
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero
{
    // to investigate: Projectile.Damage, (8843)
    class RiftZ : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
            projectile.rotation += 0.03f;
            if (projectile.timeLeft > 40)
            {
                if (projectile.alpha > 30)
                {
                    projectile.alpha -= 3;
                }
                else
                {
                    projectile.alpha = 30;
                }
            }
            else
            {
                projectile.alpha += 3;
            }

            const int aislotHomingCooldown = 0;
            const int homingDelay = 30;
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
            const float homingMaximumRangeInPixels = 400;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                Player n = Main.player[i];
                float distance = projectile.Distance(n.Center);
                if (distance <= homingMaximumRangeInPixels &&
                    (
                        selectedTarget == -1 || //there is no selected target
                        projectile.Distance(Main.npc[selectedTarget].Center) > distance)
                )
                    selectedTarget = i;
            }

            return selectedTarget;
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Texture2D Tex2 = mod.GetTexture("Bosses/Zero/RiftZ2");
            Texture2D Tex3 = mod.GetTexture("Bosses/Zero/RiftZ3");
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Tex.Width, Tex.Height, 0, 0);
            Rectangle frame1 = BaseDrawing.GetFrame(projectile.frame, Tex2.Width, Tex2.Height, 0, 0);
            Rectangle frame2 = BaseDrawing.GetFrame(projectile.frame, Tex3.Width, Tex3.Height, 0, 0);
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawTexture(spritebatch, Tex2, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 1, frame1, projectile.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawTexture(spritebatch, Tex3, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame2, projectile.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            float spread = 30f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 6f;
            double offsetAngle;
            for (int i = 0; i < 3; i++)
            {
                offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), mod.ProjectileType("RiftSlashZ"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), mod.ProjectileType("RiftSlashZ"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
            }
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModContent.DustType<VoidDust>(), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }
            projectile.active = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.active = false;
            return true;
        }
    }
}
