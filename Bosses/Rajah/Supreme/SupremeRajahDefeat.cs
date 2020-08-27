using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Rajah.Supreme
{
    [AutoloadBossHead]
    public class SupremeRajahDefeat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Rabbit");
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetDefaults()
        {
            npc.width = 130;
            npc.height = 220;
            npc.aiStyle = -1;
            npc.damage = 0;
            npc.defense = 90;
            npc.lifeMax = 50000;
            npc.knockBackResist = 0f;
            npc.npcSlots = 1000f;
            npc.dontTakeDamage = true;
            npc.boss = true;
            npc.netAlways = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
            npc.noTileCollide = false;
        }

        public override void AI()
        {
            if (npc.velocity.Y == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[0]++;
            }

            if (npc.ai[0] == 120)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat1"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 240)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat2"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 360)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat3"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 480)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat4"), 107, 137, 179, true);
            }
            if (npc.ai[0] >= 600)
            {
                npc.ai[1] = 1;
                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ThinkAboutIt");
                npc.netUpdate = true;
            }
            if (npc.ai[0] == 600)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("...", 107, 137, 179, true);
            }
            if (npc.ai[0] >= 840)
            {
                npc.ai[1] = 2;
                npc.netUpdate = true;
            }
            if (npc.ai[0] == 840)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat5"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 960)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat6"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 1080)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat7"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 1200)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat8"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 1380)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat9"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 1540)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat10"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 1660)
            {
                string Name;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    Name = "Terrarians";
                }
                else
                {
                    Name = Main.LocalPlayer.name;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat11") + Name + "?", 107, 137, 179, true);
            }
            if (npc.ai[0] == 1780)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat12"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 1900)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat13"), 107, 137, 179, true);
            }
            if (npc.ai[0] == 2020)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat14"), 107, 137, 179, true);
            }
            if (npc.ai[0] >= 2180)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat15"), 107, 137, 179, true);
                AAModEXAIWorld.downedRajahsRevenge = true;
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahDefeat16"), Color.Green, true);
                int p = Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("SupremeRajahLeave"), 0, 0, Main.myPlayer);
                Main.projectile[p].position = npc.position;
                npc.active = false;
                npc.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] == 0)
            {
                if (npc.frameCounter++ > 15)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += frameHeight;
                }
                if (npc.frame.Y > frameHeight * 3)
                {
                    npc.frame.Y = 0;
                }
            }
            else if (npc.ai[1] == 1)
            {
                npc.frame.Y = frameHeight * 4;
            }
            else if (npc.ai[1] == 2)
            {
                if (npc.frameCounter++ > 15)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += frameHeight;
                }
                if (npc.frame.Y > frameHeight * 8 || npc.frame.Y < frameHeight * 5)
                {
                    npc.frame.Y = frameHeight * 5;
                }
            }
        }
    }
}