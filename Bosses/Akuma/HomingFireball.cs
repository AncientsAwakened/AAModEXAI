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

namespace AAModEXAI.Bosses.Akuma
{
    internal class HomingFireball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Firebomb");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
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
            return AAColor.Akuma;
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
            const int aislotHomingCooldown = 0;
            const int homingDelay = 0;
            const float desiredFlySpeedInPixelsPerFrame = 5;
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 300);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 20);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int proj = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("AkumaBoom"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Main.projectile[proj].netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int shader = Terraria.Graphics.Shaders.GameShaders.Armor.GetShaderIdFromItemId(Terraria.ID.ItemID.LivingFlameDye);

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 2);

            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], shader, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, Color.White, true);
            return false;
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