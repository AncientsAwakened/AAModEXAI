using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200044B RID: 1099
	public class BunnyBattler2 : BunnyBattler
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x0012BC22 File Offset: 0x00129E22
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBattler";
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0012C0D0 File Offset: 0x0012A2D0
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 130;
			base.npc.defense = 50;
			base.npc.lifeMax = 600;
		}
	}
}
