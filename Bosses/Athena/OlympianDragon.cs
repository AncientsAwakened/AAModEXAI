
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAModEXAI.Bosses.Athena.Olympian;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Athena
{
    public class OlympianDragon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Olympian Dragon");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 38;
            npc.aiStyle = 0;
            npc.damage = 30;
            npc.defense = 30;
            npc.lifeMax = 150;
            npc.HitSound = SoundID.DD2_WyvernHurt;
            npc.DeathSound = SoundID.DD2_WyvernDeath;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0.05f;
            npc.npcSlots = 0f;
            npc.lavaImmune = true;
            npc.netAlways = true;
            npc.alpha = 255;
        }

        public override bool PreAI()
        {
            if (!NPC.AnyNPCs(mod.NPCType("Athena")) && !NPC.AnyNPCs(mod.NPCType("AthenaA")))
            {
                npc.velocity *= .95f;
                if (npc.alpha != 0)
                {
                    for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                    {
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AbyssDust"), 0f, 0f, 100);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].noLight = false;
                    }
                }
                npc.alpha += 3;
                if (npc.alpha >= 255 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.active = false;
                }
                return false;
            }
            return true;
        }

        public override void AI()
        {
            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AbyssDust"), 0f, 0f, 100);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = false;
                }
            }
            npc.alpha -= 3;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            BaseAI.AIFlier(npc, ref npc.ai, true, 0.15f, 0.08f, 6f, 5f, false, 300);
            Player player = Main.player[npc.target];
            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y > (frameHeight * 3))
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe || !Main.hardMode)
            {
                return 0f;
            }
            return SpawnCondition.Sky.Chance * 0.10f;
        }

    }
}