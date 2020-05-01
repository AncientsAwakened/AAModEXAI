using System;
using Microsoft.Xna.Framework;

namespace AAMod.NPCs.Bosses.Yamata
{
	// Token: 0x020003E7 RID: 999
	public class LimbInfo
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x000FDD3C File Offset: 0x000FBF3C
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x000FDD8A File Offset: 0x000FBF8A
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

		// Token: 0x040003FF RID: 1023
		public int limbType;

		// Token: 0x04000400 RID: 1024
		public Vector2 position;

		// Token: 0x04000401 RID: 1025
		public Vector2 oldPosition;

		// Token: 0x04000402 RID: 1026
		public Rectangle Hitbox;

		// Token: 0x04000403 RID: 1027
		public float rotation;

		// Token: 0x04000404 RID: 1028
		public float movementRatio;

		// Token: 0x04000405 RID: 1029
		public AnimationInfo overrideAnimation;

		// Token: 0x04000406 RID: 1030
		public Yamata yamata;
	}
}
