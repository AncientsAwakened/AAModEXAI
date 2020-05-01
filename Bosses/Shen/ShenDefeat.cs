using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen
{
	// Token: 0x02000429 RID: 1065
	public class ShenDefeat : ModNPC
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x0011C586 File Offset: 0x0011A786
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/ShenSpawn";
			}
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0011D0AE File Offset: 0x0011B2AE
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Discord's Defeat");
			NPCID.Sets.TechnicallyABoss[base.npc.type] = true;
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0011D0D4 File Offset: 0x0011B2D4
		public override void SetDefaults()
		{
			base.npc.height = 100;
			base.npc.width = 444;
			base.npc.friendly = false;
			base.npc.lifeMax = 1;
			base.npc.dontTakeDamage = true;
			base.npc.noGravity = true;
			base.npc.aiStyle = -1;
			base.npc.alpha = 255;
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/silence");
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0011D188 File Offset: 0x0011B388
		public override void AI()
		{
			if (base.npc.ai[1] > 240f)
			{
				base.npc.life = 0;
				base.npc.netUpdate = true;
				return;
			}
			base.npc.ai[1] += 1f;
			base.npc.ai[0] += 1f;
			if (base.npc.ai[0] > 4f)
			{
				base.npc.ai[0] = 0f;
				Main.PlaySound(new LegacySoundStyle(2, 124, 0), -1, -1);
				for (int i = 0; i < 3; i++)
				{
					Projectile.NewProjectile(new Vector2(base.npc.position.X + (float)Main.rand.Next(0, 444), base.npc.position.Y - (float)Main.rand.Next(0, 100)), Vector2.Zero, ModContent.ProjectileType<ShenDeathBoom>(), 0, 0f, Main.myPlayer, (float)Main.rand.Next(3), 0f);
				}
			}
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0011D2B0 File Offset: 0x0011B4B0
		public override void NPCLoot()
		{
			int num = AAWorld.downedShen ? 0 : 1;
			NPC.NewNPC((int)base.npc.position.X, (int)base.npc.position.Y, ModContent.NPCType<ShenDeath>(), 0, (float)num, 0f, 0f, 0f, 255);
		}
	}
}
