using System;

namespace AAMod.NPCs.Bosses.Yamata.Awakened
{
	// Token: 0x020003FE RID: 1022
	public class AnimationInfo
	{
		// Token: 0x060017BA RID: 6074 RVA: 0x00106F98 File Offset: 0x00105198
		public AnimationInfo(int type, float aMult = 1f)
		{
			this.animType = type;
			this.animMult = aMult;
		}

		// Token: 0x04000468 RID: 1128
		public int animType;

		// Token: 0x04000469 RID: 1129
		public float movementRatio;

		// Token: 0x0400046A RID: 1130
		public float movementRate = 0.01f;

		// Token: 0x0400046B RID: 1131
		public float animMult = 1f;

		// Token: 0x0400046C RID: 1132
		public float halfPI = 1.5707964f;

		// Token: 0x0400046D RID: 1133
		public bool[] fired = new bool[4];

		// Token: 0x0400046E RID: 1134
		public float[] hitRatios;

		// Token: 0x0400046F RID: 1135
		public bool flatJoint;
	}
}
