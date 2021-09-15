using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.ZeroMinion
{
    public class ZeroTeslaTurrent: ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero Tesla Turrent");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
            Main.npcFrameCount[npc.type] = 2; 
        }

        public override void SetDefaults()
        {
            npc.alpha = 255;
            npc.width = 30;
            npc.height = 50;
            npc.damage = 55;
            npc.defense = 90;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCHit4;
            npc.lifeMax = 10000;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0.0f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
            npc.lavaImmune = true;
            npc.netAlways = true;
            npc.knockBackResist = 0;
            npc.noGravity = true;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if(npc.alpha > 0)
            {
                npc.alpha -= 5;
            }
            else
            {
                npc.alpha = 0;
            }
            npc.TargetClosest(false);
		    npc.spriteDirection = npc.direction;
            npc.Center = new Vector2(npc.ai[1], npc.ai[2]);

            Vector2 vector128 = npc.Center - Vector2.UnitY * 10f;

            if (npc.ai[0] < 60f)
            {
                npc.ai[0] += 1f;
            }
            if (npc.ai[0] == 60f)
            {
                npc.ai[0] = -120f;
                Vector2 center3 = Main.player[npc.target].Center;
                Vector2 vector129 = center3 - vector128;
                vector129.X += (float)Main.rand.Next(-100, 101);
                vector129.Y += (float)Main.rand.Next(-100, 101);
                vector129.X *= (float)Main.rand.Next(70, 131) * 0.01f;
                vector129.Y *= (float)Main.rand.Next(70, 131) * 0.01f;
                vector129.Normalize();
                if (float.IsNaN(vector129.X) || float.IsNaN(vector129.Y))
                {
                    vector129 = -Vector2.UnitY;
                }
                vector129 *= 14f;
                Projectile.NewProjectile(vector128.X, vector128.Y, vector129.X, vector129.Y, mod.ProjectileType("Static"), npc.damage, 0f, Main.myPlayer, 0f, 0f);
            }

            return;
        }
        private int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[0] > 0f)
            {
                npc.frameCounter += 1.0;
                if (npc.ai[0] >= 20f)
                {
                    npc.frameCounter += 1.0;
                }
                if (npc.ai[0] >= 40f)
                {
                    npc.frameCounter += 1.0;
                }
                if (npc.frameCounter >= 10.0)
                {
                    npc.frameCounter = 0.0;
                    if (Frame ++ >= 1)
                    {
                        Frame = 0;
                    }
                }
            }
            else
            {
                npc.frameCounter += 1.0;
                if (npc.frameCounter >= 15.0)
                {
                    npc.frameCounter = 0.0;
                    if (Frame ++ >= 1)
                    {
                        Frame = 0;
                    }
                }
            }

            npc.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            Texture2D[] Number = new Texture2D[2];
            Number[0] = mod.GetTexture("Bosses/Zero/ZeroMinion/zero");
            Number[1] = mod.GetTexture("Bosses/Zero/ZeroMinion/one");
            if(Main.rand.Next(3) == 0)
            {
                Color Alpha = AAColor.Oblivion;
                int n = Main.rand.Next(2);
                BaseDrawing.DrawTexture(sb, Number[n], 0, npc.Center + new Vector2( - Main.rand.NextFloat(-15f, 15f), - Main.rand.NextFloat(-25f, 25f)), Number[n].Width, Number[n].Height, 1f, 0f, 0, 1, new Rectangle(0, 0, Number[n].Width, Number[n].Height), Alpha, true);
            }
            return true;
        }
    }
}