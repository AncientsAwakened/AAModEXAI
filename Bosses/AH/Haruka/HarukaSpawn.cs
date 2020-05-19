
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.AH.Haruka
{
    public class HarukaSpawn : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haruka Yamata");     
            Main.npcFrameCount[npc.type] = 4;     
        }

        public override void SetDefaults()
        {
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.width = 82;
            npc.height = 82;
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

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 10)
            {
                npc.frameCounter = 0;
                Frame++;
                if (Frame > 3)
                {
                    Frame = 0;
                }
            }
            npc.frame.Y = frameHeight * Frame;
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 4, npc.frame, dColor, true);
            return false;
        }
    }
}
