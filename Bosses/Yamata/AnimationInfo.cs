using System;

namespace AAMod.NPCs.Bosses.Yamata
{
	// Token: 0x020003E6 RID: 998
	public class AnimationInfo
	{
		// Token: 0x06001706 RID: 5894 RVA: 0x000FDCEC File Offset: 0x000FBEEC
		public AnimationInfo(int type, float aMult = 1f)
		{
			this.animType = type;
			this.animMult = aMult;
		}

		// Token: 0x040003F7 RID: 1015
		public int animType;

		// Token: 0x040003F8 RID: 1016
		public float movementRatio;

		// Token: 0x040003F9 RID: 1017
		public float movementRate = 0.01f;

		// Token: 0x040003FA RID: 1018
		public float animMult = 1f;

		// Token: 0x040003FB RID: 1019
		public float halfPI = 1.5707964f;

		// Token: 0x040003FC RID: 1020
		public bool[] fired = new bool[4];

		// Token: 0x040003FD RID: 1021
		public float[] hitRatios;

		// Token: 0x040003FE RID: 1022
		public bool flatJoint;
	}
}
