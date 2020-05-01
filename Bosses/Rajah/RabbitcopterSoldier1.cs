using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000459 RID: 1113
	public class RabbitcopterSoldier1 : RabbitcopterSoldier
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0012E734 File Offset: 0x0012C934
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/RabbitcopterSoldier";
			}
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0012E73B File Offset: 0x0012C93B
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 110;
			base.npc.defense = 30;
			base.npc.lifeMax = 500;
		}
	}
}
