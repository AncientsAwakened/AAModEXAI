
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Anubis
{
    public class Uraeus : ModNPC
	{
		public override void SetDefaults()
		{
			npc.damage = 40;
			npc.npcSlots = 5f;
            npc.damage = 45;
            npc.width = 20;
            npc.height = 20;
            npc.defense = 20;
            npc.lifeMax = 500;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            animationType = 10;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
            npc.value = Item.sellPrice(0, 0, 0, 0);
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
			AAAI.AIWorm(npc, new int[]{ mod.NPCType("Uraeus"), mod.NPCType("UraeusBody"), mod.NPCType("UraeusTail") }, 7, 0f, 10f, 0.07f, true, false, true, true, true);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin, hitDirection, -1f, 0);
            }
            if (npc.life == 0)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin, hitDirection, -1f, 0);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc, drawColor, true);
            return false;
        }
    }
}