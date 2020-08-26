using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using ReLogic.Utilities;
using Microsoft.Xna.Framework.Graphics;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Sag
{
    public class SagRing : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            Main.projFrames[projectile.type] = 4;
            projectile.timeLeft = 500;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
        {
            Lighting.AddLight(projectile.Center, .7f, 0f, 0f);
            ActiveSound activeSound = Main.GetActiveSound(SlotId.FromFloat(projectile.localAI[0]));
            if (activeSound != null)
            {
                if (activeSound.Volume == 0f)
                {
                    activeSound.Stop();
                    projectile.localAI[0] = SlotId.Invalid.ToFloat();
                }
                activeSound.Volume = Math.Max(0f, activeSound.Volume - 0.05f);
            }
            else
            {
                projectile.localAI[0] = SlotId.Invalid.ToFloat();
            }
            if (projectile.ai[1] == 1f)
            {
                if (projectile.alpha < 255)
                {
                    projectile.alpha += 51;
                }
                if (projectile.alpha >= 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                    return;
                }
            }
            else
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 50;
                }
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
            float num726 = 30f;
            float num727 = num726 * 4f;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > num727)
            {
                projectile.ai[0] = 0f;
            }
            Vector2 vector62 = -Vector2.UnitY.RotatedBy(6.28318548f * projectile.ai[0] / num726, default);
            float val = 0.75f + vector62.Y * 0.25f;
            float val2 = 0.8f - vector62.Y * 0.2f;
            float num728 = Math.Max(val, val2);
            projectile.position += new Vector2(projectile.width, projectile.height) / 2f;
            projectile.width = (projectile.height = (int)(80f * num728));
            projectile.position -= new Vector2(projectile.width, projectile.height) / 2f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            for (int num729 = 0; num729 < 1; num729++)
            {
                float num730 = 55f * num728;
                float num731 = 11f * num728;
                float num732 = 0.5f;
                int num733 = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("VoidDust"), 0f, 0f, 100, default, 0.5f);
                Main.dust[num733].noGravity = true;
                Main.dust[num733].velocity *= 2f;
                Main.dust[num733].position = ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + projectile.Center;
                Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - projectile.Center);
                if (Main.rand.Next(2) == 0)
                {
                    num733 = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("VoidDust"), 0f, 0f, 100, default, 0.9f);
                    Main.dust[num733].noGravity = true;
                    Main.dust[num733].velocity *= 1.2f;
                    Main.dust[num733].position = ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + projectile.Center;
                    Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - projectile.Center);
                }
                if (Main.rand.Next(4) == 0)
                {
                    num733 = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("VoidDust"), 0f, 0f, 100, default, 0.7f);
                    Main.dust[num733].noGravity = true;
                    Main.dust[num733].velocity *= 1.2f;
                    Main.dust[num733].position = ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + projectile.Center;
                    Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - projectile.Center);
                }
            }
            return;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color25 = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
            Texture2D texture2D27 = Main.projectileTexture[projectile.type];
            float num242 = 30f;
            float num243 = num242 * 4f;
            float num244 = 6.28318548f * projectile.ai[0] / num242;
            float num245 = 6.28318548f * projectile.ai[0] / num243;
            Vector2 vector33 = -Vector2.UnitY.RotatedBy(num244, default);
            float scale6 = 0.75f + vector33.Y * 0.25f;
            float scale7 = 0.8f - vector33.Y * 0.2f;
            int num246 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int y10 = num246 * projectile.frame;
            Vector2 position15 = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            SpriteEffects spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture2D27, position15, new Rectangle?(new Rectangle(0, y10, texture2D27.Width, num246)), projectile.GetAlpha(color25), projectile.rotation + num245, new Vector2(texture2D27.Width / 2f, num246 / 2f), scale6, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture2D27, position15, new Rectangle?(new Rectangle(0, y10, texture2D27.Width, num246)), projectile.GetAlpha(color25), projectile.rotation + (6.28318548f - num245), new Vector2(texture2D27.Width / 2f, num246 / 2f), scale7, spriteEffects, 0f);
            return false;
        }
    }
}