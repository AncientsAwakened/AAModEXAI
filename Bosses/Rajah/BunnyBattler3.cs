using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200044C RID: 1100
	public class BunnyBattler3 : BunnyBattler
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x0012BC22 File Offset: 0x00129E22
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBattler";
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0012C105 File Offset: 0x0012A305
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 140;
			base.npc.defense = 60;
			base.npc.lifeMax = 900;
		}
	}
}
