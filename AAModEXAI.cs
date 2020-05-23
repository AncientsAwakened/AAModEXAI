using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Localization;

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using AAMod;
using Terraria.ID;

namespace AAModEXAI
{
	public class AAModEXAI : Mod
	{
		public static Mod Instance;
		internal static AAModEXAI instance;
        public Harmony harmony = new Harmony("ModifyDD2");
        public MethodInfo DD2Invasion = typeof(Terraria.GameContent.Events.DD2Event).GetMethod("GetInvasionStatus", BindingFlags.Static | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[]{typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(bool)}, null);
        public MethodInfo DD2InvasionPatch = typeof(AAModEXAI).GetMethod("GetInvasionStatusPrefix", BindingFlags.Static | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[]{typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(bool)}, null);
        private static bool GetInvasionStatusPrefix(out int currentWave, out int requiredKillCount, out int currentKillCount, bool currentlyInCheckProgress = false)
        {
            currentWave = NPC.waveNumber;
            requiredKillCount = 10;
            currentKillCount = (int)NPC.waveKills;
            if(NPC.downedMoonlord)
            {
                Main.NewText("Okey", 67, 110, 238, false);
                requiredKillCount = Difficulty_4_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
                return false;
            }
            return true;
        }

        private static int Difficulty_4_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
        {
            switch (waveNumber)
			{
                case -1:
                    return 0;
                case 1:
                    return 60;
                case 2:
                    return 80;
                case 3:
                    return 100;
                case 4:
                    return 120;
                case 5:
                    return 140;
                case 6:
                    return 180;
                case 7:
                {
                    int num = NPC.FindFirstNPC(551);
                    if (num == -1)
                    {
                        return 1;
                    }
                    currentKillCount = 100 - (int)((float)Main.npc[num].life / (float)Main.npc[num].lifeMax * 100f);
                    return 100;
                }
                case 8:
                    return 200;
                case 9:
                    waveNumber = 8;
                    currentKillCount = 1;
                    if (currentlyInCheckProgress)
                    {
                        Terraria.GameContent.Events.DD2Event.StartVictoryScene();
                    }
                    return 1;
			}
			return 10;
        }

		public AAModEXAI()
		{
			Instance = this;
			instance = this;
		}

        public override void Load()
        {
            var mOriginal = AccessTools.Method(typeof(Terraria.GameContent.Events.DD2Event), "GetInvasionStatus", new Type[]{typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(bool)});
            harmony.Patch(mOriginal, new HarmonyMethod(DD2InvasionPatch));
            //harmony.Patch(DD2Invasion, new HarmonyMethod(DD2InvasionPatch));
        }

		public override void Unload()
        {
			Instance = null;
			instance = null;
            harmony.Unpatch(DD2Invasion, HarmonyPatchType.Prefix, "ModifyDD2");
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