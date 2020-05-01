using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.AH
{
    public class AHDeath : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sisters Defeat");
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
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
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ChaosSissy");

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

            if (AAConfigClient.Instance.NoBossDialogue)
            {
                AAWorld.downedSisters = true;
                npc.active = false;
            }

            npc.Center = player.Center;

            if (npc.ai[1] == 100)          //if the timer has gotten to 7.5 seconds, this happens (60 = 1 second)
            {
                if (AAWorld.downedSisters)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath1"), new Color(102, 20, 48));
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath2"), new Color(72, 78, 117));
                }
            }

            if (npc.ai[1] == 300)
            {
                if (AAWorld.downedSisters)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath3"), new Color(72, 78, 117));
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath4") + (player.Male ? AAMod.Lang.BossChat("male") : AAMod.Lang.BossChat("fimale")) + AAMod.Lang.BossChat("AHDeath5"), new Color(102, 20, 48));
                }
            }

            if (npc.ai[1] == 500)
            {
                if (AAWorld.downedSisters)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath6"), new Color(102, 20, 48));
                    npc.active = false;
                    AAWorld.downedSisters = true;
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath7"), new Color(72, 78, 117));
                }
            }
            
            if (npc.ai[1] == 700)
            {
                if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHDeath8"), new Color(102, 20, 48));
                AAWorld.downedSisters = true;
                npc.active = false;
            }
        }
    }
}