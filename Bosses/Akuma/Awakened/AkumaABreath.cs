﻿using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    internal class AkumaABreath : ModProjectile
    {
        public override string Texture => "AAModEXAI/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Fury");
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
            projectile.timeLeft = 60;
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
            target.AddBuff(ModContent.BuffType<DeBuffs.DragonFire>(), 600);
        }
    }
}