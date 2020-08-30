
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Sag
{
    public class SagShot : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.damage = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.aiStyle = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.ai[0] += 0.1f;
            projectile.velocity *= 0.75f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

		public bool spawnSound = false;
        public override void AI()
        {
			if(Main.netMode != NetmodeID.Server && !spawnSound)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 33);				
				spawnSound = true;
			}
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

            if (Main.rand.Next(1) == 0)
            {
                int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("VoidDust"), 0f, 0f, 200, default, 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }
        
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Violet;
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5) 
            {
                projectile.frame++; 
                projectile.frameCounter = 0;
                if (projectile.frame > 4)
                {
                    projectile.frame = 0;
                }
            }
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 5, 0, 0);
            SagiStar.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.oldPos, projectile.scale, projectile.rotation, projectile.direction, 5, frame, 1.2f, 1f, 4, true, 0, 0, true, Color.White);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 5, frame, Color.White, true);
            return false;
        }
    }
}