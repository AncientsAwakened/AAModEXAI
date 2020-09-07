using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using AAModEXAI.Bosses.AH.Ashe;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
	[AutoloadBossHead]
	public class AsheA : Ashe
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Ashe Akuma");
			Main.npcFrameCount[npc.type] = 24;
		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 80;
			npc.damage = 100;
			npc.defense = 40;
			npc.lifeMax = 100000;
			npc.value = (float)Item.sellPrice(0, 0, 0, 0);
			for (int i = 0; i < npc.buffImmune.Length; i++)
			{
				npc.buffImmune[i] = true;
			}
			npc.knockBackResist = 0f;
			npc.knockBackResist = 0f;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.GetGlobalNPC<CalamityGlobalNPC>().CalamityDR = 0.1f;
		}
		public override bool PreAI()
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<AkumaA>()))
			{
				int num = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<AsheVanish>(), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num].velocity = npc.velocity;
				npc.active = false;
				npc.netUpdate = true;
			}
			return true;
		}
		public override bool CheckActive()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<AkumaA>());
		}
		public override void NPCLoot()
		{
			npc.value = 0f;
			npc.boss = false;
			int num = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<AsheVanish>(), 0, 0f, 0f, 0f, 0f, 255);
			Main.npc[num].velocity = npc.velocity;
			if (!NPC.AnyNPCs(ModContent.NPCType<AkumaA>()))
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					AAModEXAI.Chat(AAMod.Lang.BossChat("AkumaAAshe1"), new Color(102, 20, 48), true);
				}
				return;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				AAModEXAI.Chat(AAMod.Lang.BossChat("AkumaAAshe2"), new Color(102, 20, 48), true);
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = 0;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)((float)npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)((float)npc.damage * 0.6f);
		}
		public int body = -1;
	}
}
