using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Shen
{
    internal class ShenABreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Breath");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.damage = 25;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 100;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 60)
            {
                projectile.timeLeft = 60;
            }
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
                int num297 = ModContent.DustType<DiscordDust>();
                if (Main.rand.Next(2) == 0)
                {
                    for (int num298 = 0; num298 < 8; num298++)
                    {
                        int num299 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, num297, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, Color.White, 2) ;
                        Main.dust[num299].noGravity = true;
                        if (Main.rand.Next(3) == 0)
                        {
                            Dust expr_DD5D_cp_0 = Main.dust[num299];
                            Main.dust[num299].scale *= 2f;
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
            }
            projectile.rotation += 0.3f * projectile.direction;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DiscordInferno"), 180);
        }
    }
}