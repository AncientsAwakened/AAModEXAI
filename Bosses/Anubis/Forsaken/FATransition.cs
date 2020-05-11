using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class FATransition : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anubis Legendscribe");
            Main.npcFrameCount[npc.type] = 15;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.noGravity = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.value = 0;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        public override void AI()
        {
            npc.dontTakeDamage = true;

            npc.ai[3] = 39;
            if (Main.netMode != 1)
            {
                if (npc.velocity.Y == 0)
                {
                    npc.ai[1]++;
                    if (npc.ai[1] == 120)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition1"), Color.Gold);
                        music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AnubisA");
                    }

                    if (npc.ai[1] == 240)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition2"), Color.Gold);
                    }

                    if (npc.ai[1] == 360)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition3"), Color.Gold);
                    }

                    if (npc.ai[1] == 480)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition4"), Color.Gold);
                    }

                    if (npc.ai[1] == 600)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition5"), Color.Gold);
                    }

                    if (npc.ai[1] == 720)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition6"), Color.Gold);
                    }

                    if (npc.ai[1] == 840)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AnubisTransition7"), Color.ForestGreen);
                    }

                    if (npc.ai[1] >= 900)
                    {
                        int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModLoader.GetMod("AAMod").ProjectileType("ShockwaveBoom"), 0, 0, Main.myPlayer, 0, 10);
                        Main.projectile[b].Center = npc.Center;

                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("ForsakenAnubis"));
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] < 540)
            {
                npc.frameCounter++;
                if (npc.frameCounter > 6)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += frameHeight;
                }
                if (npc.frame.Y > frameHeight * 4)
                {
                    npc.frame.Y = 0;
                }
            }
            if (npc.ai[1] == 540)
            {
                npc.frame.Y = frameHeight * 5;
            }
            if (npc.ai[1] == 730)
            {
                npc.frame.Y = frameHeight * 6;
            }
            if (npc.ai[1] == 740)
            {
                npc.frame.Y = frameHeight * 7;
            }
            if (npc.ai[1] == 750)
            {
                npc.frame.Y = frameHeight * 8;
            }
            if (npc.ai[1] == 760)
            {
                npc.frame.Y = frameHeight * 9;
            }
            if (npc.ai[1] == 770)
            {
                npc.frame.Y = frameHeight * 10;
            }
            if (npc.ai[1] == 780)
            {
                npc.frame.Y = frameHeight * 11;
            }
            if (npc.ai[1] == 790)
            {
                npc.frame.Y = frameHeight * 12;
            }
            if (npc.ai[1] == 800)
            {
                npc.frame.Y = frameHeight * 13;
            }
            if (npc.ai[1] >= 840)
            {
                npc.frame.Y = frameHeight * 14;
            }
        }
    }
}