
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.FeudalFungus
{
    public class FungusIGoNow: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feudal Fungus");
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.damage = 24;
            projectile.width = 74;
            projectile.height = 80;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 900;
            projectile.alpha = 0;
        }
        public override void AI()
        {
            if (++projectile.frameCounter >= 14)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            projectile.velocity *= 0;
            projectile.alpha += 4;
            if (projectile.alpha >= 255)
            {
                projectile.active = false;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/FeudalFungusIGoNow_Glow");

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);

            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, projectile.GetAlpha(lightColor), true);
            BaseDrawing.DrawTexture(spriteBatch, glowTex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, projectile.GetAlpha(AAColor.Glow), true);
            return false;
        }
    }
}