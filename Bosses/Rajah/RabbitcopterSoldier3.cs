using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200045B RID: 1115
	public class RabbitcopterSoldier3 : RabbitcopterSoldier
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x0012E734 File Offset: 0x0012C934
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/RabbitcopterSoldier";
			}
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0012E7AA File Offset: 0x0012C9AA
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 150;
			base.npc.defense = 50;
			base.npc.lifeMax = 750;
		}
	}
}
