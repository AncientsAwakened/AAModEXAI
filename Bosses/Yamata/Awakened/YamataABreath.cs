using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    internal class YamataABreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Wrath");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 420;
            projectile.aiStyle = -1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                Main.PlaySound(SoundID.Item34, projectile.Center);
            }
            projectile.velocity *= 0.97f;
            if (projectile.ai[0] > 7f)
            {
                float num296 = 1f;
                if (projectile.ai[0] == 8f)
                {
                    num296 = 0.25f;
                }
                else if (projectile.ai[0] == 9f)
                {
                    num296 = 0.5f;
                }
                else if (projectile.ai[0] == 10f)
                {
                    num296 = 0.75f;
                }
                projectile.ai[0] += 1f;
                int num297 = ModContent.DustType<YamataADust>();
                if (Main.rand.Next(2) == 0)
                {
                    for (int num298 = 0; num298 < 3; num298++)
                    {
                        int num299 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, num297, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
                        if (Main.rand.Next(3) == 0)
                        {
                            Main.dust[num299].noGravity = true;
                            Main.dust[num299].scale *= 3f;
                            Dust expr_DD5D_cp_0 = Main.dust[num299];
                            expr_DD5D_cp_0.velocity.X *= 2f;
                            Dust expr_DD7D_cp_0 = Main.dust[num299];
                            expr_DD7D_cp_0.velocity.Y *= 2f;
                        }
                        Main.dust[num299].scale *= 1f;
                        Dust expr_DDE2_cp_0 = Main.dust[num299];
                        expr_DDE2_cp_0.velocity.X *= 1.2f;
                        Dust expr_DE02_cp_0 = Main.dust[num299];
                        expr_DE02_cp_0.velocity.Y *= 1.2f;
                        Main.dust[num299].scale *= num296;
                        Main.dust[num299].velocity += projectile.velocity;
                        if (!Main.dust[num299].noGravity)
                        {
                            Main.dust[num299].velocity *= 0.5f;
                        }
                    }
                }
            }
            else
            {
                projectile.ai[0] += 1f;
                projectile.width += 6;
                projectile.height += 6;
            }
            projectile.rotation += 0.3f * projectile.direction;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<DeBuffs.HydraToxin>(), 600);
        }
    }
}