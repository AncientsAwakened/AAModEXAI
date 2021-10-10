using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;

namespace AAModEXAI.Base.SubWorld
{
	public abstract class Subworld
	{
		public string Name
		{
			get
			{
				return Language.GetTextValue("Mods." + this.mod.Name + ".SubworldName." + this.id.Substring(this.mod.Name.Length + 1));
			}
		}

		public string Current
		{
			get
			{
				if (!SLWorld.subworld)
				{
					return null;
				}
				return SLWorld.currentSubworld.id;
			}
		}

		public abstract int width { get; }

		public abstract int height { get; }

		public abstract List<GenPass> tasks { get; }

		public virtual void Load()
		{
		}

		public virtual void Unload()
		{
		}

		public virtual ModWorld modWorld
		{
			get
			{
				return null;
			}
		}

		public virtual bool saveSubworld
		{
			get
			{
				return false;
			}
		}

		public virtual bool disablePlayerSaving
		{
			get
			{
				return false;
			}
		}

		public virtual bool saveModData
		{
			get
			{
				return false;
			}
		}

		public virtual bool noWorldUpdate
		{
			get
			{
				return true;
			}
		}

		public virtual UserInterface loadingUI
		{
			get
			{
				return Main.MenuUI;
			}
		}

		public virtual UIState loadingUIState
		{
			get
			{
				return new UIDefaultSubworldLoad();
			}
		}

		public virtual UIState votingUI
		{
			get
			{
				return new UIDefaultVoting();
			}
		}

		public virtual ushort votingDuration
		{
			get
			{
				return 1800;
			}
		}

		public virtual void OnVotedFor()
		{
			SLWorld.currentSubworld = this;
			ThreadPool.QueueUserWorkItem(new WaitCallback(SLWorld.ExitWorldCallBack), true);
			ModPacket packet = AAModEXAI.instance.GetPacket(256);
			packet.Write(1);
			packet.Write(SLWorld.votingFor);
			packet.Send(-1, -1);
		}

		internal static void BeginEntering(int index)
		{
			SLWorld.currentSubworld = Subworld.subworlds[index];
			ThreadPool.QueueUserWorkItem(new WaitCallback(SLWorld.ExitWorldCallBack), true);
			if (Main.netMode == NetmodeID.Server)
			{
				ModPacket packet = AAModEXAI.instance.GetPacket(256);
				packet.Write(1);
				packet.Write(index);
				packet.Send(-1, -1);
			}
		}

		internal static void BeginVoting(ushort votingFor)
		{
			SLWorld.votingFor = votingFor;
			SLWorld.votingToLeave = false;
			SLWorld.votingTimer = 1800;
			ModPacket packet = AAModEXAI.instance.GetPacket(256);
			packet.Write(7);
			packet.Write(votingFor);
			packet.Write(false);
			packet.Send(-1, -1);
		}

		internal static void SendEnterPacket(int i, bool noVote)
		{
			ModPacket packet = AAModEXAI.instance.GetPacket(256);
			if (noVote)
			{
				packet.Write(0);
			}
			else
			{
				packet.Write(5);
			}
			packet.Write((ushort)i);
			packet.Send(-1, -1);
		}

		public static bool Enter(string id, bool noVote = false)
		{
			if (!SLWorld.loading)
			{
				for (int i = 0; i < Subworld.subworlds.Count; i++)
				{
					if (Subworld.subworlds[i].id == id)
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							Subworld.SendEnterPacket(i, noVote);
						}
						else if (Main.netMode == NetmodeID.Server && !noVote)
						{
							Subworld.BeginVoting((ushort)i);
						}
						else
						{
							Subworld.BeginEntering(i);
						}
						return true;
					}
				}
			}
			return false;
		}

		public static bool Enter<T>(bool noVote = false) where T : Subworld
		{
			if (!SLWorld.loading)
			{
				for (int i = 0; i < Subworld.subworlds.Count; i++)
				{
					if (Subworld.subworlds[i].GetType() == typeof(T))
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							Subworld.SendEnterPacket(i, noVote);
						}
						else if (Main.netMode == NetmodeID.Server && !noVote)
						{
							Subworld.BeginVoting((ushort)i);
						}
						else
						{
							Subworld.BeginEntering(i);
						}
						return true;
					}
				}
			}
			return false;
		}

		public static void VoteFor(UIMouseEvent evt, UIElement listeningElement)
		{
			ModPacket packet = AAModEXAI.instance.GetPacket(256);
			if (!SLWorld.votingToLeave)
			{
				packet.Write(5);
				packet.Write(SLWorld.votingFor);
			}
			else
			{
				packet.Write(6);
			}
			packet.Send(-1, -1);
			AAModEXAI.instance.SubWorldInterface.SetState(null);
		}

		public static void VoteAgainst(UIMouseEvent evt, UIElement listeningElement)
		{
			ModPacket packet = AAModEXAI.instance.GetPacket(256);
			if (!SLWorld.votingToLeave)
			{
				packet.Write(6);
			}
			else
			{
				packet.Write(5);
				packet.Write(0);
			}
			packet.Send(-1, -1);
			AAModEXAI.instance.SubWorldInterface.SetState(null);
		}

		public static void Exit(bool noVote = false)
		{
			if (SLWorld.subworld && !SLWorld.loading)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					ModPacket packet = AAModEXAI.instance.GetPacket(256);
					if (noVote)
					{
						packet.Write(3);
					}
					else
					{
						packet.Write(6);
					}
					packet.Send(-1, -1);
					return;
				}
				if (Main.netMode == NetmodeID.Server && !noVote)
				{
					SLWorld.votingToLeave = true;
					SLWorld.votingTimer = 1800;
					ModPacket packet2 = AAModEXAI.instance.GetPacket(256);
					packet2.Write(7);
					packet2.Write(0);
					packet2.Write(true);
					packet2.Send(-1, -1);
					return;
				}
				ThreadPool.QueueUserWorkItem(new WaitCallback(SLWorld.ExitWorldCallBack), false);
				if (Main.netMode == NetmodeID.Server)
				{
					ModPacket packet3 = AAModEXAI.instance.GetPacket(256);
					packet3.Write(4);
					packet3.Send(-1, -1);
				}
			}
		}

		public static bool IsActive(string id)
		{
			return SLWorld.subworld && SLWorld.currentSubworld.id == id;
		}

		public static bool IsActive<T>() where T : Subworld
		{
			return SLWorld.subworld && SLWorld.currentSubworld.GetType() == typeof(T);
		}

		public static bool AnyActive(Mod mod)
		{
			return SLWorld.subworld && SLWorld.currentSubworld.mod == mod;
		}

		public static bool AnyActive<T>() where T : Mod
		{
			return SLWorld.subworld && SLWorld.currentSubworld.mod.GetType() == typeof(T);
		}

		internal Mod mod;

		internal string id;

		internal static List<Subworld> subworlds;
	}
}
