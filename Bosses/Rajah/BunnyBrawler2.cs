using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000450 RID: 1104
	public class BunnyBrawler2 : BunnyBrawler
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x0012C52A File Offset: 0x0012A72A
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBrawler";
			}
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0012C56B File Offset: 0x0012A76B
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 140;
			base.npc.defense = 70;
			base.npc.lifeMax = 800;
		}
	}
}
