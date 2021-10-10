using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.IO;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.World.Generation;

namespace AAModEXAI.Base.SubWorld
{
	public class SLWorld : ModWorld
	{
		public static GenerationProgress progress { get; internal set; }

		public override void Initialize()
		{
			SLWorld.subworld = false;
			SLWorld.drawUnderworldBackground = true;
			SLWorld.noReturn = false;
			SLWorld.votingFor = 0;
			SLWorld.votingToLeave = false;
			for (int i = 0; i < 255; i++)
			{
				SLWorld.votes[i] = null;
			}
			SLWorld.votingTimer = 0;
		}

		public override void NetSend(BinaryWriter writer)
		{
			if (SLWorld.subworld)
			{
				writer.Write(true);
				for (int i = 0; i < Subworld.subworlds.Count; i++)
				{
					if (Subworld.subworlds[i].id == SLWorld.currentSubworld.id)
					{
						writer.Write((ushort)i);
						break;
					}
				}
			}
			else
			{
				writer.Write(false);
				writer.Write(0);
			}
			if (SLWorld.votingTimer > 0)
			{
				writer.Write(SLWorld.votingFor);
				writer.Write(SLWorld.votingToLeave);
			}
			else
			{
				writer.Write(0);
				writer.Write(false);
			}
			writer.Write(SLWorld.votingTimer);
		}

		public override void NetReceive(BinaryReader reader)
		{
			SLWorld.drawMenu = true;
			bool flag = reader.ReadBoolean();
			ushort index = reader.ReadUInt16();
			if (flag && !SLWorld.subworld)
			{
				SLWorld.subworld = true;
				SLWorld.currentSubworld = Subworld.subworlds[(int)index];
				SLWorld.drawUnderworldBackground = false;
				SLWorld.currentSubworld.loadingUI.SetState(SLWorld.currentSubworld.loadingUIState);
				SLWorld.currentSubworld.Load();
			}
			bool flag2 = SLWorld.votingTimer != 0;
			SLWorld.votingFor = reader.ReadUInt16();
			SLWorld.votingToLeave = reader.ReadBoolean();
			SLWorld.votingTimer = reader.ReadUInt16();
			if (!flag2 && SLWorld.votingTimer > 0 && SubworldLibrary.Instance.UI.CurrentState == null)
			{
				UserInterface ui = SubworldLibrary.Instance.UI;
				if (ui == null)
				{
					return;
				}
				ui.SetState(Subworld.subworlds[(int)SLWorld.votingFor].votingUI);
			}
		}

		internal static void DrawMenu(GameTime gameTime)
		{
			Main.instance.GraphicsDevice.Clear(Color.Black);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.instance.Rasterizer, null, Main.UIScaleMatrix);
			SLWorld.currentSubworld.loadingUI.Draw(Main.spriteBatch, gameTime);
			Main.DrawCursor(Main.DrawThickCursor(false), false);
			Main.spriteBatch.End();
		}

		internal static void GenerateSubworlds()
		{
			SLWorld.origin = Main.ActiveWorldFileData;
			bool isCloudSave = SLWorld.origin.IsCloudSave;
			foreach (Subworld subworld in Subworld.subworlds)
			{
				if (subworld.saveSubworld)
				{
					SLWorld.currentSubworld = subworld;
					SLWorld.LoadSubworld(Path.Combine(Main.WorldPath, "Subworlds", Path.GetFileNameWithoutExtension(SLWorld.origin.Path), SLWorld.currentSubworld.id + ".wld"), isCloudSave);
					WorldFile.saveWorld(isCloudSave, false);
					Main.ActiveWorldFileData = SLWorld.origin;
				}
			}
		}

		internal static void TallyVotes(bool returnIfNull)
		{
			int num = 0;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					if (SLWorld.votes[i] != null)
					{
						int num2 = num;
						bool? flag = SLWorld.votes[i];
						bool flag2 = true;
						num = num2 + ((flag.GetValueOrDefault() == flag2 & flag != null) ? 1 : -1);
					}
					else if (returnIfNull)
					{
						return;
					}
				}
			}
			if (num > 0)
			{
				if (!SLWorld.votingToLeave)
				{
					Subworld.subworlds[(int)SLWorld.votingFor].OnVotedFor();
					return;
				}
				ThreadPool.QueueUserWorkItem(new WaitCallback(SLWorld.ExitWorldCallBack), false);
				ModPacket packet = AAModEXAI.instance.GetPacket(256);
				packet.Write(4);
				packet.Send(-1, -1);
			}
		}

		internal static void VotingCountdown()
		{
			if (Main.netMode == NetmodeID.Server && SLWorld.votingTimer > 0 && (SLWorld.votingTimer -= 1) <= 0)
			{
				SLWorld.TallyVotes(false);
			}
		}

		private static void SyncEquipment(byte whoAmI, byte i, int j)
		{
			NetMessage.SendData(5, -1, -1, null, (int)whoAmI, (float)i, (float)j, 0f, 0, 0, 0);
		}

		internal static void ExitWorldCallBack(object threadContext)
		{
			SLWorld.loading = true;
			try
			{
				Main.PlaySound(34, -1, -1, 0, 1f, 0f);
				Main.PlaySound(35, -1, -1, 0, 1f, 0f);
			}
			catch
			{
			}
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				WorldFile.CacheSaveTime();
			}
			Main.invasionProgress = 0;
			Main.invasionProgressDisplayLeft = 0;
			Main.invasionProgressAlpha = 0f;
			Main.gameMenu = true;
			if (Main.netMode != NetmodeID.Server)
			{
				SLWorld.drawMenu = false;
				Main.menuMode = 888;
				SLWorld.currentSubworld.loadingUI.SetState(SLWorld.currentSubworld.loadingUIState);
				Main.StopTrackedSounds();
				CaptureInterface.ResetFocus();
				Main.ActivePlayerFileData.StopPlayTimer();
				Player.SavePlayer(Main.ActivePlayerFileData, false);
			}
			if (!(bool)threadContext)
			{
				SLWorld.currentSubworld.Unload();
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				WorldFile.saveWorld();
			}
			Main.fastForwardTime = false;
			Main.UpdateSundial();
			WorldGen.noMapUpdate = true;
			if (!(bool)threadContext && SLWorld.currentSubworld.disablePlayerSaving && Main.netMode != NetmodeID.Server)
			{
				PlayerFileData fileData = Player.GetFileData(Main.ActivePlayerFileData.Path, Main.ActivePlayerFileData.IsCloudSave);
				if (fileData != null)
				{
					byte b = (byte)Main.LocalPlayer.whoAmI;
					fileData.SetAsActive();
					fileData.Player.whoAmI = (int)b;
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						NetMessage.SendData(4, -1, -1, null, (int)b, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(68, -1, -1, null, (int)b, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(16, -1, -1, null, (int)b, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(42, -1, -1, null, (int)b, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(50, -1, -1, null, (int)b, 0f, 0f, 0f, 0, 0, 0);
						byte b2 = 0;
						for (int i = 0; i < 59; i++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.inventory[i].prefix);
							b2 += 1;
						}
						for (int j = 0; j < fileData.Player.armor.Length; j++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.armor[j].prefix);
							b2 += 1;
						}
						for (int k = 0; k < fileData.Player.dye.Length; k++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.dye[k].prefix);
							b2 += 1;
						}
						for (int l = 0; l < fileData.Player.miscEquips.Length; l++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.miscEquips[l].prefix);
							b2 += 1;
						}
						for (int m = 0; m < fileData.Player.miscDyes.Length; m++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.miscDyes[m].prefix);
							b2 += 1;
						}
						for (int n = 0; n < fileData.Player.bank.item.Length; n++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.bank.item[n].prefix);
							b2 += 1;
						}
						for (int num = 0; num < fileData.Player.bank2.item.Length; num++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.bank2.item[num].prefix);
							b2 += 1;
						}
						SLWorld.SyncEquipment(b, b2, (int)fileData.Player.trashItem.prefix);
						b2 += 1;
						for (int num2 = 0; num2 < fileData.Player.bank3.item.Length; num2++)
						{
							SLWorld.SyncEquipment(b, b2, (int)fileData.Player.bank3.item[num2].prefix);
							b2 += 1;
						}
					}
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				SLWorld.PlayWorld((bool)threadContext);
			}
			SLWorld.loading = false;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005230 File Offset: 0x00003430
		internal static void PlayWorld(bool playingSubworld)
		{
			try
			{
				string text = "";
				WorldGen.gen = true;
				WorldGen.loadFailed = false;
				WorldGen.loadSuccess = false;
				WorldGen.worldBackup = true;
				if (!SLWorld.subworld)
				{
					SLWorld.origin = Main.ActiveWorldFileData;
				}
				bool isCloudSave = SLWorld.origin.IsCloudSave;
				string text2 = playingSubworld ? Path.Combine(Main.WorldPath, "Subworlds", Path.GetFileNameWithoutExtension(SLWorld.origin.Path), SLWorld.currentSubworld.id + ".wld") : SLWorld.origin.Path;
				if (!playingSubworld || SLWorld.currentSubworld.saveSubworld)
				{
					SLWorld.LoadWorldFile(text2, isCloudSave);
					if (WorldGen.loadFailed)
					{
						SLWorld.LoadWorldFile(text2, isCloudSave);
						if (WorldGen.loadFailed)
						{
							if (FileUtilities.Exists(text2 + ".bak", isCloudSave))
							{
								FileUtilities.Copy(text2, text2 + ".bad", isCloudSave, true);
								FileUtilities.Copy(text2 + ".bak", text2, isCloudSave, true);
								FileUtilities.Delete(text2 + ".bak", isCloudSave);
								string text3 = Path.ChangeExtension(text2, ".twld");
								if (FileUtilities.Exists(text3, isCloudSave))
								{
									FileUtilities.Copy(text3, text3 + ".bad", isCloudSave, true);
								}
								if (FileUtilities.Exists(text3 + ".bak", isCloudSave))
								{
									FileUtilities.Copy(text3 + ".bak", text3, isCloudSave, true);
									FileUtilities.Delete(text3 + ".bak", isCloudSave);
								}
								SLWorld.LoadWorldFile(text2, isCloudSave);
								if (WorldGen.loadFailed)
								{
									SLWorld.LoadWorldFile(text2, isCloudSave);
									if (WorldGen.loadFailed)
									{
										FileUtilities.Copy(text2, text2 + ".bak", isCloudSave, true);
										FileUtilities.Copy(text2 + ".bad", text2, isCloudSave, true);
										FileUtilities.Delete(text2 + ".bad", isCloudSave);
										if (FileUtilities.Exists(text3, isCloudSave))
										{
											FileUtilities.Copy(text3, text3 + ".bak", isCloudSave, true);
										}
										if (FileUtilities.Exists(text3 + ".bad", isCloudSave))
										{
											FileUtilities.Copy(text3 + ".bad", text3, isCloudSave, true);
											FileUtilities.Delete(text3 + ".bad", isCloudSave);
										}
									}
								}
							}
							else
							{
								WorldGen.worldBackup = false;
							}
						}
					}
				}
				if (playingSubworld)
				{
					if (WorldGen.loadFailed)
					{
						text = "Failed to load \"" + (playingSubworld ? SLWorld.currentSubworld.id : SLWorld.origin.Name) + "\" from file";
						if (!WorldGen.worldBackup)
						{
							text += ", no backup";
						}
						AAModEXAI.instance.Logger.Warn(text);
					}
					if (!WorldGen.loadSuccess)
					{
						SLWorld.LoadSubworld(text2, isCloudSave);
					}
					Main.worldName = SLWorld.currentSubworld.Name;
					SLWorld.drawUnderworldBackground = false;
					SLWorld.currentSubworld.Load();
				}
				else if (!WorldGen.loadSuccess)
				{
					text = "Failed to load \"" + (playingSubworld ? SLWorld.currentSubworld.id : SLWorld.origin.Name) + "\" from file";
					if (!WorldGen.worldBackup)
					{
						text += ", no backup";
					}
					AAModEXAI.instance.Logger.Warn(text);
					Main.menuMode = 0;
					SLWorld.drawMenu = true;
					if (Main.netMode == NetmodeID.Server)
					{
						Netplay.disconnect = true;
					}
					return;
				}
				SLWorld.subworld = playingSubworld;
				WorldGen.gen = false;
				if (Main.netMode == NetmodeID.Server)
				{
					for (int i = 0; i < 255; i++)
					{
						if (Netplay.Clients[i].IsActive)
						{
							Netplay.Clients[i].State = 2;
							Netplay.Clients[i].ResetSections();
						}
					}
					ModPacket packet = AAModEXAI.instance.GetPacket(256);
					packet.Write(2);
					packet.Send(-1, -1);
				}
				else
				{
					Main.player[Main.myPlayer].Spawn();
					if (Main.mapEnabled)
					{
						Main.Map.Load();
					}
					Main.sectionManager.SetAllFramesLoaded();
					while (Main.mapEnabled && Main.loadMapLock)
					{
						float num = (float)Main.loadMapLastX / (float)Main.maxTilesX;
						Main.statusText = string.Concat(new object[]
						{
							Language.GetTextValue("LegacyWorldGen.68"),
							" ",
							(int)(num * 100f + 1f),
							"%"
						});
						Thread.Sleep(0);
					}
					Main.player[Main.myPlayer].Update(Main.myPlayer);
					Player.Hooks.EnterWorld(Main.myPlayer);
					if (Main.anglerWhoFinishedToday.Contains(Main.player[Main.myPlayer].name))
					{
						Main.anglerQuestFinished = true;
					}
					Main.resetClouds = true;
					SLWorld.drawMenu = true;
					Main.gameMenu = false;
					Main.ActivePlayerFileData.StartPlayTimer();
					if (text != "")
					{
						Main.NewText(text, byte.MaxValue, 25, 25, false);
					}
				}
			}
			catch (Exception ex)
			{
				AAModEXAI.instance.Logger.Error(ex);
			}
		}

		internal static void LoadSubworld(string path, bool fromCloud)
		{
			WorldFileData worldFileData = new WorldFileData(path, fromCloud);
			worldFileData.Name = SLWorld.currentSubworld.Name;
			worldFileData.IsExpertMode = Main.expertMode;
			worldFileData.CreationTime = DateTime.Now;
			worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
			worldFileData.SetFavorite(false, true);
			worldFileData.WorldGeneratorVersion = 828928688129UL;
			using (MD5 md = MD5.Create())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(Path.GetFileNameWithoutExtension(SLWorld.origin.Path) + SLWorld.currentSubworld.id);
				byte[] b = md.ComputeHash(bytes);
				worldFileData.UniqueId = new Guid(b);
			}
			worldFileData.SetSeed(SLWorld.origin.SeedText);
			Main.ActiveWorldFileData = worldFileData;
			Main.maxTilesX = SLWorld.currentSubworld.width;
			Main.maxTilesY = SLWorld.currentSubworld.height;
			Main.spawnTileX = Main.maxTilesX / 2;
			Main.spawnTileY = Main.maxTilesY / 2;
			WorldGen.setWorldSize();
			WorldGen.clearWorld();
			Main.worldSurface = (double)Main.maxTilesY * 0.3;
			Main.rockLayer = (double)Main.maxTilesY * 0.5;
			WorldGen.waterLine = Main.maxTilesY;
			Main.weatherCounter = int.MaxValue;
			Cloud.resetClouds();
			float num = 0f;
			for (int i = 0; i < SLWorld.currentSubworld.tasks.Count; i++)
			{
				num += SLWorld.currentSubworld.tasks[i].Weight;
			}
			SLWorld.progress = new GenerationProgress();
			SLWorld.progress.TotalWeight = num;
			for (int j = 0; j < SLWorld.currentSubworld.tasks.Count; j++)
			{
				WorldGen._genRand = new UnifiedRandom(worldFileData.Seed);
				Main.rand = new UnifiedRandom(worldFileData.Seed);
				SLWorld.progress.Start(SLWorld.currentSubworld.tasks[j].Weight);
				SLWorld.currentSubworld.tasks[j].Apply(SLWorld.progress);
				SLWorld.progress.End();
			}
			SLWorld.progress = null;
		}

		internal static void LoadWorldFile(string path, bool fromCloud)
		{
			bool flag = fromCloud && SocialAPI.Cloud != null;
			if (!FileUtilities.Exists(path, flag))
			{
				if (!flag)
				{
					for (int i = path.Length - 1; i >= 0; i--)
					{
						if (path.Substring(i, 1) == string.Concat(Path.DirectorySeparatorChar))
						{
							Directory.CreateDirectory(path.Substring(0, i));
							return;
						}
					}
					return;
				}
			}
			else
			{
				using (MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(path, flag)))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						try
						{
							int num = WorldFile.LoadWorld_Version2(binaryReader);
							binaryReader.Close();
							memoryStream.Close();
							SubworldLibrary.WorldHooks_SetupWorld.Invoke(null, null);
							SubworldLibrary.WorldIO_Load.Invoke(null, new object[]
							{
								path,
								flag
							});
							if (num != 0)
							{
								WorldGen.loadFailed = true;
								WorldGen.loadSuccess = false;
								return;
							}
							WorldGen.loadSuccess = true;
							WorldGen.loadFailed = false;
							WorldGen.waterLine = Main.maxTilesY;
							Liquid.QuickWater(2, -1, -1);
							WorldGen.WaterCheck();
							Liquid.quickSettle = true;
							int num2 = 0;
							int num3 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
							float num4 = 0f;
							while (Liquid.numLiquid > 0 && num2 < 100000)
							{
								num2++;
								float num5 = (float)(num3 - Liquid.numLiquid + LiquidBuffer.numLiquidBuffer) / (float)num3;
								if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num3)
								{
									num3 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
								}
								if (num5 > num4)
								{
									num4 = num5;
								}
								else
								{
									num5 = num4;
								}
								Main.statusText = string.Concat(new object[]
								{
									Language.GetTextValue("LegacyWorldGen.27"),
									" ",
									(int)(num5 * 100f / 2f + 50f),
									"%"
								});
								Liquid.UpdateLiquid();
							}
							Liquid.quickSettle = false;
							Main.weatherCounter = WorldGen.genRand.Next(3600, 18000);
							Cloud.resetClouds();
							WorldGen.WaterCheck();
							if (Main.slimeRainTime > 0.0)
							{
								Main.StartSlimeRain(false);
							}
							WorldFile.SetOngoingToTemps();
						}
						catch
						{
							WorldGen.loadFailed = true;
							WorldGen.loadSuccess = false;
							try
							{
								binaryReader.Close();
								memoryStream.Close();
							}
							catch
							{
							}
							return;
						}
					}
				}
				WorldFileData allMetadata = WorldFile.GetAllMetadata(path, fromCloud);
				if (allMetadata != null)
				{
					Main.ActiveWorldFileData = allMetadata;
				}
			}
		}

		public static bool subworld;

		internal static bool loading;

		public static Subworld currentSubworld;

		public static bool drawUnderworldBackground;

		public static bool drawMenu = true;

		public static bool noReturn;

		internal static ushort votingFor;

		internal static bool votingToLeave;

		internal static bool?[] votes = new bool?[255];

		internal static ushort votingTimer;

		public static WorldFileData origin;
	}
}
