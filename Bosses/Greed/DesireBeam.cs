using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Greed
{
    public class DesireBeam : ModProjectile // Thanks to Dan Yami for the code
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desire Beam");
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 3600;
            projectile.tileCollide = false;
        }
        internal const float charge = 40f;
        public float LaserLength { get { return projectile.localAI[1]; } set { projectile.localAI[1] = value; } }
        public const float LaserLengthMax = 2000f;
        int multiplier = 1;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }
        float attackCounter = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadSingle();
        }
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                if (projectile.ai[1] == 0)
                    Main.PlaySound(SoundID.Item28, projectile.position);
                projectile.ai[1] = 5;
            }
            else if (projectile.ai[0] >= 20)
                projectile.ai[1] += 5f * multiplier;
            projectile.ai[0]++;
            if (projectile.ai[1] == charge)
            {
                projectile.hostile = true;
            }
            if (projectile.ai[1] >= charge + 60f && multiplier == 1)
            {
                multiplier = -1;
            }
            if (multiplier == -1 && projectile.ai[1] <= 0)
                projectile.Kill();

            projectile.rotation = projectile.velocity.ToRotation() - 1.57079637f;
            projectile.velocity = Vector2.Normalize(projectile.velocity);

            float[] sampleArray = new float[2];
            Collision.LaserScan(projectile.Center, projectile.velocity, 0, LaserLengthMax, sampleArray);
            float sampledLength = 0f;
            for (int i = 0; i < sampleArray.Length; i++)
            {
                sampledLength += sampleArray[i];
            }
            sampledLength /= sampleArray.Length;
            float amount = 0.75f; // last prism is 0.75 rather than 0.5?
            LaserLength = MathHelper.Lerp(LaserLength, sampledLength, amount);
            LaserLength = LaserLengthMax;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * LaserLength, projHitbox.Width, ref collisionPoint);
        }
        public override bool? CanCutTiles()
        {
            DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * LaserLength, projectile.width * projectile.scale * 2, new Utils.PerLinePoint(CutTilesAndBreakWalls));
            return true;
        }

        private bool CutTilesAndBreakWalls(int x, int y)
        {
            return DelegateMethods.CutTiles(x, y);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            Texture2D texture2D19 = Main.projectileTexture[projectile.type];
            Texture2D texture2D20 = mod.GetTexture("Bosses/Greed/DesireBeam_Beam");
            Texture2D texture2D21 = mod.GetTexture("Bosses/Greed/DesireBeam_End");
            float num228 = LaserLength;
            Color color44 = Color.White * 0.8f;
            Texture2D arg_AF99_1 = texture2D19;
            Vector2 arg_AF99_2 = projectile.Center + new Vector2(0, projectile.gfxOffY) - Main.screenPosition;
            Rectangle? sourceRectangle2 = null;
            spriteBatch.Draw(arg_AF99_1, arg_AF99_2, sourceRectangle2, color44, projectile.rotation, texture2D19.Size() / 2f, new Vector2(Math.Min(projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
            num228 -= (texture2D19.Height / 2 + texture2D21.Height) * projectile.scale;
            Vector2 value20 = projectile.Center + new Vector2(0, projectile.gfxOffY);
            value20 += projectile.velocity * projectile.scale * texture2D19.Height / 2f;
            if (num228 > 0f)
            {
                float num229 = 0f;
                Rectangle rectangle7 = new Rectangle(0, 16 * (projectile.timeLeft / 3 % 5), texture2D20.Width, 16);
                while (num229 + 1f < num228)
                {
                    if (num228 - num229 < rectangle7.Height)
                    {
                        rectangle7.Height = (int)(num228 - num229);
                    }
                    Main.spriteBatch.Draw(texture2D20, value20 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle7), color44, projectile.rotation, new Vector2(rectangle7.Width / 2, 0f), new Vector2(Math.Min(projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
                    num229 += rectangle7.Height * projectile.scale;
                    value20 += projectile.velocity * rectangle7.Height * projectile.scale;
                    rectangle7.Y += 16;
                    if (rectangle7.Y + rectangle7.Height > texture2D20.Height)
                    {
                        rectangle7.Y = 0;
                    }
                }
            }
            SpriteBatch arg_B1FF_0 = Main.spriteBatch;
            Texture2D arg_B1FF_1 = texture2D21;
            Vector2 arg_B1FF_2 = value20 - Main.screenPosition;
            sourceRectangle2 = null;
            arg_B1FF_0.Draw(arg_B1FF_1, arg_B1FF_2, sourceRectangle2, color44, projectile.rotation, texture2D21.Frame(1, 1, 0, 0).Top(), new Vector2(Math.Min(projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
            return false;
        }
    }
}