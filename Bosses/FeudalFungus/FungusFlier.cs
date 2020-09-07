using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.FeudalFungus
{
    public class FungusFlier : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fungus Flier");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 14;
            npc.height = 14;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 5;
            npc.defense = 0;
            npc.damage = 20;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = null;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
        }

        public override void AI()
        {
            Player player = Main.player[npc.target]; // makes it so you can reference the player the npc is targetting

            BaseAI.AIFloater(npc, ref npc.ai, false, 0.2f, 2f, 1.5f, 0.04f, 1.5f, 3);

            if (npc.wet)
            {
                npc.life = 0;
            }

            npc.frameCounter++;
            if (npc.frameCounter > 8)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 20;
                if (npc.frame.Y > 40)
                {
                    npc.frame.Y = 0;
                }
            }
        }
    }
}


