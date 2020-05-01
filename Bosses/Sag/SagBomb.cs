
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Sag
{
    public class SagBomb : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 11;
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = false;
            projectile.tileCollide = true;
            aiType = 48;
            projectile.hostile = true;
            projectile.penetrate = 1;
        }

        public override void AI()
        {
            projectile.rotation += projectile.velocity.Length() * 0.025f;
            projectile.velocity.Y += .15f;
        }
        

        public override void Kill(int timeLeft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, 1, ModLoader.GetMod("AAMod").DustType("FulguriteDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("FulguriteDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default);
                Main.dust[num469].velocity *= 2f;
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y + 20, 0, 0, mod.ProjectileType("SagRing"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 10)
                    projectile.frame = 0;
            }

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 11, 0, 0);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 11, frame, lightColor, true);
            BaseDrawing.DrawTexture(sb, ModLoader.GetMod("AAMod").GetTexture("Glowmasks/SagBomb_Glow"), 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 11, frame, AAColor.ZeroShield, true);
            return false;
        }

        public static void DrawAfterimage(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, Vector2[] oldPoints, float scale = 1f, float rotation = 0f, int direction = 0, int framecount = 1, Rectangle frame = default, float distanceScalar = 1.0F, float sizeScalar = 1f, int imageCount = 7, bool useOldPos = true, float offsetX = 0f, float offsetY = 0f, bool drawCentered = false, Color? overrideColor = null)
        {
            Color lightColor = overrideColor != null ? (Color)overrideColor : BaseDrawing.GetLightColor(position + new Vector2(width * 0.5f, height * 0.5f));
            Vector2 velAddon = default;
            Vector2 originalpos = position;
            Vector2 offset = new Vector2(offsetX, offsetY);
            for (int m = 1; m <= imageCount; m++)
            {
                scale *= sizeScalar;
                Color newLightColor = lightColor;
                newLightColor.R = (byte)(newLightColor.R * (imageCount + 3 - m) / (imageCount + 9));
                newLightColor.G = (byte)(newLightColor.G * (imageCount + 3 - m) / (imageCount + 9));
                newLightColor.B = (byte)(newLightColor.B * (imageCount + 3 - m) / (imageCount + 9));
                newLightColor.A = (byte)(newLightColor.A * (imageCount + 3 - m) / (imageCount + 9));
                if (useOldPos)
                {
                    position = Vector2.Lerp(originalpos, m - 1 >= oldPoints.Length ? oldPoints[oldPoints.Length - 1] : oldPoints[m - 1], distanceScalar);
                    BaseDrawing.DrawTexture(sb, texture, shader, position + offset, width, height, scale, rotation, direction, framecount, frame, newLightColor, drawCentered ? true : false);
                }
                else
                {
                    Vector2 velocity = m - 1 >= oldPoints.Length ? oldPoints[oldPoints.Length - 1] : oldPoints[m - 1];
                    velAddon += velocity * distanceScalar;
                    BaseDrawing.DrawTexture(sb, texture, shader, position + offset - velAddon, width, height, scale, rotation, direction, framecount, frame, newLightColor, drawCentered ? true : false);
                }
            }
        }
    }
}