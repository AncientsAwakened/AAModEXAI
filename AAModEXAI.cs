using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;

using AAModEXAI.Loaders;
using AAModEXAI.Dusts;

namespace AAModEXAI
{
	public class AAModEXAI : Mod
	{
        [BypassAutoUnload] // Don't unload the instance automatically because it is needed until unloading finishes.
        public static AAModEXAI instance;

        [BypassAutoUnload]
        public static Type[] allTypesInAssembly;

        public static List<SoundEffectInstance> activeRumbleSounds;

		public override void Load()
        {
            Loader.Load();
		}

		public override void Unload()
        {
            Unloader.Unload();
		}

        public override void PreSaveAndQuit()
        {
            foreach (SoundEffectInstance sound in activeRumbleSounds)
            {
                sound.Stop();
            }
            activeRumbleSounds.Clear();
        }

        public override void PostSetupContent()
        {
            WeakReferences.PerformModSupport();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
            int num = layers.FindIndex((GameInterfaceLayer layer) => layer.Name == "Vanilla: Mouse Text");
			if (num != -1)
			{
                layers.Insert(num, new LegacyGameInterfaceLayer("Death Worm Head Icon", delegate()
				{
                    if (!Main.mapFullscreen && (Main.mapStyle == 1 || Main.mapStyle == 2))
                    {
                        string text = "";
                        Bosses.Invoker.DeathWorm.DeathWormBody.DrawHeadIcon(Main.spriteBatch, ref text);
                    }
					return true;
				}, InterfaceScaleType.UI));
            }
		}

        public override void PostDrawFullscreenMap(ref string mouseText)
		{
            if (Main.mapFullscreen)
            {
                Bosses.Invoker.DeathWorm.DeathWormBody.DrawHeadIcon(Main.spriteBatch, ref mouseText);
            }
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
            if (Main.netMode == NetmodeID.SinglePlayer) { Main.NewText(s, colorR, colorG, colorB); }
            else if (Main.netMode == NetmodeID.MultiplayerClient) { Main.NewText(s, colorR, colorG, colorB); }
            else if (sync && Main.netMode == NetmodeID.Server) { NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color(colorR, colorG, colorB), -1); }
        }

        public static void ShowTitle(NPC npc, int ID)
        {
            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<AAMod.Title>(), 0, 0, Main.myPlayer, ID, 0);
        }

        public static void ShowTitle(Player player, int ID)
        {
            Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AAMod.Title>(), 0, 0, Main.myPlayer, ID, 0);
        }

        public static void ShowSistersTitle(NPC npc)
        {
            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<AAMod.SistersTitle>(), 0, 0, Main.myPlayer, 16, 0);
        }
	}
}