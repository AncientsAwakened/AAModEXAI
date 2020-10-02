using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RajahDayStormBullet1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah DayStorm Bullet");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 500;
            projectile.alpha = 255;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);
            }
            if(projectile.alpha < 150)
            {
                float x2 = projectile.position.X + projectile.width / 2 - projectile.velocity.X / 2f;
				float y2 = projectile.position.Y + projectile.height / 2 - projectile.velocity.Y / 2f;
                int num166 = Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2), projectile.width, projectile.height + 5, ModContent.DustType<InfinityOverloadG>(), projectile.velocity.X * 0.2f,
                    projectile.velocity.Y * 0.2f, 0, default, 2f);
                Main.dust[num166].alpha = projectile.alpha;
                Main.dust[num166].position.X = x2;
				Main.dust[num166].position.Y = y2;
                Main.dust[num166].velocity *= 0f;
                Main.dust[num166].noGravity = true;
            }
            float num167 = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
            float num168 = projectile.localAI[0];
            if (num168 == 0f)
            {
                projectile.localAI[0] = num167;
                num168 = num167;
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            float num169 = projectile.position.X;
            float num170 = projectile.position.Y;
            float num171 = 300f;
            bool flag4 = false;
            int num172 = 0;
            if (projectile.ai[1] == 0f)
            {
                int num;
                for (int num173 = 0; num173 < 200; num173 = num + 1)
                {
                    if (Main.player[num173].active && Main.player[num173].statLife > 0 && (projectile.ai[1] == 0f || projectile.ai[1] == num173 + 1))
                    {
                        float num174 = Main.player[num173].position.X + Main.player[num173].width / 2;
                        float num175 = Main.player[num173].position.Y + Main.player[num173].height / 2;
                        float num176 = Math.Abs(projectile.position.X + projectile.width / 2 - num174) + Math.Abs(projectile.position.Y + projectile.height / 2 - num175);
                        if (num176 < num171 && Collision.CanHit(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2), 1, 1, Main.player[num173].position, Main.player[num173].width, Main.player[num173].height))
                        {
                            num171 = num176;
                            num169 = num174;
                            num170 = num175;
                            flag4 = true;
                            num172 = num173;
                        }
                    }
                    num = num173;
                }
                if (flag4)
                {
                    projectile.ai[1] = num172 + 1;
                }
                flag4 = false;
            }
            if (projectile.ai[1] > 0f)
            {
                int num177 = (int)(projectile.ai[1] - 1f);
                if (Main.player[num177].active && Main.player[num177].statLife > 0)
                {
                    float num178 = Main.player[num177].position.X + Main.player[num177].width / 2;
                    float num179 = Main.player[num177].position.Y + Main.player[num177].height / 2;
                    float num180 = Math.Abs(projectile.position.X + projectile.width / 2 - num178) + Math.Abs(projectile.position.Y + projectile.height / 2 - num179);
                    if (num180 < 1000f)
                    {
                        flag4 = true;
                        num169 = Main.player[num177].position.X + Main.player[num177].width / 2;
                        num170 = Main.player[num177].position.Y + Main.player[num177].height / 2;
                    }
                }
                else
                {
                    projectile.ai[1] = 0f;
                }
            }
            if (flag4)
            {
                float num181 = num168;
                Vector2 vector19 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                float num182 = num169 - vector19.X;
                float num183 = num170 - vector19.Y;
                float num184 = (float)Math.Sqrt(num182 * num182 + num183 * num183);
                num184 = num181 / num184;
                num182 *= num184;
                num183 *= num184;
                int num185 = 8;
                projectile.velocity.X = (projectile.velocity.X * (num185 - 1) + num182) / num185;
                projectile.velocity.Y = (projectile.velocity.Y * (num185 - 1) + num183) / num185;
            }
        }

        public override void Kill(int timeleft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<InfinityOverloadG>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188), 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<InfinityOverloadG>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188));
                Main.dust[num469].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 600);
        }
    }
}
