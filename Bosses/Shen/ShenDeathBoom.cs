using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Shen
{
    public class ShenDeathBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Strike");     
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = -1;
            projectile.damage = 0;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
            projectile.alpha = 80;
        }

        bool draw = true;
        public override void AI()
        {
            if (!draw)
            {
                draw = true;
            }
            else
            {
                draw = false;
            }
            
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 7)
                {
                    projectile.Kill();

                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y *= 0.00f;

        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!draw)
            {
                return false;
            }
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 7, 0, 2);

            Texture2D Tex = Main.projectileTexture[projectile.type];
            if (projectile.ai[0] == 1)
            {
                Tex = mod.GetTexture("Bosses/Shen/ShenDeathBoomR");
            }
            else
            if (projectile.ai[0] == 1)
            {
                Tex = mod.GetTexture("Bosses/Shen/ShenDeathBoomB");
            }
            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 7, frame, projectile.GetAlpha(Color.White), true);
            return false;
        }
    }
}
