
using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class BlockF1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.width = 208;
            projectile.height = 64;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.extraUpdates = 1;
            projectile.tileCollide = false;
        }

        public float[] internalAI = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
            }
        }

        public override void AI()
        {
            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
            if (projectile.frame < 5)
            {
                if (projectile.frameCounter++ > 3)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                }
            }
            else
            {
                projectile.frame = 5;
            }

            if (internalAI[0]++ > 60)
            {
                if (projectile.ai[0] == 0)
                {
                    if (projectile.velocity.Y < 116)
                    {
                        projectile.velocity.Y += .1f;
                    }
                }
                else if (projectile.ai[0] == 1)
                {
                    if (projectile.velocity.Y > -16)
                    {
                        projectile.velocity.Y -= .1f;
                    }
                    projectile.direction = projectile.spriteDirection = -1;
                }

                Projectile clearCheck = Main.projectile[(int)projectile.ai[1]];
                if (Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    for (int m = 0; m < 80; m++)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, 32, 0f, 0f, 100, new Color(32, 32, 46), 1.6f);
                    }
                    clearCheck.Kill();
                    projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item62, (int)projectile.position.X, (int)projectile.position.Y);
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 6, 0, 0);

            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile, 2f, 1f, Math.Abs((int)projectile.velocity.Y), true, 0f, 0f, Color.LightGreen, frame, 6);

            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 6, frame, dColor, true);
            return false;
        }
    }
}