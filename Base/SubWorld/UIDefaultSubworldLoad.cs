using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace AAModEXAI.Base.SubWorld
{
	public class UIDefaultSubworldLoad : UIState
	{
		public UIDefaultSubworldLoad()
		{
			this._progressBar.HAlign = 0.5f;
			this._progressBar.VAlign = 0.5f;
			this._progressBar.Recalculate();
			this._progressMessage.CopyStyle(this._progressBar);
			this._progressMessage.Recalculate();
			base.Append(this._progressBar);
			base.Append(this._progressMessage);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (SLWorld.progress != null)
			{
				this._progressBar.SetProgress(SLWorld.progress.TotalProgress, SLWorld.progress.Value);
				this._progressMessage.Text = SLWorld.progress.Message;
				return;
			}
			this._progressMessage.Text = Main.statusText;
		}

		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			if (SLWorld.progress != null)
			{
				this._progressBar.Draw(spriteBatch);
				this._progressMessage.Top.Pixels = -70f;
			}
			else
			{
				this._progressMessage.Top.Pixels = 0f;
			}
			this._progressMessage.Recalculate();
			this._progressMessage.Draw(spriteBatch);
		}

		private UIGenProgressBar _progressBar = new UIGenProgressBar();

		private UIHeader _progressMessage = new UIHeader();
	}
}
