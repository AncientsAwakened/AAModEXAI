using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen
{
	// Token: 0x0200042A RID: 1066
	public class ShenDeath : ModNPC
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x0011C586 File Offset: 0x0011A786
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/ShenSpawn";
			}
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0011D30D File Offset: 0x0011B50D
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Discord's Death");
			NPCID.Sets.TechnicallyABoss[base.npc.type] = true;
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0011D334 File Offset: 0x0011B534
		public override void SetDefaults()
		{
			base.npc.width = 100;
			base.npc.height = 100;
			base.npc.friendly = false;
			base.npc.lifeMax = 1;
			base.npc.dontTakeDamage = true;
			base.npc.noGravity = true;
			base.npc.aiStyle = -1;
			base.npc.alpha = 255;
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/ChaosSissy");
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0011D3E8 File Offset: 0x0011B5E8
		public override void AI()
		{
			if (AAConfigClient.Instance.NoBossDialogue)
			{
				AAWorld.downedShen = true;
				base.npc.active = false;
				base.npc.netUpdate = true;
			}
			base.npc.TargetClosest(true);
			Player player = Main.player[base.npc.target];
			base.npc.Center = player.Center;
			base.npc.ai[1] += 1f;
			if (base.npc.ai[0] == 0f)
			{
				if (base.npc.ai[1] == 180f)
				{
					BaseUtility.Chat(Lang.BossChat("ShenDeath1"), new Color(180, 41, 32), true);
				}
				if (base.npc.ai[1] == 360f)
				{
					BaseUtility.Chat(Lang.BossChat("ShenDeath2"), new Color(45, 46, 70), true);
				}
				if (base.npc.ai[1] == 540f)
				{
					BaseUtility.Chat(((Main.netMode != 0) ? Lang.BossChat("ShenDeath3") : player.name) + Lang.BossChat("ShenDeath4"), new Color(180, 41, 32), true);
				}
				if (base.npc.ai[1] == 720f)
				{
					BaseUtility.Chat(Lang.BossChat("ShenDeath5"), new Color(45, 46, 70), true);
				}
				if (base.npc.ai[1] == 899f)
				{
					BaseUtility.Chat(Lang.BossChat("ShenDeath6"), new Color(45, 46, 70), true);
					BaseUtility.Chat(Lang.BossChat("ShenDeath6"), new Color(180, 41, 32), true);
				}
				if (base.npc.ai[1] >= 900f)
				{
					AAWorld.downedShen = true;
					base.npc.active = false;
					base.npc.netUpdate = true;
				}
				return;
			}
			if (base.npc.ai[1] == 180f)
			{
				BaseUtility.Chat(Lang.BossChat("ShenDeath7"), new Color(45, 46, 70), true);
			}
			if (base.npc.ai[1] == 360f)
			{
				BaseUtility.Chat(Lang.BossChat("ShenDeath8"), new Color(180, 41, 32), true);
			}
			if (base.npc.ai[1] == 540f)
			{
				string str = (Main.netMode != 0) ? Lang.BossChat("ShenDeath9") : (player.Male ? Lang.BossChat("boy") : Lang.BossChat("girl"));
				BaseUtility.Chat(Lang.BossChat("ShenDeath10") + str + Lang.BossChat("ShenDeath11"), new Color(45, 46, 70), true);
			}
			if (base.npc.ai[1] == 720f)
			{
				BaseUtility.Chat(Lang.BossChat("ShenDeath12"), new Color(180, 41, 32), true);
			}
			if (base.npc.ai[1] == 899f && Main.netMode != 1)
			{
				BaseUtility.Chat(Lang.BossChat("ShenDeath13"), new Color(45, 46, 70), true);
				BaseUtility.Chat(Lang.BossChat("ShenDeath13"), new Color(180, 41, 32), true);
			}
			if (base.npc.ai[1] >= 900f)
			{
				base.npc.active = false;
				base.npc.netUpdate = true;
			}
		}
	}
}
