using System;
using AAModEXAI.Bosses.AH.Haruka;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
	// Token: 0x020003F7 RID: 1015
	[AutoloadBossHead]
	public class HarukaY : Haruka
	{
		// Token: 0x0600177C RID: 6012 RVA: 0x00103757 File Offset: 0x00101957
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Haruka Yamata");
			Main.npcFrameCount[npc.type] = 28;
			NPCID.Sets.TechnicallyABoss[npc.type] = true;
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00103790 File Offset: 0x00101990
		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 60;
			npc.friendly = false;
			npc.damage = 100;
			npc.defense = 50;
			npc.lifeMax = 90000;
			npc.HitSound = SoundID.NPCHit1;
			npc.knockBackResist = 0f;
			for (int i = 0; i < npc.buffImmune.Length; i++)
			{
				npc.buffImmune[i] = true;
			}
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.boss = false;
			npc.value = (float)Item.sellPrice(0, 0, 0, 0);
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x001038B0 File Offset: 0x00101AB0
		public override void PostAI()
		{
			Player player = Main.player[npc.target];
			if (internalAI[0] != Haruka.AISTATE_SPIN)
			{
				if (player.Center.X > npc.Center.X)
				{
					if (pos == -250f)
					{
						pos = 250f;
					}
					npc.direction = 1;
				}
				else
				{
					if (pos == 250f)
					{
						pos = -250f;
					}
					npc.direction = -1;
				}
			}
			else
			{
				npc.direction = ((npc.velocity.X > 0f) ? 1 : -1);
			}
			if (!NPC.AnyNPCs(ModContent.NPCType<YamataAHead>()))
			{
				npc.life = 0;
			}
			if (body == -1)
			{
				int npcid = BaseAI.GetNPC(npc.Center, base.mod.NPCType("YamataA"), -1f, null);
				if (npcid >= 0)
				{
					body = npcid;
				}
			}
			if (body == -1)
			{
				return;
			}
			NPC npc2 = Main.npc[body];
			if (npc2 == null || npc2.life <= 0 || !npc2.active || npc2.type != base.mod.NPCType("YamataA"))
			{
				BaseAI.KillNPCWithLoot(npc);
				return;
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00103A37 File Offset: 0x00101C37
		public override bool CheckActive()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<YamataAHead>());
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00103A48 File Offset: 0x00101C48
		public override void NPCLoot()
		{
			npc.value = 0f;
			npc.boss = false;
			int num = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HarukaVanish>(), 0, 0f, 0f, 0f, 0f, 255);
			Main.npc[num].velocity = npc.velocity;
			if (!NPC.AnyNPCs(ModContent.NPCType<YamataAHead>()))
			{
				if (Main.netMode != 1)
				{
					AAModEXAI.Chat(AAMod.Lang.BossChat("HarukaY1"), new Color(72, 78, 117), true);
				}
				return;
			}
			if (Main.netMode != 1)
			{
				AAModEXAI.Chat(AAMod.Lang.BossChat("HarukaY2"), new Color(72, 78, 117), true);
			}
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00103B22 File Offset: 0x00101D22
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = 0;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00103B27 File Offset: 0x00101D27
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)((float)npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)((float)npc.damage * 0.9f);
		}

		// Token: 0x0400043B RID: 1083
		public int body = -1;
	}
}
