using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using IL.Terraria;
using IL.Terraria.IO;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;

//using Terraria;
//using Terraria.IO;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.World.Generation;

namespace AAModEXAI
{
	public class SubworldLibrary
	{
		internal static event ILContext.Manipulator PreUpdate
		{
			add
			{
				HookEndpointManager.Modify(typeof(WorldHooks).GetMethod("PreUpdate"), value);
			}
			remove
			{
				HookEndpointManager.Unmodify(typeof(WorldHooks).GetMethod("PreUpdate"), value);
			}
		}

		internal static event ILContext.Manipulator PostUpdate
		{
			add
			{
				HookEndpointManager.Modify(typeof(WorldHooks).GetMethod("PostUpdate"), value);
			}
			remove
			{
				HookEndpointManager.Unmodify(typeof(WorldHooks).GetMethod("PostUpdate"), value);
			}
		}

		internal static event ILContext.Manipulator Save
		{
			add
			{
				HookEndpointManager.Modify(SubworldLibrary.WorldIO.GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic), value);
			}
			remove
			{
				HookEndpointManager.Unmodify(SubworldLibrary.WorldIO.GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic), value);
			}
		}

		public SubworldLibrary()
		{
			SubworldLibrary.Instance = this;
		}

		public static void Load()
		{
			SubworldLibrary.WorldIO = typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.WorldIO");
			SubworldLibrary.WorldIO_Load = SubworldLibrary.WorldIO.GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic);
			SubworldLibrary.WorldHooks_SetupWorld = typeof(WorldHooks).GetMethod("SetupWorld", BindingFlags.Static | BindingFlags.NonPublic);
			Main.UpdateTime += new ILContext.Manipulator(HookUpdateTime);
			WorldGen.UpdateWorld += new ILContext.Manipulator(HookUpdateWorld);
			Liquid.Update += new ILContext.Manipulator(HookUpdate);
			SubworldLibrary.PreUpdate += new ILContext.Manipulator(HookPreUpdate);
			SubworldLibrary.PostUpdate += new ILContext.Manipulator(HookPostUpdate);
			Player.Update += new ILContext.Manipulator(HookUpdatePlayer);
			NPC.UpdateNPC_UpdateGravity += new ILContext.Manipulator(HookUpdateGravity);
			WorldGen.do_worldGenCallBack += new ILContext.Manipulator(HookWorldGen);
			Player.SavePlayer += new ILContext.Manipulator(HookSavePlayer);
			WorldFile.saveWorld_bool_bool += new ILContext.Manipulator(HookSaveWorld);
			SubworldLibrary.Save += new ILContext.Manipulator(HookSave);
			Main.EraseWorld += new ILContext.Manipulator(HookEraseWorld);
			Main.DoDraw += new ILContext.Manipulator(HookDoDraw);
			Main.DrawBlack += new ILContext.Manipulator(HookDrawBlack);
			Main.DrawBG += new ILContext.Manipulator(HookDrawBG);
			Main.DrawBackground += new ILContext.Manipulator(HookDrawBackground);
			Main.OldDrawBackground += new ILContext.Manipulator(HookOldDrawBackground);
			Main.DrawUnderworldBackground += new ILContext.Manipulator(HookDrawUnderworldBackground);
			Player.UpdateBiomes += new ILContext.Manipulator(HookUpdateBiomes);
			IngameOptions.Draw += new ILContext.Manipulator(HookDraw);
			Lighting.PreRenderPhase += new ILContext.Manipulator(HookPreRenderPhase);
			Subworld.subworlds = new List<Subworld>();

			foreach (Type type in ModContent.GetInstance<AAModEXAI>().Code.GetExportedTypes())
			{
				if (!type.IsAbstract && !(type.GetConstructor(new Type[0]) == null) && type.IsSubclassOf(typeof(Subworld)))
				{
					Subworld subworld = (Subworld)Activator.CreateInstance(type);
					subworld.mod = ModContent.GetInstance<AAModEXAI>();
					subworld.id = ModContent.GetInstance<AAModEXAI>().Name + "_" + subworld.GetType().Name;
					Subworld.subworlds.Add(subworld);
				}
			}

			if (!Terraria.Main.dedServ)
			{
				AAModEXAI.instance.SubWorldInterface = new UserInterface();
			}
			ModTranslation modTranslation = ModContent.GetInstance<AAModEXAI>().CreateTranslation("SubworldReturn");
			modTranslation.SetDefault("Return");
			ModContent.GetInstance<AAModEXAI>().AddTranslation(modTranslation);
			modTranslation = ModContent.GetInstance<AAModEXAI>().CreateTranslation("Voting");
			modTranslation.SetDefault("Someone wants to enter {0}.");
			ModContent.GetInstance<AAModEXAI>().AddTranslation(modTranslation);
			modTranslation = ModContent.GetInstance<AAModEXAI>().CreateTranslation("VotingToLeave");
			modTranslation.SetDefault("Someone wants to leave.");
			ModContent.GetInstance<AAModEXAI>().AddTranslation(modTranslation);
		}

		public static void AddRecipes()
		{
			SubworldLibrary.canRegister = false;
		}

		public static object Call(params object[] args)
		{
			try
			{
				string text = args[0] as string;
				if (text == "Register")
				{
					if (!SubworldLibrary.canRegister)
					{
						AAModEXAI.instance.Logger.Error("Call Error: Register called too late!");
						return null;
					}
					if (args.Length < 6)
					{
						AAModEXAI.instance.Logger.Error("Call Error: Missing " + (6 - args.Length) + " parameters!");
						return null;
					}
					Mod mod = args[1] as Mod;
					string text2 = mod.Name + "_" + args[2];
					int i;
					for (i = 0; i < Subworld.subworlds.Count; i++)
					{
						if (Subworld.subworlds[i].id == text2)
						{
							AAModEXAI.instance.Logger.Error("Call Error: \"" + text2 + "\" has already been registered!");
							return null;
						}
					}
					i = 6;
					Subworld.subworlds.Add(new WeakRefSubworld(mod, text2, Convert.ToInt32(args[3]), Convert.ToInt32(args[4]), args[5] as List<GenPass>, (args.Length > i) ? (args[i] as Action) : null, (args.Length > ++i) ? (args[i] as Action) : null, (args.Length > ++i) ? (args[i] as ModWorld) : null, args.Length > ++i && Convert.ToBoolean(args[i]), args.Length > ++i && Convert.ToBoolean(args[i]), args.Length > ++i && Convert.ToBoolean(args[i]), args.Length <= ++i || Convert.ToBoolean(args[i]), (args.Length > ++i) ? (args[i] as UserInterface) : null, (args.Length > ++i) ? (args[i] as Func<UIState>) : null, (args.Length > ++i) ? (args[i] as Func<UIState>) : null, (args.Length > ++i) ? Convert.ToUInt16(args[i]) : Convert.ToUInt16(1800), (args.Length > ++i) ? (args[i] as Action) : null));
					return text2;
				}
				else if (text == "DrawUnderworldBackground")
				{
					if (args.Length > 1)
					{
						SLWorld.drawUnderworldBackground = Convert.ToBoolean(args[1]);
					}
					return SLWorld.drawUnderworldBackground;
				}
				else if (text == "AnyActive")
				{
					return Subworld.AnyActive(args[1] as Mod);
				}
				else if (text == "Exit")
				{
					Subworld.Exit(false);
					return true;
				}
				else if (text == "Enter")
				{
					return Subworld.Enter(args[1] as string, false);
				}
				else if (text == "IsActive")
				{
					return Subworld.IsActive(args[1] as string);
				}
				else if (text == "Current")
				{
					return SLWorld.subworld ? SLWorld.currentSubworld.id : null;
				}
				else if (text == "DrawMenu")
				{
					if (args.Length > 1)
					{
						SLWorld.drawMenu = Convert.ToBoolean(args[1]);
					}
					return SLWorld.drawMenu;
				}
			}
			catch (Exception ex)
			{
				AAModEXAI.instance.Logger.Error("Call Error: " + ex.StackTrace + ex.Message);
			}
			return null;
		}

		public static void Unload()
		{
			SubworldLibrary.WorldIO = typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.WorldIO");
			SubworldLibrary.PreUpdate -= new ILContext.Manipulator(HookPreUpdate);
			SubworldLibrary.PostUpdate -= new ILContext.Manipulator(HookPostUpdate);
			SubworldLibrary.Save -= new ILContext.Manipulator(HookSave);
			SubworldLibrary.WorldIO = null;
			Subworld.subworlds = null;
			SubworldLibrary.canRegister = true;
		}

		public static void UpdateUI(GameTime gameTime)
		{
			UserInterface ui = AAModEXAI.instance.SubWorldInterface;
			if (((ui != null) ? ui.CurrentState : null) != null)
			{
				AAModEXAI.instance.SubWorldInterface.Update(gameTime);
			}
		}

		public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int num = layers.FindIndex((GameInterfaceLayer layer) => layer.Name.Equals("Vanilla: Mouse Text"));
			if (num != -1)
			{
				layers.Insert(num, new LegacyGameInterfaceLayer("Subworld Library: Voting UI", delegate()
				{
					AAModEXAI.instance.SubWorldInterface.Draw(Terraria.Main.spriteBatch, new GameTime());
					return true;
				}, InterfaceScaleType.UI));
			}
		}

		public static void HandlePacket(BinaryReader reader, int whoAmI)
		{
			byte b = reader.ReadByte();
			switch (b)
			{
			case 0:
				Subworld.BeginEntering((int)reader.ReadUInt16());
				return;
			case 1:
				SLWorld.currentSubworld = Subworld.subworlds[(int)reader.ReadUInt16()];
				ThreadPool.QueueUserWorkItem(new WaitCallback(SLWorld.ExitWorldCallBack), true);
				Terraria.Netplay.Connection.State = 1;
				return;
			case 2:
				Terraria.Netplay.Connection.State = 3;
				Terraria.NetMessage.SendData(8, whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				return;
			case 3:
				Subworld.Exit(true);
				return;
			case 4:
				ThreadPool.QueueUserWorkItem(new WaitCallback(SLWorld.ExitWorldCallBack), false);
				Terraria.Netplay.Connection.State = 1;
				return;
			case 5:
			{
				ushort num = reader.ReadUInt16();
				if (SLWorld.votingTimer <= 0)
				{
					SLWorld.votingFor = num;
					SLWorld.votingToLeave = false;
					SLWorld.votes[whoAmI] = new bool?(true);
					SLWorld.votingTimer = Subworld.subworlds[(int)num].votingDuration;
					ModPacket packet = AAModEXAI.instance.GetPacket(256);
					packet.Write(7);
					packet.Write(num);
					packet.Write(false);
					packet.Send(-1, -1);
				}
				else
				{
					SLWorld.votes[whoAmI] = new bool?(!SLWorld.votingToLeave && num == SLWorld.votingFor);
				}
				SLWorld.TallyVotes(true);
				return;
			}
			case 6:
				if (SLWorld.votingTimer <= 0)
				{
					SLWorld.votingToLeave = true;
					SLWorld.votes[whoAmI] = new bool?(true);
					SLWorld.votingTimer = SLWorld.currentSubworld.votingDuration;
					ModPacket packet2 = AAModEXAI.instance.GetPacket(256);
					packet2.Write(7);
					packet2.Write(0);
					packet2.Write(true);
					packet2.Send(-1, -1);
				}
				else
				{
					SLWorld.votes[whoAmI] = new bool?(SLWorld.votingToLeave);
				}
				SLWorld.TallyVotes(true);
				return;
			case 7:
			{
				SLWorld.votingFor = reader.ReadUInt16();
				SLWorld.votingToLeave = reader.ReadBoolean();
				SLWorld.votingTimer = (SLWorld.votingToLeave ? SLWorld.currentSubworld.votingDuration : Subworld.subworlds[(int)SLWorld.votingFor].votingDuration);
				UserInterface ui = AAModEXAI.instance.SubWorldInterface;
				if (ui == null)
				{
					return;
				}
				ui.SetState(Subworld.subworlds[(int)SLWorld.votingFor].votingUI);
				return;
			}
			default:
				AAModEXAI.instance.Logger.WarnFormat("Unknown Message type: " + b, new object[0]);
				return;
			}
		}

		private static void HookUpdateTime(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookUpdateWorld(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.Index ++;
			ilcursor.Emit(OpCodes.Call, typeof(SLWorld).GetMethod("VotingCountdown", BindingFlags.Static | BindingFlags.NonPublic));
			ilcursor.EmitDelegate<Func<bool>>(() => SLWorld.subworld && SLWorld.currentSubworld.noWorldUpdate);
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Call, typeof(WorldHooks).GetMethod("PostUpdate"));
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookUpdate(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			if (!ILPatternMatchingExt.MatchLdsfld(ilcursor.Next, typeof(Terraria.Main), "maxTilesY"))
			{
				return;
			}
			ilcursor.Index -= 2;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			if (!ILPatternMatchingExt.MatchConvU1(ilcursor.Next))
			{
				return;
			}
			ilcursor.Index += 2;
			ilcursor.MarkLabel(illabel);
		}

		private static void HookPreUpdate(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.EmitDelegate<Func<bool>>(delegate()
			{
				bool flag = SLWorld.subworld && SLWorld.currentSubworld.modWorld != null;
				if (flag)
				{
					SLWorld.currentSubworld.modWorld.PreUpdate();
				}
				return flag;
			});
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookPostUpdate(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.EmitDelegate<Func<bool>>(delegate()
			{
				bool flag = SLWorld.subworld && SLWorld.currentSubworld.modWorld != null;
				if (flag)
				{
					SLWorld.currentSubworld.modWorld.PostUpdate();
				}
				return flag;
			});
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookUpdatePlayer(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			do
			{
				ILCursor ilcursor2 = ilcursor;
				Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
				array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 1));
				if (!ilcursor2.TryGotoNext(array))
				{
					return;
				}
				ILCursor ilcursor3 = ilcursor;
				int index = ilcursor3.Index;
				ilcursor3.Index = index + 1;
			}
			while (!ILPatternMatchingExt.MatchLdarg(ilcursor.Next, 0));
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ldc_R4, 1f);
			ILLabel illabel2 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Br, illabel2);
			ilcursor.MarkLabel(illabel);
			ILCursor ilcursor4 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 2));
			if (!ilcursor4.TryGotoNext(array2))
			{
				return;
			}
			ilcursor.MarkLabel(illabel2);
		}

		private static void HookUpdateGravity(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 0));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ILCursor ilcursor3 = ilcursor;
			int index = ilcursor3.Index;
			ilcursor3.Index = index + 1;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ILCursor ilcursor4 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchStsfld(i, typeof(Terraria.NPC).GetField("gravity", BindingFlags.Static | BindingFlags.NonPublic)));
			if (!ilcursor4.TryGotoNext(array2))
			{
				return;
			}
			ILCursor ilcursor5 = ilcursor;
			index = ilcursor5.Index;
			ilcursor5.Index = index + 1;
			ilcursor.MarkLabel(illabel);
		}

		private static void HookWorldGen(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchCall(i, typeof(Terraria.IO.WorldFile), "saveWorld"));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ILCursor ilcursor3 = ilcursor;
			int index = ilcursor3.Index;
			ilcursor3.Index = index + 1;
			ilcursor.Emit(OpCodes.Call, typeof(SLWorld).GetMethod("GenerateSubworlds", BindingFlags.Static | BindingFlags.NonPublic));
		}

		private static void HookSavePlayer(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.EmitDelegate<Func<bool>>(() => SLWorld.subworld && SLWorld.currentSubworld.disablePlayerSaving);
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookSaveWorld(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.EmitDelegate<Func<bool>>(() => SLWorld.subworld && !SLWorld.currentSubworld.saveSubworld);
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookSave(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdstr(i, "modData"));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ILCursor ilcursor3 = ilcursor;
			int index = ilcursor3.Index;
			ilcursor3.Index = index - 1;
			ilcursor.EmitDelegate<Func<bool>>(() => SLWorld.subworld && !SLWorld.currentSubworld.saveModData);
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ilcursor.Index += 2;
			ilcursor.EmitDelegate<Func<bool>>(() => SLWorld.subworld && SLWorld.currentSubworld.modWorld != null);
			ILLabel illabel2 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel2);
			ilcursor.EmitDelegate<Func<List<TagCompound>>>(() => new List<TagCompound>
			{
				SLWorld.currentSubworld.modWorld.Save()
			});
			ILLabel illabel3 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Br, illabel3);
			ilcursor.MarkLabel(illabel2);
			ILCursor ilcursor4 = ilcursor;
			index = ilcursor4.Index;
			ilcursor4.Index = index + 1;
			ilcursor.MarkLabel(illabel3);
			ILCursor ilcursor5 = ilcursor;
			index = ilcursor5.Index;
			ilcursor5.Index = index + 1;
			ilcursor.MarkLabel(illabel);
		}

		private static void HookEraseWorld(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.Emit(OpCodes.Ldarg_0);
			ilcursor.EmitDelegate<Action<int>>(delegate(int i)
			{
				string text = Path.Combine(Terraria.Main.WorldPath, "Subworlds", Path.GetFileNameWithoutExtension(Terraria.Main.WorldList[i].Path));
				bool isCloudSave = Terraria.Main.WorldList[i].IsCloudSave;
				if (FileUtilities.Exists(text, isCloudSave))
				{
					FileUtilities.Delete(text, isCloudSave);
				}
			});
		}

		private static void HookDoDraw(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStsfld(i, typeof(Terraria.Main), "HoverItem"));
			if (ilcursor2.TryGotoNext(array))
			{
				ILCursor ilcursor3 = ilcursor;
				int index = ilcursor3.Index;
				ilcursor3.Index = index + 1;
				ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawMenu"));
				ILLabel illabel = ilcursor.DefineLabel();
				ilcursor.Emit(OpCodes.Brtrue, illabel);
				ilcursor.Emit(OpCodes.Ldc_R4, 1f);
				ilcursor.Emit(OpCodes.Stsfld, typeof(Terraria.Main).GetField("_uiScaleUsed", BindingFlags.Static | BindingFlags.NonPublic));
				ilcursor.Emit(OpCodes.Ldc_R4, 1f);
				ilcursor.Emit(OpCodes.Ldc_R4, 1f);
				ilcursor.Emit(OpCodes.Ldc_R4, 1f);
				ilcursor.Emit(OpCodes.Call, typeof(Matrix).GetMethod("CreateScale", new Type[]
				{
					typeof(float),
					typeof(float),
					typeof(float)
				}));
				ilcursor.Emit(OpCodes.Stsfld, typeof(Terraria.Main).GetField("_uiScaleMatrix", BindingFlags.Static | BindingFlags.NonPublic));
				ilcursor.Emit(OpCodes.Ldarg_0);
				ilcursor.Emit(OpCodes.Call, typeof(SLWorld).GetMethod("DrawMenu", BindingFlags.Static | BindingFlags.NonPublic));
				ilcursor.Emit(OpCodes.Ret);
				ilcursor.MarkLabel(illabel);
			}
			ILCursor ilcursor4 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchStsfld(i, typeof(Terraria.Main), "atmo"));
			if (!ilcursor4.TryGotoNext(array2))
			{
				return;
			}
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel2 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel2);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Ldc_R4, 1f);
			ilcursor.MarkLabel(illabel2);
		}

		private static void HookDrawBlack(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			int index;
			do
			{
				ILCursor ilcursor2 = ilcursor;
				Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
				array[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdloc(i, 7));
				if (!ilcursor2.TryGotoNext(array))
				{
					return;
				}
				ILCursor ilcursor3 = ilcursor;
				index = ilcursor3.Index;
				ilcursor3.Index = index + 1;
			}
			while (!ILPatternMatchingExt.MatchLdsfld(ilcursor.Next, typeof(Terraria.Main), "maxTilesY"));
			ILCursor ilcursor4 = ilcursor;
			index = ilcursor4.Index;
			ilcursor4.Index = index - 1;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ILCursor ilcursor5 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdloc(i, 8));
			if (!ilcursor5.TryGotoNext(array2))
			{
				return;
			}
			ilcursor.MarkLabel(illabel);
		}

		private static void HookDrawBG(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdcR8(i, 10.0));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ilcursor.Index -= 6;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ILCursor ilcursor3 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdsfld(i, typeof(Terraria.Main), "shroomTiles"));
			if (!ilcursor3.TryGotoNext(array2))
			{
				return;
			}
			ilcursor.MarkLabel(illabel);
		}

		private static void HookDrawBackground(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			do
			{
				ILCursor ilcursor2 = ilcursor;
				Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
				array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 18));
				if (!ilcursor2.TryGotoNext(array))
				{
					goto IL_9F;
				}
			}
			while (!ILPatternMatchingExt.MatchLdcI4(ilcursor.Prev, 1));
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Ldloc_0);
			ilcursor.MarkLabel(illabel);
			IL_9F:
			ILCursor ilcursor3 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 20));
			if (!ilcursor3.TryGotoNext(array2))
			{
				return;
			}
			ilcursor.Index += 9;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel2 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel2);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Ldloc_1);
			ilcursor.Emit(OpCodes.Ldloc_0);
			ilcursor.MarkLabel(illabel2);
		}

		private static void HookOldDrawBackground(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			for (;;)
			{
				ILCursor ilcursor2 = ilcursor;
				Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
				array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 19));
				if (!ilcursor2.TryGotoNext(array))
				{
					break;
				}
				if (ILPatternMatchingExt.MatchLdcI4(ilcursor.Prev, 1))
				{
					goto Block_1;
				}
			}
			do
			{
				ILCursor ilcursor3 = ilcursor;
				Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
				array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdloc(i, 17));
				if (!ilcursor3.TryGotoNext(array2))
				{
					return;
				}
			}
			while (!ILPatternMatchingExt.MatchStfld(ilcursor.Prev, typeof(Terraria.Main), "bgTop"));
			ilcursor.Index += 8;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Ldloc_1);
			ilcursor.Emit(OpCodes.Ldloc_0);
			ilcursor.MarkLabel(illabel);
			return;
			Block_1:
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel2 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel2);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Ldloc_0);
			ilcursor.MarkLabel(illabel2);
		}

		private static void HookDrawUnderworldBackground(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ilcursor.Emit(OpCodes.Ret);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookUpdateBiomes(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 10));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brtrue, illabel);
			ilcursor.Emit(OpCodes.Pop);
			ilcursor.Emit(OpCodes.Ldc_I4_0);
			ilcursor.MarkLabel(illabel);
		}

		private static void HookDraw(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchLdcI4(i, 35));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ilcursor.Index -= 5;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("noReturn"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Pop);
			ILLabel illabel2 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Br, illabel2);
			ilcursor.MarkLabel(illabel);
			ilcursor.Index += 4;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ldstr, "Mods.AAModEXAI.SubworldReturn");
			ilcursor.Emit(OpCodes.Call, typeof(Language).GetMethod("GetTextValue", new Type[]
			{
				typeof(string)
			}));
			ILLabel illabel3 = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Br, illabel3);
			ilcursor.MarkLabel(illabel);
			ilcursor.Index += 4;
			ilcursor.MarkLabel(illabel3);
			ILCursor ilcursor3 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchCall(i, typeof(Terraria.WorldGen), "SaveAndQuit"));
			if (!ilcursor3.TryGotoNext(array2))
			{
				return;
			}
			ILCursor ilcursor4 = ilcursor;
			int index = ilcursor4.Index;
			ilcursor4.Index = index - 1;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("subworld"));
			illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ilcursor.Emit(OpCodes.Ldc_I4_0);
			ilcursor.Emit(OpCodes.Call, typeof(Subworld).GetMethod("Exit"));
			ilcursor.Emit(OpCodes.Br, illabel2);
			ilcursor.MarkLabel(illabel);
			ilcursor.Index += 2;
			ilcursor.MarkLabel(illabel2);
		}

		private static void HookPreRenderPhase(ILContext il)
		{
			ILCursor ilcursor = new ILCursor(il);
			ILCursor ilcursor2 = ilcursor;
			Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
			array[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 35));
			if (!ilcursor2.TryGotoNext(array))
			{
				return;
			}
			ILCursor ilcursor3 = ilcursor;
			int index = ilcursor3.Index;
			ilcursor3.Index = index + 1;
			ilcursor.Emit(OpCodes.Ldsfld, typeof(SLWorld).GetField("drawUnderworldBackground"));
			ILLabel illabel = ilcursor.DefineLabel();
			ilcursor.Emit(OpCodes.Brfalse, illabel);
			ILCursor ilcursor4 = ilcursor;
			Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
			array2[0] = ((Instruction i) => ILPatternMatchingExt.MatchStloc(i, 36));
			if (!ilcursor4.TryGotoNext(array2))
			{
				return;
			}
			ilcursor.Index -= 2;
			ilcursor.MarkLabel(illabel);
		}

		internal static SubworldLibrary Instance;

		internal static bool canRegister = true;

		internal static Type WorldIO;

		internal static MethodInfo WorldIO_Load;

		internal static MethodInfo WorldHooks_SetupWorld;
	}


}
