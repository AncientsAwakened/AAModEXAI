
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;using AAMod;

namespace AAModEXAI.Bosses.AH.Ashe
{
    internal class AsheMagicSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Bomb");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {

            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale = 1.1f;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
            projectile.velocity *= 1f + Math.Abs(projectile.ai[0]);

            Vector2 acceleration = projectile.velocity.RotatedBy(Math.PI / 2);
            acceleration *= projectile.ai[1];
            projectile.velocity += acceleration;

            if (projectile.timeLeft < 60)
            {
                projectile.alpha+= 5;
            }
            if (projectile.timeLeft == 0)
            {
                projectile.Kill();
            }

            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            if (projectile.frameCounter++ > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 600);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 4, frame, projectile.GetAlpha(Color.White), true);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
        }
    }
}