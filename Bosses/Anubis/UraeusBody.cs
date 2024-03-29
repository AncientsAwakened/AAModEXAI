﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace AAModEXAI.Bosses.Anubis
{
    public class UraeusBody : Uraeus
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