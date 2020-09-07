using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.MushroomMonarch
{
    public class RedMushling : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushling");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 50;
            npc.damage = 6;
            npc.defense = 5; 
            npc.knockBackResist = 1f;
            npc.value = Item.sellPrice(0, 0, 0, 0);
            npc.aiStyle = -1;
            npc.width = 30;
            npc.height = 44;
            npc.npcSlots = 0f;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            Player player = Main.player[npc.target]; // makes it so you can reference the player the npc is targetting

            BaseAI.AIZombie(npc, ref npc.ai, false, false, -1, .09f, 2, 3, 5, 120, true, 10, 10, true);

            if (npc.velocity.Y == 0)
            {
                npc.frameCounter++;
                if (npc.frameCounter > 8)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += 44;
                }
                if (npc.frame.Y > 44 * 6)
                {
                    npc.frame.Y = 0;
                }
            }
            else
            {
                npc.frame.Y = 44 * 6;
            }
        }
    }
}


