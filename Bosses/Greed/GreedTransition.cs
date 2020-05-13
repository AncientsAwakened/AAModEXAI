
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Greed
{
    public class GreedTransition : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Greed/GreedSpawn";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spark of Greed");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 100;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            npc.alpha = 255;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        public override void AI()
        {
            npc.TargetClosest();

            Player player = Main.player[npc.target];
            MoveToPoint(player.Center - new Vector2(0, 300f));

            if (Main.netMode != 2) //clientside stuff
            {
                if (npc.ai[0] > 175)
                {
                    npc.alpha -= 3;
                    if (npc.alpha < 0)
                    {
                        npc.alpha = 0;
                    }
                }
            }

            if (Main.netMode != 1)
            {
                npc.ai[0]++;

                if (npc.ai[0] == 175)
                {
                    if (Main.netMode != 1)
                    {
                        BaseUtility.Chat(AAMod.Lang.BossChat("GreedTransition1"), Color.Goldenrod);
                    }
                    music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/GreedA");

                    npc.netUpdate = true;
                }
                else if (npc.ai[0] == 350)
                {
                    if (Main.netMode != 1)
                    {
                        BaseUtility.Chat(AAMod.Lang.BossChat("GreedTransition2"), Color.Goldenrod);
                    }
                }
                else if (npc.ai[0] == 500)
                {
                    if (Main.netMode != 1)
                    {
                        BaseUtility.Chat(AAMod.Lang.BossChat("GreedTransition3"), Color.Goldenrod);
                    }

                    npc.netUpdate = true;
                }
                else if (npc.ai[0] >= 610)
                {
                    AAModGlobalNPC.SpawnBoss(player, mod.NPCType("GreedA"), true, npc.Center, AAMod.Lang.BossChat("GreedAName"), false);

                    if (Main.netMode != 1)
                    {
                        BaseUtility.Chat(AAMod.Lang.BossChat("GreedTransition4"), Color.Goldenrod);
                    }

                    npc.netUpdate = true;
                    npc.active = false;
                }
            }
        }

        public void MoveToPoint(Vector2 point, bool goUpFirst = false)
        {
            float moveSpeed = 14f;
            if (moveSpeed == 0f || npc.Center == point) return; //don't move if you have no move speed
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override bool CheckActive()
        {
            if (!NPC.AnyNPCs(mod.NPCType("GreedA")))
            {
                return false;
            }

            npc.active = false;
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter >= 4)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= frameHeight * 3)
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle SunFrame = new Rectangle(0, 0, 70, 70);
            BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture("Bosses/Greed/GreedSpawn"), 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, 0, npc.spriteDirection, 4, SunFrame, npc.GetAlpha(AAColor.COLOR_WHITEFADE1), true);
            return false;
        }
    }
}