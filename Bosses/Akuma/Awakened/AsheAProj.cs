using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    [AutoloadBossHead]
    public class AsheAProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashe Akuma");
            Main.projFrames[projectile.type] = 12;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.timeLeft = 600;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
            projectile.hide = true;
        }

        public override void AI()
        {
            projectile.hide = false;

            Frames(); 
            
            if (projectile.velocity.X < 0)
            {
                projectile.direction = -1;
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.direction = 1;
                projectile.spriteDirection = 1;
            }

            Player player = Main.player[(int)projectile.ai[0]];

            projectile.Center = player.Center;
            projectile.position.Y -= 400;
            projectile.position.X += 400 * (float)Math.Sin(2 * Math.PI / 180 * projectile.ai[1]++);

            if (++projectile.localAI[0] == 52)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(projectile.Center, Vector2.UnitY * 4, mod.ProjectileType("AkumaRock"), projectile.damage, 0, Main.myPlayer);
            }

            if (projectile.localAI[1] == 0)
            {
                projectile.localAI[1] = 1; 
                int pieCut = 20;
                Main.PlaySound(SoundID.Item14, projectile.position);
                for (int m = 0; m < pieCut; m++)
                {
                    int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, Color.White, 1.6f);
                    Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                    Main.dust[dustID].noLight = false;
                    Main.dust[dustID].noGravity = true;
                }
                for (int m = 0; m < pieCut; m++)
                {
                    int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, Color.White, 2f);
                    Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                    Main.dust[dustID].noLight = false;
                    Main.dust[dustID].noGravity = true;
                }
            }

            if (!NPC.AnyNPCs(mod.NPCType("AkumaA")) || NPC.AnyNPCs(mod.NPCType("AsheA")))
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
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
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 25, default, 1.5f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = projectile.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += projectile.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = projectile.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += projectile.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 12, 0, 0);

            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.spriteDirection, 12, frame, lightColor, true);
            return false;
        }

        public void Frames()
        {
            if (projectile.localAI[0] > 40)
            {
                if (projectile.localAI[0] < 43)
                {
                    projectile.frame = 4;
                }
                else if (projectile.localAI[0] < 46)
                {
                    projectile.frame = 5;
                }
                else if (projectile.localAI[0] < 49)
                {
                    projectile.frame = 6;
                }
                else if (projectile.localAI[0] < 52)
                {
                    projectile.frame = 7;
                }
                else if (projectile.localAI[0] < 55)
                {
                    projectile.frame = 8;
                }
                else if (projectile.localAI[0] < 58)
                {
                    projectile.frame = 9;
                }
                else if (projectile.localAI[0] < 61)
                {
                    projectile.frame = 10;
                }
                else if (projectile.localAI[0] < 64)
                {
                    projectile.frame = 11;
                }
                else
                {
                    projectile.localAI[0] = 0;
                }
            }
            else
            {
                if (projectile.frameCounter++ > 5)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame > 3)
                    {
                        projectile.frame = 0;
                    }
                }
            }
        }
    }
}


