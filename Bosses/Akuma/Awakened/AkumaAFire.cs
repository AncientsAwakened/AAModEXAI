using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    public class AkumaAFire : ModProjectile
    {
        public override string Texture => "AAMod/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akuma Fire");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.damage = 30;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 60;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 180)
            {
                projectile.timeLeft = 180;
            }
            int num297 = ModContent.DustType<AkumaADust>();
            if (Main.rand.Next(2) == 0)
            {
                for (int num298 = 0; num298 < 3; num298++)
                {
                    int num299 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, num297, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
                    Main.dust[num299].noGravity = true;
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[num299].scale *= 2f;
                        Dust expr_DD5D_cp_0 = Main.dust[num299];
                        expr_DD5D_cp_0.velocity.X *= 2f;
                        Dust expr_DD7D_cp_0 = Main.dust[num299];
                        expr_DD7D_cp_0.velocity.Y *= 2f;
                    }
                    Dust expr_DDE2_cp_0 = Main.dust[num299];
                    expr_DDE2_cp_0.velocity.X *= .4f;
                    Dust expr_DE02_cp_0 = Main.dust[num299];
                    expr_DE02_cp_0.velocity.Y *= 1.2f;
                    Main.dust[num299].scale *= .75f;
                    Main.dust[num299].velocity += projectile.velocity;
                    if (!Main.dust[num299].noGravity)
                    {
                        Main.dust[num299].velocity *= 0.5f;
                    }
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 600);
        }
    }
}