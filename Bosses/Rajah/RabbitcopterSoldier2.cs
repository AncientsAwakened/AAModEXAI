using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200045A RID: 1114
	public class RabbitcopterSoldier2 : RabbitcopterSoldier
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0012E734 File Offset: 0x0012C934
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/RabbitcopterSoldier";
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0012E775 File Offset: 0x0012C975
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 130;
			base.npc.defense = 40;
			base.npc.lifeMax = 650;
		}
	}
}
