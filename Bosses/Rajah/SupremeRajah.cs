using System;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000467 RID: 1127
	[AutoloadBossHead]
	public class SupremeRajah : Rajah
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x001324A7 File Offset: 0x001306A7
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Supreme/SupremeRajah";
			}
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x001324AE File Offset: 0x001306AE
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Rajah Rabbit; Champion of the Innocent");
			Main.npcFrameCount[base.npc.type] = 8;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x001324D4 File Offset: 0x001306D4
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 310;
			base.npc.defense = 0;
			base.npc.lifeMax = 2600000;
			base.npc.life = 2600000;
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/SupremeRajah");
			this.bossBag = base.mod.ItemType("RajahCache");
			this.isSupreme = true;
			base.npc.value = (float)Item.sellPrice(3, 0, 0, 0);
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0013256D File Offset: 0x0013076D
		public override string BossHeadTexture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/SupremeRajah_Head_Boss";
			}
		}
	}
}
