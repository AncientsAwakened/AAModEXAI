
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Anubis
{
    public class UraeusTail : Uraeus
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Uraeus");
        }

		public override void SetDefaults()
		{
            base.SetDefaults();
            npc.dontCountMe = true;
		}

		public override bool PreNPCLoot()
		{
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc, drawColor, true);
            return false;
        }
    }
}