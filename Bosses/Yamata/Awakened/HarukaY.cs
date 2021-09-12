using System;
using AAModEXAI.Bosses.AH.Haruka;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
	[AutoloadBossHead]
	public class HarukaY : Haruka
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haruka Yamata");
			Main.npcFrameCount[npc.type] = 28;
			NPCID.Sets.TechnicallyABoss[npc.type] = true;
		}

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
			npc.GetGlobalNPC<CalamityGlobalNPC>().CalamityDR = 0.1f;
		}

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
				int npcid = BaseAI.GetNPC(npc.Center, mod.NPCType("YamataA"), -1f, null);
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
			if (npc2 == null || npc2.life <= 0 || !npc2.active || npc2.type != mod.NPCType("YamataA"))
			{
				BaseAI.KillNPCWithLoot(npc);
				return;
			}
		}

		public override bool CheckActive()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<YamataAHead>());
		}

		public override void NPCLoot()
		{
			npc.value = 0f;
			npc.boss = false;
			int num = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HarukaVanish>(), 0, 0f, 0f, 0f, 0f, 255);
			Main.npc[num].velocity = npc.velocity;
			if (!NPC.AnyNPCs(ModContent.NPCType<YamataAHead>()))
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					AAModEXAI.Chat(AAMod.Lang.BossChat("HarukaY1"), new Color(72, 78, 117), true);
				}
				return;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				AAModEXAI.Chat(AAMod.Lang.BossChat("HarukaY2"), new Color(72, 78, 117), true);
			}
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = 0;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)((float)npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)((float)npc.damage * 0.9f);
		}

		public int body = -1;
	}
}
