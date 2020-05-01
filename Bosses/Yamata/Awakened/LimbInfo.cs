using System;
using Microsoft.Xna.Framework;

namespace AAMod.NPCs.Bosses.Yamata.Awakened
{
	// Token: 0x020003FF RID: 1023
	public class LimbInfo
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x00106FE8 File Offset: 0x001051E8
		// (set) Token: 0x060017BC RID: 6076 RVA: 0x00107036 File Offset: 0x00105236
		public Vector2 Center
		{
			get
			{
				return new Vector2(this.position.X + (float)this.Hitbox.Width * 0.5f, this.position.Y + (float)this.Hitbox.Height * 0.5f);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.Hitbox.Width * 0.5f, value.Y - (float)this.Hitbox.Height * 0.5f);
			}
		}

		// Token: 0x04000470 RID: 1136
		public int limbType;

		// Token: 0x04000471 RID: 1137
		public Vector2 position;

		// Token: 0x04000472 RID: 1138
		public Vector2 oldPosition;

		// Token: 0x04000473 RID: 1139
		public Rectangle Hitbox;

		// Token: 0x04000474 RID: 1140
		public float rotation;

		// Token: 0x04000475 RID: 1141
		public float movementRatio;

		// Token: 0x04000476 RID: 1142
		public AnimationInfo overrideAnimation;

		// Token: 0x04000477 RID: 1143
		public YamataA yamataA;
	}
}
