using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Localization;

using AAMod;

namespace AAModEXAI
{
	public class AAModEXAI : Mod
	{
		public static Mod Instance;
		internal static AAModEXAI instance;
		public AAModEXAI()
		{
			Instance = this;
			instance = this;
		}

		public override void Unload()
        {
			Instance = null;
			instance = null;
		}

		public static void Chat(string s, Color color, bool sync = true)
        {
            Chat(s, color.R, color.G, color.B, sync);
        }

        /*
         * Sends the given string to chat, with the given color values.
         */
        public static void Chat(string s, byte colorR = 255, byte colorG = 255, byte colorB = 255, bool sync = true)
        {
            if (!AAConfigClient.Instance.NoBossDialogue)
            {
                if (Main.netMode == 0) { Main.NewText(s, colorR, colorG, colorB); }
                else
                if (Main.netMode == 1) { Main.NewText(s, colorR, colorG, colorB); }
                else //if(sync){ NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color(colorR, colorG, colorB), Main.myPlayer); } }else
                if (sync && Main.netMode == 2) { NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color(colorR, colorG, colorB), -1); }
            }
        }
	}
}