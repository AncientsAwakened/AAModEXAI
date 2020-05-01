using System;
using AAMod.Dusts;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen
{
	// Token: 0x0200042B RID: 1067
	public class ShenTransition : ModNPC
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x0011D764 File Offset: 0x0011B964
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/ShenTransition";
			}
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0011D76B File Offset: 0x0011B96B
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Discordian Awakening");
			NPCID.Sets.TechnicallyABoss[base.npc.type] = true;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0011D790 File Offset: 0x0011B990
		public override void SetDefaults()
		{
			base.npc.width = 100;
			base.npc.height = 100;
			base.npc.friendly = false;
			base.npc.alpha = 255;
			base.npc.lifeMax = 10000000;
			base.npc.dontTakeDamage = true;
			base.npc.noGravity = true;
			base.npc.aiStyle = -1;
			base.npc.timeLeft = 10;
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0011D83C File Offset: 0x0011BA3C
		public override void AI()
		{
			base.npc.TargetClosest(true);
			Player player = Main.player[base.npc.target];
			base.npc.Center = player.Center - new Vector2(0f, 300f);
			base.npc.ai[0] += 1f;
			if (base.npc.timeLeft <= 10)
			{
				base.npc.timeLeft = 10;
			}
			if (base.npc.alpha < 255 && base.npc.ai[0] > 350f)
			{
				this.music = base.mod.GetSoundSlot(51, "Sounds/Music/ShenA");
				for (int i = 0; i < 8; i++)
				{
					Vector2 center = base.npc.Center;
					Dust dust = Main.dust[Dust.NewDust(center, 20, 20, ModContent.DustType<Discord>(), 0f, 0f, 0, default(Color), 1f)];
					dust.noGravity = false;
					dust.scale *= 1.3f;
					dust.velocity.Y = dust.velocity.Y - 6f;
				}
			}
			if (Main.netMode != 1)
			{
				if (base.npc.ai[0] == 375f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("ShenTransition1"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
					}
					base.npc.netUpdate = true;
				}
				if (base.npc.ai[0] == 475f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("ShenTransition2"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
					}
					base.npc.netUpdate = true;
				}
				if (base.npc.ai[0] == 600f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("ShenTransition3"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
					}
					base.npc.netUpdate = true;
				}
				if (base.npc.ai[0] == 820f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("ShenTransition4"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
					}
					base.npc.netUpdate = true;
				}
				if (base.npc.ai[0] == 960f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("ShenTransition5"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
					}
					base.npc.netUpdate = true;
				}
				if (base.npc.ai[0] >= 1100f)
				{
					base.npc.alpha -= 5;
				}
				if (base.npc.ai[0] == 1100f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("ShenTransition6"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
					}
					base.npc.netUpdate = true;
				}
				if (base.npc.ai[0] >= 1400f)
				{
					this.SummonShen();
					base.npc.active = false;
					base.npc.netUpdate = true;
				}
			}
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0011DC18 File Offset: 0x0011BE18
		public override bool PreAI()
		{
			if (AAConfigClient.Instance.NoBossDialogue)
			{
				base.npc.TargetClosest(true);
				Player player = Main.player[base.npc.target];
				base.npc.Center = player.Center - new Vector2(0f, 300f);
				base.npc.ai[0] += 1f;
				if (base.npc.alpha < 255 && base.npc.ai[0] > 200f)
				{
					this.music = base.mod.GetSoundSlot(51, "Sounds/Music/ShenA");
					for (int i = 0; i < 8; i++)
					{
						Vector2 center = base.npc.Center;
						Dust dust = Main.dust[Dust.NewDust(center, 20, 20, ModContent.DustType<Discord>(), 0f, 0f, 0, default(Color), 1f)];
						dust.noGravity = false;
						dust.scale *= 1.3f;
						dust.velocity.Y = dust.velocity.Y - 6f;
					}
				}
				if (base.npc.ai[0] >= 400f)
				{
					base.npc.alpha -= 5;
				}
				if (base.npc.ai[0] >= 600f)
				{
					this.SummonShen();
					base.npc.active = false;
					base.npc.netUpdate = true;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0011DDA4 File Offset: 0x0011BFA4
		public void SummonShen()
		{
			Player player = Main.player[base.npc.target];
			if (Main.netMode != 1)
			{
				BaseUtility.Chat(Lang.BossChat("ShenTransition7"), Color.Magenta.R, Color.Magenta.G, Color.Magenta.B, true);
			}
			if (Main.netMode != 1)
			{
				BaseUtility.Chat(Lang.BossChat("ShenTransition8"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B, true);
			}
			int num = Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, 0f, 0f, base.mod.ProjectileType("ShockwaveBoom"), 0, 1f, Main.myPlayer, 0f, 0f);
			Main.projectile[num].Center = base.npc.Center;
			AAModGlobalNPC.SpawnBoss(player, base.mod.NPCType("ShenA"), false, base.npc.Center, "Shen Awakened", false);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0011CF56 File Offset: 0x0011B156
		public Color GetColorAlpha()
		{
			return new Color(233, 0, 233) * ((float)Main.mouseTextColor / 255f);
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0011DED4 File Offset: 0x0011C0D4
		public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
			if (this.auraDirection)
			{
				this.auraPercent += 0.1f;
				this.auraDirection = (this.auraPercent < 1f);
			}
			else
			{
				this.auraPercent -= 0.1f;
				this.auraDirection = (this.auraPercent <= 0f);
			}
			if (base.npc.alpha <= 0)
			{
				BaseDrawing.DrawTexture(sb, Main.npcTexture[base.npc.type], 0, base.npc, new Color?(dColor), false, default(Vector2));
				BaseDrawing.DrawAura(sb, Main.npcTexture[base.npc.type], 0, base.npc, this.auraPercent, 1f, 0f, 0f, new Color?(this.GetColorAlpha()));
				BaseDrawing.DrawTexture(sb, Main.npcTexture[base.npc.type], 0, base.npc, new Color?(this.GetColorAlpha()), false, default(Vector2));
				return false;
			}
			return true;
		}

		// Token: 0x040004E7 RID: 1255
		public float auraPercent;

		// Token: 0x040004E8 RID: 1256
		public bool auraDirection = true;

		// Token: 0x040004E9 RID: 1257
		public bool saythelinezero;
	}
}
