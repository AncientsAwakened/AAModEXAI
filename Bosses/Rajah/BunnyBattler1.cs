using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200044A RID: 1098
	public class BunnyBattler1 : BunnyBattler
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0012BC22 File Offset: 0x00129E22
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBattler";
			}
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0012C096 File Offset: 0x0012A296
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 100;
			base.npc.defense = 40;
			base.npc.lifeMax = 400;
		}
	}
}
