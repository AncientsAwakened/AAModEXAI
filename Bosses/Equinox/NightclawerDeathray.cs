using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using AAMod;

namespace AAModEXAI.Bosses.Equinox
{
    public class NightclawerDeathray : ModProjectile
    {
        public float maxTime = 180;
        public float maxScale = 3f;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightclawer Deathray");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
            cooldownSlot = 1;
        }

        public override bool CanDamage()
        {
            return projectile.scale >= 1f;
        }

        public override void AI()
        {
            Vector2? vector78 = null;
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if (Main.npc[(int)projectile.ai[1]].active && (Main.npc[(int)projectile.ai[1]].type == mod.NPCType("NightcrawlerBody")) && !Main.dayTime)
            {
                if (projectile.ai[0] == 0)
                {
                    Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[(int)projectile.ai[1]].rotation + 3.1415f)) * 8f;
                    projectile.velocity = speed;
                    projectile.position += 30 * projectile.velocity;
                    projectile.position += 10 * projectile.velocity.RotatedBy(Main.npc[(int)projectile.ai[1]].spriteDirection > 0 ? -Math.PI / 2 : Math.PI / 2);
                }
                else
                {
                    projectile.velocity = (Main.npc[(int)projectile.ai[1]].rotation + (float)Math.PI / 2).ToRotationVector2();
                    projectile.velocity = projectile.velocity.RotatedBy(projectile.ai[0]);
                }
                projectile.Center = Main.npc[(int)projectile.ai[1]].Center;
            }
            else
            {
                projectile.Kill();
                return;
            }
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if (projectile.localAI[0] == 0f && maxScale >= 1)
            {
                Main.PlaySound(29, (int)projectile.position.X, (int)projectile.position.Y, 104, 1f, 0f);
            }
            float num801 = maxScale;
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= maxTime)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * 3.14159274f / maxTime) * 10f * num801;
            if (projectile.scale > num801)
            {
                projectile.scale = num801;
            }
            float num804 = projectile.velocity.ToRotation();
            //num804 += projectile.ai[0];
            projectile.rotation = num804 - 1.57079637f;
            projectile.velocity = num804.ToRotationVector2();
            float num805 = 3f;
            float num806 = projectile.width;
            Vector2 samplingPoint = projectile.Center;
            if (vector78.HasValue)
            {
                samplingPoint = vector78.Value;
            }
            float[] array3 = new float[(int)num805];
            for (int i = 0; i < array3.Length; i++) array3[i] = 4000f;
            float num807 = 0f;
            int num3;
            for (int num808 = 0; num808 < array3.Length; num808 = num3 + 1)
            {
                num807 += array3[num808];
                num3 = num808;
            }
            num807 /= num805;
            float amount = 0.5f;
            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num807, amount);
            Vector2 vector79 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
            for (int num809 = 0; num809 < 2; num809 = num3 + 1)
            {
                float num810 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float num811 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector80 = new Vector2((float)Math.Cos(num810) * num811, (float)Math.Sin(num810) * num811);
                int num812 = Dust.NewDust(vector79, 0, 0, 244, vector80.X, vector80.Y, 0, default, 1f);
                Main.dust[num812].noGravity = true;
                Main.dust[num812].scale = 1.7f;
                num3 = num809;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 value29 = projectile.velocity.RotatedBy(1.5707963705062866, default) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                int num813 = Dust.NewDust(vector79 + value29 - Vector2.One * 4f, 8, 8, 244, 0f, 0f, 100, default, 1.5f);
                Dust dust = Main.dust[num813];
                dust.velocity *= 0.5f;
                Main.dust[num813].velocity.Y = -Math.Abs(Main.dust[num813].velocity.Y);
            }
            //DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            //Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            Texture2D texture2D19 = Main.projectileTexture[projectile.type];
            Texture2D texture2D20 = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Equinox/NightclawerDeathray2");
            Texture2D texture2D21 = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Equinox/NightclawerDeathray3");
            float num223 = projectile.localAI[1];
            Color color44 = new Color(255, 255, 255, 0) * 0.5f;
            SpriteBatch arg_ABD8_0 = Main.spriteBatch;
            Texture2D arg_ABD8_1 = texture2D19;
            Vector2 arg_ABD8_2 = projectile.Center - Main.screenPosition;
            Rectangle? sourceRectangle2 = null;
            arg_ABD8_0.Draw(arg_ABD8_1, arg_ABD8_2, sourceRectangle2, color44, projectile.rotation, texture2D19.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            num223 -= (texture2D19.Height / 2 + texture2D21.Height) * projectile.scale;
            Vector2 value20 = projectile.Center;
            value20 += projectile.velocity * projectile.scale * texture2D19.Height / 2f;
            if (num223 > 0f)
            {
                float num224 = 0f;
                Rectangle rectangle7 = new Rectangle(0, 16 * (projectile.timeLeft / 3 % 5), texture2D20.Width, 16);
                while (num224 + 1f < num223)
                {
                    if (num223 - num224 < rectangle7.Height)
                    {
                        rectangle7.Height = (int)(num223 - num224);
                    }
                    Main.spriteBatch.Draw(texture2D20, value20 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle7), color44, projectile.rotation, new Vector2(rectangle7.Width / 2, 0f), projectile.scale, SpriteEffects.None, 0f);
                    num224 += rectangle7.Height * projectile.scale;
                    value20 += projectile.velocity * rectangle7.Height * projectile.scale;
                    rectangle7.Y += 16;
                    if (rectangle7.Y + rectangle7.Height > texture2D20.Height)
                    {
                        rectangle7.Y = 0;
                    }
                }
            }
            SpriteBatch arg_AE2D_0 = Main.spriteBatch;
            Texture2D arg_AE2D_1 = texture2D21;
            Vector2 arg_AE2D_2 = value20 - Main.screenPosition;
            sourceRectangle2 = null;
            arg_AE2D_0.Draw(arg_AE2D_1, arg_AE2D_2, sourceRectangle2, color44, projectile.rotation, texture2D21.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * projectile.localAI[1], projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float num6 = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 22f * projectile.scale, ref num6))
            {
                return true;
            }
            return false;
        }
    }

    public class NightclawerDeathraySmall : NightclawerDeathray
    {
        public override string Texture => "AAMod/NPCs/Bosses/Equinox/NightclawerDeathray";

        public override void SetDefaults()
        {
            base.SetDefaults();
            maxTime = 60;
            maxScale = 0.2f;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
                Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("NightclawerDeathray"), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);
            base.Kill(timeLeft);
        }
    }
}