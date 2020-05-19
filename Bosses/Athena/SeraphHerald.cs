
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Athena
{
    public class SeraphHerald : ModNPC
	{
        public override string Texture => "AAModEXAI/Bosses/Athena/SeraphA";

        public override void SetDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            npc.width = 60;
            npc.height = 40;
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
        }

        int pos;
        public override bool PreAI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];

            if (player.Center.X > npc.Center.X)
            {
                pos = 250;

                npc.direction = 1;
            }
            else
            {
                pos = -250;

                npc.direction = -1;
            }

            Vector2 wantedVelocity = player.Center - new Vector2(pos, 200);
            MoveToPoint(wantedVelocity);

            if (Main.netMode != NetmodeID.Server)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 6)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += Main.npcTexture[npc.type].Height / 4;
                }
                if (npc.frame.Y > Main.npcTexture[npc.type].Height / 4 * 3)
                {
                    npc.frame.Y = 0;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Vector2.Distance(player.Center, npc.Center) > 2200)
                {
                    npc.position = new Vector2(pos, 200);
                    for (int i = 0; i < 5; i++)
                    {
                        Dust d = Main.dust[Dust.NewDust(npc.position, npc.height, npc.width, ModLoader.GetMod("AAMod").DustType("Feather"), Main.rand.Next(-1, 2), 1, 0)];
                        d.position = npc.Center;
                    }
                }
                npc.ai[0]++;
                npc.alpha -= 15;

                if (npc.ai[0] == 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald1"), Color.CadetBlue);
                    npc.netUpdate = true;
                }
                else
                if (npc.ai[0] == 120)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald2"), Color.CadetBlue);
                }
                else
                if (npc.ai[0] == 240)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald3"), Color.CadetBlue);
                }
                else
                if (npc.ai[0] == 360)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald4"), Color.CadetBlue);
                }
                if (!AAWorld.downedGreed)
                {
                    if (npc.ai[0] >= 480)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald5"), Color.CadetBlue);

                        for (int i = 0; i < 5; i++)
                        {
                            Dust.NewDust(npc.position, npc.height, npc.width, ModLoader.GetMod("AAMod").DustType("Feather"), Main.rand.Next(-1, 2), 1, 0);
                        }

                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    if (npc.ai[0] == 480)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald6"), Color.CadetBlue);
                    }

                    if (npc.ai[0] >= 600)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SeraphHerald5"), Color.CadetBlue);

                        for (int i = 0; i < 5; i++)
                        {
                            Dust.NewDust(npc.position, npc.height, npc.width, ModLoader.GetMod("AAMod").DustType("Feather"), Main.rand.Next(-1, 2), 1, 0);
                        }

                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity.X > 0f)
            {
                npc.spriteDirection = 1;
            }
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = -1;
            }
            npc.rotation = npc.velocity.X * 0.1f;
            if (npc.type == 210 || npc.type == 211)
            {
                npc.frameCounter += 1.0;
                npc.rotation = npc.velocity.X * 0.2f;
            }
            npc.frameCounter += 1.0;
            if (npc.frameCounter >= 6.0)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
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
    }
}