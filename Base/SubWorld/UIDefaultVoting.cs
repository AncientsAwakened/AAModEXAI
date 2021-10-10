using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

namespace AAModEXAI.Base.SubWorld
{
	public class UIDefaultVoting : UIState
	{
		public UIDefaultVoting()
		{
			string text;
			if (SLWorld.votingToLeave)
			{
				text = Language.GetTextValue("Mods.SubworldLibrary.VotingToLeave");
			}
			else
			{
				text = string.Format(Language.GetTextValue("Mods.SubworldLibrary.Voting"), Subworld.subworlds[(int)SLWorld.votingFor].Name);
			}
			UIText uitext = new UIText(text, 1f, false);
			uitext.HAlign = 0.5f;
			uitext.Top.Set(96f, 0f);
			base.Append(uitext);
			UIElement uielement = new UIElement();
			uielement.HAlign = 0.5f;
			uielement.Width.Set(130f, 0f);
			uielement.Height.Set(24f, 0f);
			uielement.Top.Set(126f, 0f);
			base.Append(uielement);
			this.timer = new UIText("", 1f, false);
			this.timer.HAlign = 0.5f;
			this.timer.VAlign = 0.5f;
			uielement.Append(this.timer);
			UIImage uiimage = new UIImage(ModContent.GetTexture("SubworldLibrary/Upvote"));
			uiimage.VAlign = 0.5f;
			uiimage.OnClick += new UIElement.MouseEvent(Subworld.VoteFor);
			uielement.Append(uiimage);
			UIImage uiimage2 = new UIImage(ModContent.GetTexture("SubworldLibrary/Downvote"));
			uiimage2.HAlign = 1f;
			uiimage2.VAlign = 0.5f;
			uiimage2.OnClick += new UIElement.MouseEvent(Subworld.VoteAgainst);
			uielement.Append(uiimage2);
		}

		public override void OnActivate()
		{
			Main.PlaySound(24, -1, -1, 1, 1f, 0f);
		}

		public override void OnDeactivate()
		{
			Main.PlaySound(11, -1, -1, 1, 1f, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.timer.SetText(((int)((SLWorld.votingTimer + 59) / 60)).ToString());
			if (SLWorld.votingTimer > 0)
			{
				SLWorld.votingTimer -= 1;
			}
			if (SLWorld.votingTimer == 0)
			{
				SubworldLibrary.Instance.UI.SetState(null);
			}
		}

		private UIText timer;
	}
}
