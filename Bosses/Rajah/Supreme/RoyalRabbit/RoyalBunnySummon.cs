using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RoyalBunnySummon : ModProjectile
    {
        public override string Texture => "AAModEXAI/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Bunny Summon");
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.timeLeft = 120;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, 0, 0, ModContent.DustType<AbyssDust>(), 0f, 0f, 0, Main.DiscoColor, 1f);
                Main.dust[num469].noGravity = true;
            }
            projectile.damage = 0;
            projectile.knockBack = 0;
            if (projectile.timeLeft < 10)
            {
                Kill(projectile.timeLeft);
            }
        }

        public override void Kill(int timeLeft)
        {
            for(int proj = 0; proj < 1000; proj ++)
            {
                if (Main.projectile[proj].active && Main.projectile[proj].friendly && !Main.projectile[proj].hostile)
                {
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].friendly = false;
                    Vector2 vector = Main.projectile[proj].Center - projectile.Center;
                    vector.Normalize();
                    Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    reflectvelocity.Normalize();
                    reflectvelocity *= vector.Length();
                    reflectvelocity += vector * 20f;
                    reflectvelocity.Normalize();
                    reflectvelocity *= vector.Length();
                    if(reflectvelocity.Length() < 20f)
                    {
                        reflectvelocity.Normalize();
                        reflectvelocity *= 20f;
                    }

                    Main.projectile[proj].penetrate = 1;

                    Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                    Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                    Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                }
            }

            int MinionType = (int)projectile.ai[0];

            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                int Minion = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, MinionType, 0);
                Main.npc[Minion].netUpdate2 = true;
                projectile.active = false;
                projectile.netUpdate2 = true;
            }
        }

        public void Move(Vector2 point)
        {
            float Speed = 13;

            float velMultiplier = 1f;
            Vector2 dist = point - projectile.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < Speed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / Speed);
            }
            if (length < 200f)
            {
                Speed *= 0.5f;
            }
            if (length < 100f)
            {
                Speed *= 0.5f;
            }
            if (length < 50f)
            {
                Speed *= 0.5f;
            }
            projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            projectile.velocity *= Speed;
            projectile.velocity *= velMultiplier;
        }
    }
}
