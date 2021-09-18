
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Localization;
using AAModEXAI.Bosses.AH.Ashe;
using AAModEXAI.Bosses.AH.Haruka;

namespace AAModEXAI.Bosses.AH
{
    public class AHSpawn : ModNPC
    {
        public override string Texture => "AAModEXAI/BlankTex";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sisters of Discord");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.width = 100;
            npc.height = 100;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }
        public bool ATransitionActive = false;
        public int RVal = 255;
        public int BVal = 0;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(RVal, 125, BVal);
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            npc.ai[1]++;

            npc.Center = player.Center;

            if (npc.ai[1] == 60)          //if the timer has gotten to 7.5 seconds, this happens (60 = 1 second)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHSpawn1"), new Color(102, 20, 48));
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ChaosSissy");
            }

            if (npc.ai[1] == 300)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHSpawn2"), new Color(102, 20, 48));
            }

            if (npc.ai[1] == 500)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHSpawn3"), new Color(72, 78, 117));
            }

            if (npc.ai[1] == 550)
            {
                NPC.NewNPC((int)npc.position.X - 200, (int)npc.position.Y - 150, ModContent.NPCType<AsheSpawn>(), 0, npc.whoAmI);
            }

            if (npc.ai[1] == 700)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHSpawn4"), new Color(102, 20, 48));
            }

            if (npc.ai[1] == 550)
            {
                NPC.NewNPC((int)npc.position.X + 200, (int)npc.position.Y - 150, ModContent.NPCType<HarukaSpawn>(), 0, npc.whoAmI);
            }

            if (npc.ai[1] == 820)
            {
                Main.npc[BaseAI.GetNPC(npc.Center, ModContent.NPCType<AsheSpawn>(), -1)].Transform(ModContent.NPCType<Ashe.Ashe>());
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHSpawn5"), new Color(102, 20, 48));
                SpawnBoss(player, "Ashe");
            }

            if (npc.ai[1] >= 960)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHSpawn6"), new Color(72, 78, 117));
                Main.npc[BaseAI.GetNPC(npc.Center, ModContent.NPCType<HarukaSpawn>(), -1)].Transform(ModContent.NPCType<Haruka.Haruka>());
                SpawnBoss2(player, "Haruka");
                npc.active = false;
            }
        }

        public void SpawnBoss(Player player, string name)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int bossType = mod.NPCType(name);
                if (NPC.AnyNPCs(bossType)) { return; } //don't spawn if there's already a boss!
                int npcID = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, bossType, 0);
                Main.npc[npcID].Center = player.Center - new Vector2(MathHelper.Lerp(-100f, 100f, (float)Main.rand.NextDouble()), 800f);
                Main.npc[npcID].netUpdate2 = true; Main.npc[npcID].netUpdate = true;
            }
        }

        public void SpawnBoss2(Player player, string name)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int bossType = mod.NPCType(name);
                if (NPC.AnyNPCs(bossType)) { return; } //don't spawn if there's already a boss!
                int npcID = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, bossType, 0);
                Main.npc[npcID].Center = player.Center - new Vector2(800f, 0);
                Main.npc[npcID].netUpdate2 = true; Main.npc[npcID].netUpdate = true;
            }
        }
    }
}