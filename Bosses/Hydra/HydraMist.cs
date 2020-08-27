using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Hydra
{
    internal class HydraMist : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Breath");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.scale = 1.1f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            projectile.tileCollide = false;
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 60f)
            {
                projectile.ai[0] += 10f;
            }
            if (projectile.ai[0] > 255f)
            {
                projectile.Kill();
                projectile.ai[0] = 255f;
            }
            projectile.alpha = (int)(100.0 + projectile.ai[0] * 0.7);
            projectile.rotation += projectile.velocity.X * 0.1f;
            projectile.rotation += projectile.direction * 0.003f;
            projectile.velocity *= 0.96f;
            Rectangle rectangle5 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            for (int num886 = 0; num886 < 1000; num886++)
            {
                if (num886 != projectile.whoAmI && Main.projectile[num886].active && Main.projectile[num886].type >= 511 && Main.projectile[num886].type <= 513)
                {
                    Rectangle value53 = new Rectangle((int)Main.projectile[num886].position.X, (int)Main.projectile[num886].position.Y, Main.projectile[num886].width, Main.projectile[num886].height);
                    if (rectangle5.Intersects(value53))
                    {
                        Vector2 vector91 = Main.projectile[num886].Center - projectile.Center;
                        if (vector91.X == 0f && vector91.Y == 0f)
                        {
                            if (num886 < projectile.whoAmI)
                            {
                                vector91.X = -1f;
                                vector91.Y = 1f;
                            }
                            else
                            {
                                vector91.X = 1f;
                                vector91.Y = -1f;
                            }
                        }
                        vector91.Normalize();
                        vector91 *= 0.005f;
                        projectile.velocity -= vector91;
                        Main.projectile[num886].velocity += vector91;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Poison"), 300);
        }
    }
}