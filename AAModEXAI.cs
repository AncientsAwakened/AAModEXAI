using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Localization;

using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

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

        public override void PostSetupContent()
        {
            WeakReferences.PerformModSupport();
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
                if (Main.netMode == NetmodeID.SinglePlayer) { Main.NewText(s, colorR, colorG, colorB); }
                else
                if (Main.netMode == NetmodeID.MultiplayerClient) { Main.NewText(s, colorR, colorG, colorB); }
                else //if(sync){ NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color(colorR, colorG, colorB), Main.myPlayer); } }else
                if (sync && Main.netMode == NetmodeID.Server) { NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color(colorR, colorG, colorB), -1); }
            }
        }

        public static void ShowTitle(NPC npc, int ID)
        {
            if (AAConfigClient.Instance.AncientIntroText)
            {
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<AAMod.Title>(), 0, 0, Main.myPlayer, ID, 0);
            }
        }

        public static void ShowTitle(Player player, int ID)
        {
            if (AAConfigClient.Instance.AncientIntroText)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AAMod.Title>(), 0, 0, Main.myPlayer, ID, 0);
            }
        }

        public static void ShowSistersTitle(NPC npc)
        {
            if (AAConfigClient.Instance.AncientIntroText)
            {
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<AAMod.SistersTitle>(), 0, 0, Main.myPlayer, 16, 0);
            }
        }
	}
}