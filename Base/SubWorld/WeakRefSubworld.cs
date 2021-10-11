using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;

namespace AAModEXAI
{
	public class WeakRefSubworld : Subworld
	{
		public override int width { get; }

		public override int height { get; }

		public override ModWorld modWorld { get; }

		public override List<GenPass> tasks { get; }

		public override void Load()
		{
			this._Load();
		}

		public override void Unload()
		{
			this._Unload();
		}

		public override bool saveSubworld { get; }

		public override bool disablePlayerSaving { get; }

		public override bool saveModData { get; }

		public override bool noWorldUpdate { get; }

		public override UserInterface loadingUI { get; }

		public override UIState loadingUIState
		{
			get
			{
				return this._loadingUIState();
			}
		}

		public override UIState votingUI
		{
			get
			{
				return this._votingUI();
			}
		}

		public override ushort votingDuration { get; }

		public override void OnVotedFor()
		{
			this._OnVotedFor();
		}

		public WeakRefSubworld(Mod mod, string id, int width, int height, List<GenPass> tasks, Action load = null, Action unload = null, ModWorld modWorld = null, bool saveSubworld = false, bool disablePlayerSaving = false, bool saveModData = false, bool noWorldUpdate = true, UserInterface loadingUI = null, Func<UIState> loadingUIState = null, Func<UIState> votingUI = null, ushort votingDuration = 1800, Action onVotedFor = null)
		{
			this.mod = mod;
			this.id = id;
			this.width = width;
			this.height = height;
			this.tasks = tasks;
			this._Load = load;
			if (load == null)
			{
				this._Load = new Action(base.Load);
			}
			this._Unload = unload;
			if (unload == null)
			{
				this._Unload = new Action(base.Unload);
			}
			this.modWorld = modWorld;
			this.saveSubworld = saveSubworld;
			this.disablePlayerSaving = disablePlayerSaving;
			this.saveModData = saveModData;
			this.noWorldUpdate = noWorldUpdate;
			this.loadingUI = loadingUI;
			if (loadingUI == null)
			{
				this.loadingUI = Main.MenuUI;
			}
			this._loadingUIState = loadingUIState;
			if (loadingUIState == null)
			{
				this._loadingUIState = (() => new UIDefaultSubworldLoad());
			}
			this._votingUI = votingUI;
			if (votingUI == null)
			{
				this._votingUI = (() => new UIDefaultVoting());
			}
			this.votingDuration = votingDuration;
			this._OnVotedFor = onVotedFor;
			if (onVotedFor == null)
			{
				this._OnVotedFor = new Action(base.OnVotedFor);
			}
		}

		private Action _Load;

		private Action _Unload;

		private Func<UIState> _loadingUIState;

		private Func<UIState> _votingUI;

		private Action _OnVotedFor;
	}
}
