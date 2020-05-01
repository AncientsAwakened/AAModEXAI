using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class GlitchBomb : ModProjectile
    {
        public float maxDistToAttack = 350f;
        public int target = -1;
        public float maxSpeed = 12f;
        public int targetTick = 8;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.hostile = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 240;
            projectile.tileCollide = true;
            projectile.aiStyle = 0;
            projectile.scale *= 1.5f;
            projectile.damage = 100;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.Oblivion;
        }

        public override void AI()
        {
            int num103 = Player.FindClosest(projectile.Center, 1, 1);
            projectile.ai[1] += 1f;
            if (projectile.ai[1] < 110f && projectile.ai[1] > 30f)
            {
                float scaleFactor2 = projectile.velocity.Length();
                Vector2 vector11 = Main.player[num103].Center - projectile.Center;
                vector11.Normalize();
                vector11 *= scaleFactor2;
                projectile.velocity = ((projectile.velocity * 24f) + vector11) / 25f;
                projectile.velocity.Normalize();
                projectile.velocity *= scaleFactor2;
            }
            if (projectile.ai[0] < 0f)
            {
                if (projectile.velocity.Length() < 18f)
                {
                    projectile.velocity *= 1.02f;
                }
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 0f / 255f, (255 - projectile.alpha) * 0.15f / 255f);

            int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height + 10, ModLoader.GetMod("AAMod").DustType("VoidDust"), projectile.velocity.X * 0.2f,
					projectile.velocity.Y * 0.2f, 100);
				Main.dust[dustId].noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 2)
            {
                for (int m = 0; m < 6; m++)
                {
                    int dustID = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("VoidDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                }
                Main.PlaySound(4, (int)projectile.Center.X, (int)projectile.Center.Y, 3);
            }

            Main.PlaySound(ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Glitch"), (int)projectile.Center.X, (int)projectile.Center.Y);
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("GlitchBoom"), projectile.damage, 1, projectile.owner);
        }
        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 2) 
                    projectile.frame = 0; 
            }
            return true;
        }
    }
}