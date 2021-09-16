using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

using AAModEXAI.Localization;

namespace AAModEXAI.Bosses
{
    public class SpawnBossMethod
    {
        // SpawnBoss(player, "MyBoss", true, 0, 0, "DerpyBoi", false);
        public static void SpawnBoss(Player player, string type, bool spawnMessage = true, int overrideDirection = 0, int overrideDirectionY = 0, string overrideDisplayName = "", bool namePlural = false)
        {
            Mod mod = AAModEXAI.instance;
            SpawnBoss(player, mod.NPCType(type), spawnMessage, overrideDirection, overrideDirectionY, overrideDisplayName, namePlural);
        }

        // SpawnBoss(player, ModContent.NPCType<MyBoss>(), true, 0, 0, "DerpyBoi 2", false);
        public static void SpawnBoss(Player player, int bossType, bool spawnMessage = true, int overrideDirection = 0, int overrideDirectionY = 0, string overrideDisplayName = "", bool namePlural = false)
        {
            if (overrideDirection == 0)
            {
                overrideDirection = Main.rand.Next(2) == 0 ? -1 : 1;
            }

            if (overrideDirectionY == 0)
            {
                overrideDirectionY = -1;
            }

            Vector2 npcCenter = player.Center + new Vector2(MathHelper.Lerp(500f, 800f, (float)Main.rand.NextDouble()) * overrideDirection, 800f * overrideDirectionY);
            SpawnBoss(player, bossType, spawnMessage, npcCenter, overrideDisplayName, namePlural);
        }

        // SpawnBoss(player, "MyBoss", true, player.Center + new Vector2(0, -800f), "DerpFromAbove", false);
        public static void SpawnBoss(Player player, string type, bool spawnMessage = true, Vector2 npcCenter = default, string overrideDisplayName = "", bool namePlural = false)
        {
            Mod mod = AAModEXAI.instance;
            SpawnBoss(player, mod.NPCType(type), spawnMessage, npcCenter, overrideDisplayName, namePlural);
        }

        // SpawnBoss(player, ModContent.NPCType<MyBoss>(), true, player.Center + new Vector2(0, 800f), "DerpFromBelow", false);
        public static void SpawnBoss(Player player, int bossType, bool spawnMessage = true, Vector2 npcCenter = default, string overrideDisplayName = "", bool namePlural = false)
        {
            if (npcCenter == default)
            {
                npcCenter = player.Center;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.AnyNPCs(bossType))
                {
                    return;
                }

                int npcID = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y, bossType);
                Main.npc[npcID].Center = npcCenter;
                Main.npc[npcID].netUpdate2 = true;

                if (spawnMessage)
                {
                    string npcName = !string.IsNullOrEmpty(Main.npc[npcID].GivenName) ? Main.npc[npcID].GivenName : overrideDisplayName;
                    if ((npcName == null || npcName.Equals("")) && Main.npc[npcID].modNPC != null)
                    {
                        npcName = Main.npc[npcID].modNPC.DisplayName.GetDefault();
                    }

                    if (namePlural)
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                BaseUtility.Chat(npcName + " " + Trans.text("Common", "NPCarrive"), 175, 75, 255, false);
                            }
                        }
                        else if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(npcName + Trans.text("Common", "NPCarrive")), new Color(175, 75, 255));
                        }
                    }
                    else
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                BaseUtility.Chat(Language.GetTextValue("Announcement.HasAwoken", npcName), 175, 75, 255, false);
                            }
                        }
                        else if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.BroadcastChatMessage(
                                NetworkText.FromKey("Announcement.HasAwoken", new object[] { NetworkText.FromLiteral(npcName) }), 
                                new Color(175, 75, 255)
                            );
                        }
                    }
                }
            }
            else
            {
                //I have no idea how to convert this to the standard system so im gonna post this method too lol
                SendNetMessage(SummonNPCFromClient, (byte)player.whoAmI, (short)bossType, spawnMessage, (int)npcCenter.X, (int)npcCenter.Y, overrideDisplayName, namePlural);
            }
        }

        public static void SpawnRajah(Player player, bool spawnMessage = false, Vector2 npcCenter = default, string overrideDisplayName = "", bool namePlural = false)
        {
            if (npcCenter == default)
            {
                npcCenter = player.Center;
            }

            int RajahType = ModContent.NPCType<Bosses.Rajah.Rajah>();

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.AnyNPCs(RajahType))
                {
                    return;
                }

                int npcID = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y, RajahType, 0, 0, 0, player.whoAmI);
                Main.npc[npcID].Center = npcCenter;
                Main.npc[npcID].netUpdate = true;
            }
            else
            {
                //I have no idea how to convert this to the standard system so im gonna post this method too lol
                SendNetMessage(SummonNPCFromClient, (byte)player.whoAmI, (short)RajahType, spawnMessage, (int)npcCenter.X, (int)npcCenter.Y, overrideDisplayName, namePlural);
            }
        }

        public static void SendNetMessage(int msg, params object[] param)
        {
            SendNetMessageClient(msg, -1, param);
        }

        public static void SendNetMessageClient(int msg, int client, params object[] param)
        {
            try
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    return;
                }

                BaseNet.WriteToPacket(AAModEXAI.instance.GetPacket(), (byte)msg, param).Send(client);
            }
            catch (Exception e)
            {
                string mode = Main.netMode == NetmodeID.Server ? "--SERVER-- " : "--CLIENT-- ";
                AAModEXAI.instance.Logger.Error($"{mode} ERROR SENDING MSG: {msg}: {e.Message}");
                AAModEXAI.instance.Logger.Info(e.StackTrace);
                AAModEXAI.instance.Logger.Info("-------");

                string param2 = "";
                for (int m = 0; m < param.Length; m++)
                {
                    param2 += param[m];
                }

                AAModEXAI.instance.Logger.Info("PARAMS: " + param2);
                AAModEXAI.instance.Logger.Info("-------");
            }
        }

        public const byte SummonNPCFromClient = 0;
    }
}
