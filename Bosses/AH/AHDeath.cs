using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Localization;

namespace AAModEXAI.Bosses.AH
{
    public class AHDeath : ModNPC
    {
        public override string Texture => "AAModEXAI/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sisters Defeat");
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
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ChaosSissy");

            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }
        public override void AI()
        {
            npc.ai[1]++;
            npc.TargetClosest();
            Player player = Main.player[npc.target];

            npc.Center = player.Center;

            if (npc.ai[1] == 100)          //if the timer has gotten to 7.5 seconds, this happens (60 = 1 second)
            {
                if (AAModEXAIWorld.downedSisters)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath1"), new Color(102, 20, 48));
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath2"), new Color(72, 78, 117));
                }
            }

            if (npc.ai[1] == 300)
            {
                if (AAModEXAIWorld.downedSisters)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath3"), new Color(72, 78, 117));
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath4") + (player.Male ? Trans.text("Common", "male") : Trans.text("Common", "fimale")) + Trans.text("AH", "AHDeath5"), new Color(102, 20, 48));
                }
            }

            if (npc.ai[1] == 500)
            {
                if (AAModEXAIWorld.downedSisters)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath6"), new Color(102, 20, 48));
                    npc.active = false;
                    AAModEXAIWorld.downedSisters = true;
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath7"), new Color(72, 78, 117));
                }
            }
            
            if (npc.ai[1] == 700)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("AH", "AHDeath8"), new Color(102, 20, 48));
                AAModEXAIWorld.downedSisters = true;
                npc.active = false;
            }
        }
    }
}