using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Toad
{
    public class GlowshroomGrow : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Mushroom");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 40;
            npc.aiStyle = -1;
            npc.damage = 30;
            npc.defense = 40;
            npc.lifeMax = 200;
            npc.knockBackResist = 0f;
            npc.npcSlots = 0f;
            npc.aiStyle = -1;
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            npc.noTileCollide = false;
        }

        public override void AI()
        {
            if (Main.netMode != 1)
            {
                npc.ai[0]++;
            }
            if (npc.ai[0] < 600)
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 4;
                }
                else
                {
                    npc.alpha = 0;
                }
            }
            else
            {
                if (npc.alpha < 255)
                {
                    npc.alpha += 5;
                }
                else
                {
                    npc.active = false;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 5)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y > frameHeight * 4)
            {
                npc.frame.Y = frameHeight * 4;
            }
        }

        public override bool PreNPCLoot()
        {
            return false;
        }
    }
}