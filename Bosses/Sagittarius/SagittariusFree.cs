using System;
using System.IO;
using AAMod.Dusts;
using AAMod.Items.Materials;
using AAMod.NPCs.Bosses.Zero;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Sagittarius
{
	// Token: 0x02000445 RID: 1093
	[AutoloadBossHead]
	public class SagittariusFree : Sagittarius
	{
		// Token: 0x060019FA RID: 6650 RVA: 0x001299D1 File Offset: 0x00127BD1
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sagittarius-A");
			Main.npcFrameCount[base.npc.type] = 5;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x001299F8 File Offset: 0x00127BF8
		public override void SetDefaults()
		{
			base.npc.lifeMax = 6000;
			base.npc.boss = true;
			base.npc.defense = 0;
			base.npc.damage = 40;
			base.npc.width = 74;
			base.npc.height = 70;
			base.npc.aiStyle = -1;
			base.npc.value = (float)Item.sellPrice(0, 8, 0, 0);
			base.npc.HitSound = SoundID.NPCHit4;
			base.npc.DeathSound = SoundID.NPCDeath14;
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/Sagittarius");
			base.npc.knockBackResist = 0f;
			base.npc.noGravity = true;
			base.npc.alpha = 255;
			this.bossBag = base.mod.ItemType("SagBag");
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00129AF0 File Offset: 0x00127CF0
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if (Main.netMode == 2 || Main.dedServ)
			{
				writer.Write(this.internalAI[0]);
				writer.Write(this.internalAI[1]);
				writer.Write(this.internalAI[2]);
				writer.Write(this.internalAI[3]);
				writer.Write(this.internalAI[4]);
				writer.Write(this.shootAI[0]);
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00129B68 File Offset: 0x00127D68
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if (Main.netMode == 1)
			{
				this.internalAI[0] = BaseExtensions.ReadFloat(reader);
				this.internalAI[1] = BaseExtensions.ReadFloat(reader);
				this.internalAI[2] = BaseExtensions.ReadFloat(reader);
				this.internalAI[3] = BaseExtensions.ReadFloat(reader);
				this.internalAI[4] = BaseExtensions.ReadFloat(reader);
				this.shootAI[0] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00129BD8 File Offset: 0x00127DD8
		public override void AI()
		{
			if (Main.expertMode)
			{
				this.damage = base.npc.damage / 4;
			}
			else
			{
				this.damage = base.npc.damage / 2;
			}
			base.npc.noGravity = true;
			base.npc.TargetClosest(true);
			Player player = Main.player[base.npc.target];
			player.GetModPlayer<AAPlayer>();
			this.RingRoatation += 0.05f;
			base.npc.ai[1] += 1f;
			if (this.internalAI[3] > 0f)
			{
				this.internalAI[3] -= 1f;
			}
			base.npc.frameCounter += 1.0;
			if (base.npc.frameCounter > 30.0)
			{
				NPC npc = base.npc;
				npc.frame.Y = npc.frame.Y + 72;
				base.npc.frameCounter = 0.0;
				if (base.npc.frame.Y > 288)
				{
					base.npc.frame.Y = 0;
				}
			}
			if ((Main.player[base.npc.target].dead && Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) > 5000f) || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) > 5000f)
			{
				base.npc.TargetClosest(true);
				if (Main.player[base.npc.target].dead && this.internalAI[0] != 1f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("SagittariusFree1"), Color.PaleVioletRed, true);
					}
					this.internalAI[0] = 1f;
				}
				if ((Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) > 5000f || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) > 5000f) && this.internalAI[0] != 1f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("SagittariusFree2"), Color.PaleVioletRed, true);
					}
					this.internalAI[0] = 1f;
				}
			}
			if (this.internalAI[0] == 1f)
			{
				base.npc.TargetClosest(true);
				base.npc.velocity *= 0.7f;
				base.npc.alpha += 5;
				if (base.npc.alpha >= 255)
				{
					base.npc.active = false;
				}
				if (Main.player[base.npc.target].dead || Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) > 6000f || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) > 6000f)
				{
					base.npc.TargetClosest(true);
					this.internalAI[0] = 0f;
				}
			}
			else
			{
				base.npc.TargetClosest(true);
				if (base.npc.alpha > 0)
				{
					base.npc.alpha -= 10;
				}
				if (base.npc.alpha <= 0)
				{
					base.npc.alpha = 0;
				}
			}
			this.internalAI[1] += 1f;
			if (this.internalAI[2] == 2f)
			{
				base.npc.dontTakeDamage = true;
			}
			else
			{
				base.npc.dontTakeDamage = false;
			}
			if (this.internalAI[1] >= 300f)
			{
				this.internalAI[1] = 0f;
				this.internalAI[2] = (float)((this.internalAI[3] <= 0f) ? Main.rand.Next(3) : Main.rand.Next(2));
				this.internalAI[4] = (float)Main.rand.Next(2);
				if (this.internalAI[2] == 2f && Main.netMode != 1)
				{
					BaseUtility.Chat(Lang.BossChat("SagittariusFree3"), Color.PaleVioletRed, true);
				}
				base.npc.ai = new float[4];
				base.npc.netUpdate = true;
			}
			if (this.internalAI[2] == 1f)
			{
				BaseAI.AIEater(base.npc, ref base.npc.ai, 0.05f, 4f, 0f, false, true);
				base.npc.rotation = 0f;
				BaseAI.ShootPeriodic(base.npc, player.position, player.width, player.height, BaseExtensions.ProjType(base.mod, "SagProj"), ref this.shootAI[0], 15f, this.damage, 10f, true, new Vector2(20f, 15f));
			}
			else if (this.internalAI[2] == 2f)
			{
				this.ShieldScale += 0.02f;
				if (this.ShieldScale >= 1f)
				{
					this.ShieldScale = 1f;
				}
				this.internalAI[3] = 1200f;
				base.npc.life += 2;
				CombatText.NewText(new Rectangle((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height), CombatText.HealLife, string.Concat(2), false, false);
				BaseAI.AISpaceOctopus(base.npc, ref base.npc.ai, Main.player[base.npc.target].Center, 0.07f, 5f, 250f, 70f, new Action<NPC, Vector2>(this.ShootLaser));
			}
			else
			{
				BaseAI.AIEye(base.npc, ref base.npc.ai, false, true, 0.3f, 0.3f, 4f, 4f, 0f, 0f);
				base.npc.rotation = 0f;
			}
			if (this.internalAI[2] != 2f)
			{
				this.ShieldScale -= 0.02f;
				if (this.ShieldScale <= 0f)
				{
					this.ShieldScale = 0f;
				}
			}
			bool flag = false;
			if (this.internalAI[4] == 1f)
			{
				base.npc.alpha += 5;
				int num = Main.rand.Next(-500, 500);
				int num2 = Main.rand.Next(-500, 500);
				if (base.npc.alpha >= 255)
				{
					base.npc.alpha = 255;
					if ((num < -100 || num > 100) && (num2 < -90 || num2 > 90))
					{
						flag = true;
					}
				}
				if (flag)
				{
					Vector2 center = new Vector2(player.Center.X + (float)num, player.Center.Y + (float)num2);
					base.npc.Center = center;
					this.internalAI[4] = 0f;
				}
			}
			else
			{
				base.npc.alpha -= 3;
				if (base.npc.alpha <= 0)
				{
					base.npc.alpha = 0;
				}
			}
			base.npc.noTileCollide = true;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0012A414 File Offset: 0x00128614
		public void ShootLaser(NPC npc, Vector2 velocity)
		{
			double num = (double)0.783f;
			float num2 = (float)Math.Sqrt(200.0);
			double num3 = Math.Atan2(10.0, 10.0) - 0.1;
			double num4 = num / (double)6f;
			Player player = Main.player[npc.target];
			this.shootAI[0] += 1f;
			if (this.shootAI[0] >= 10f)
			{
				this.shootAI[0] = 0f;
				for (int i = 0; i < Main.rand.Next(1, 3); i++)
				{
					double num5 = num3 + num4 * (double)i;
					Projectile.NewProjectile(player.position.X, player.position.Y, num2 * (float)Math.Sin(num5), num2 * (float)Math.Cos(num5), ModContent.ProjectileType<DeathLaser>(), this.damage, 2f, Main.myPlayer, 0f, 0f);
				}
			}
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0012A514 File Offset: 0x00128714
		public override void HitEffect(int hitDirection, double damage)
		{
			if (base.npc.life <= 0)
			{
				int num = ModContent.DustType<VoidDust>();
				int num2 = ModContent.DustType<VoidDust>();
				Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, num, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 0.5f;
				Main.dust[num].scale *= 1.3f;
				Main.dust[num].fadeIn = 1f;
				Main.dust[num].noGravity = false;
				Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, num2, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num2].velocity *= 0.5f;
				Main.dust[num2].scale *= 1.3f;
				Main.dust[num2].fadeIn = 1f;
				Main.dust[num2].noGravity = true;
			}
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0012A698 File Offset: 0x00128898
		public override void NPCLoot()
		{
			AAWorld.downedSag = true;
			if (Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("SagTrophy"), 1, false, 0, false, false);
			}
			if (!Main.expertMode)
			{
				if (Main.rand.Next(7) == 0)
				{
					base.npc.DropLoot(base.mod.ItemType("SagMask"), 1);
				}
				string[] array = new string[]
				{
					"SagCore",
					"NeutronStaff",
					"Legg"
				};
				int num = Main.rand.Next(array.Length);
				base.npc.DropLoot(base.mod.ItemType(array[num]), 1);
				Item.NewItem(base.npc.Center, ModContent.ItemType<Doomite>(), Main.rand.Next(20, 30), false, 0, false, false);
				return;
			}
			base.npc.DropBossBags();
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0012A7BC File Offset: 0x001289BC
		public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
			Texture2D texture = base.mod.GetTexture("NPCs/Bosses/Sagittarius/SagittariusShield");
			Texture2D texture2 = base.mod.GetTexture("NPCs/Bosses/Sagittarius/SagittariusFreeRing");
			Texture2D texture3 = base.mod.GetTexture("Glowmasks/SagittariusFreeRing_Glow");
			Texture2D texture4 = base.mod.GetTexture("Glowmasks/SagittariusFree_Glow");
			BaseDrawing.DrawTexture(sb, Main.npcTexture[base.npc.type], 0, base.npc, new Color?(dColor), true, default(Vector2));
			BaseDrawing.DrawTexture(sb, texture4, 0, base.npc, new Color?(ColorUtils.COLOR_GLOWPULSE), true, default(Vector2));
			if (this.ShieldScale > 0f)
			{
				BaseDrawing.DrawTexture(sb, texture, 0, base.npc.position, base.npc.width, base.npc.height, this.ShieldScale, 0f, 0, 1, new Rectangle(0, 0, texture.Width, texture.Height), new Color?(AAColor.ZeroShield), true, default(Vector2));
			}
			BaseDrawing.DrawTexture(sb, texture2, 0, base.npc.position, base.npc.width, base.npc.height, 1f, this.RingRoatation, 0, 1, new Rectangle(0, 0, texture2.Width, texture2.Height), new Color?(dColor), true, default(Vector2));
			BaseDrawing.DrawTexture(sb, texture3, 0, base.npc.position, base.npc.width, base.npc.height, 1f, this.RingRoatation, 0, 1, new Rectangle(0, 0, texture3.Width, texture3.Height), new Color?(ColorUtils.COLOR_GLOWPULSE), true, default(Vector2));
			return false;
		}

		// Token: 0x0400050D RID: 1293
		public int damage;

		// Token: 0x0400050E RID: 1294
		public float ShieldScale;

		// Token: 0x0400050F RID: 1295
		public float RingRoatation;
	}
}
