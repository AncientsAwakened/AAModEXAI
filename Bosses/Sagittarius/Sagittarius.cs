using System;
using System.IO;
using AAMod.Dusts;
using AAMod.Items.Materials;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Sagittarius
{
	// Token: 0x02000444 RID: 1092
	[AutoloadBossHead]
	public class Sagittarius : ModNPC
	{
		// Token: 0x060019EF RID: 6639 RVA: 0x00128AFF File Offset: 0x00126CFF
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sagittarius");
			Main.npcFrameCount[base.npc.type] = 4;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00128B24 File Offset: 0x00126D24
		public override void SetDefaults()
		{
			base.npc.lifeMax = 6000;
			base.npc.boss = true;
			base.npc.defense = 300;
			base.npc.damage = 35;
			base.npc.width = 124;
			base.npc.height = 186;
			base.npc.aiStyle = -1;
			base.npc.HitSound = SoundID.NPCHit4;
			base.npc.DeathSound = SoundID.NPCDeath14;
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/Sagittarius");
			base.npc.knockBackResist = 0f;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
			this.bossBag = base.mod.ItemType("SagBag");
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00128C0C File Offset: 0x00126E0C
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
				writer.Write(this.internalAI[5]);
				writer.Write(this.internalAI[6]);
				writer.Write(this.shootAI[0]);
				writer.Write(Sagittarius.MovementType[0]);
				writer.Write(Sagittarius.MovementType[1]);
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00128CBC File Offset: 0x00126EBC
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
				this.internalAI[5] = BaseExtensions.ReadFloat(reader);
				this.internalAI[6] = BaseExtensions.ReadFloat(reader);
				this.shootAI[0] = BaseExtensions.ReadFloat(reader);
				Sagittarius.MovementType[0] = BaseExtensions.ReadFloat(reader);
				Sagittarius.MovementType[1] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00128D68 File Offset: 0x00126F68
		public override void AI()
		{
			base.npc.noGravity = true;
			base.npc.TargetClosest(true);
			Player player = Main.player[base.npc.target];
			AAPlayer modPlayer = player.GetModPlayer<AAPlayer>();
			if (player.Center.X > base.npc.Center.X)
			{
				base.npc.direction = -1;
			}
			else
			{
				base.npc.direction = 1;
			}
			if (this.internalAI[0] == 0f)
			{
				if (Main.netMode != 1)
				{
					for (int i = 0; i < this.ProbeCount; i++)
					{
						int num = NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y, base.mod.NPCType("SagittariusOrbiter"), 0, 0f, 0f, 0f, 0f, 255);
						Main.npc[num].Center = base.npc.Center;
						Main.npc[num].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
						Main.npc[num].velocity *= 8f;
						Main.npc[num].ai[0] = (float)i;
						Main.npc[num].netUpdate2 = true;
						Main.npc[num].netUpdate = true;
					}
				}
				this.internalAI[0] = 1f;
			}
			if (Sagittarius.MovementType[0] == 0f)
			{
				this.internalAI[6] += 1f;
				Sagittarius.MovementType[1] -= 5f;
				if (Sagittarius.MovementType[1] <= 0f)
				{
					Sagittarius.MovementType[1] = 0f;
				}
				if (this.internalAI[6] > 200f)
				{
					this.internalAI[6] = 0f;
					Sagittarius.MovementType[0] = (float)Main.rand.Next(3);
					base.npc.netUpdate = true;
				}
			}
			if (Sagittarius.MovementType[0] == 1f)
			{
				this.internalAI[6] += 1f;
				if (this.internalAI[6] > 240f)
				{
					this.internalAI[6] = 0f;
					Sagittarius.MovementType[0] = 5f;
					base.npc.netUpdate = true;
				}
			}
			if (Sagittarius.MovementType[0] == 2f)
			{
				Sagittarius.MovementType[1] += 5f;
				if (Sagittarius.MovementType[1] >= 255f)
				{
					Sagittarius.MovementType[0] = 3f;
					base.npc.netUpdate = true;
				}
			}
			if (Sagittarius.MovementType[0] == 3f)
			{
				Sagittarius.MovementType[1] -= 5f;
				if (Sagittarius.MovementType[1] <= 0f)
				{
					Sagittarius.MovementType[1] = 0f;
				}
				this.internalAI[6] += 1f;
				if (this.internalAI[6] > 360f)
				{
					this.internalAI[6] = 0f;
					Sagittarius.MovementType[0] = 5f;
					base.npc.netUpdate = true;
				}
			}
			else if (Sagittarius.MovementType[0] == 4f || Sagittarius.MovementType[0] == 5f)
			{
				Sagittarius.MovementType[1] += 5f;
				if (Sagittarius.MovementType[1] >= 255f)
				{
					Sagittarius.MovementType[0] = 0f;
					base.npc.netUpdate = true;
				}
			}
			if (this.internalAI[4] < 60f)
			{
				this.internalAI[4] += 1f;
			}
			if (!NPC.AnyNPCs(ModContent.NPCType<SagittariusOrbiter>()) && this.internalAI[4] >= 60f)
			{
				base.npc.Transform(ModContent.NPCType<SagittariusFree>());
			}
			if (this.internalAI[3] == 1f)
			{
				base.npc.TargetClosest(true);
				base.npc.velocity *= 0.7f;
				base.npc.alpha += 5;
				if (base.npc.alpha >= 255)
				{
					base.npc.active = false;
				}
				if (!Main.player[base.npc.target].dead || Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) <= 6000f || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) >= 6000f)
				{
					base.npc.TargetClosest(true);
				}
				return;
			}
			base.npc.TargetClosest(true);
			if (base.npc.alpha > 0)
			{
				base.npc.alpha -= 10;
			}
			if (base.npc.alpha <= 0)
			{
				base.npc.alpha = 0;
			}
			if (Main.player[base.npc.target].dead)
			{
				base.npc.TargetClosest(true);
				if (Main.player[base.npc.target].dead && this.internalAI[3] != 1f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("Sagittarius1"), Color.PaleVioletRed, true);
					}
					this.internalAI[3] = 1f;
				}
			}
			else if (Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) > 5000f || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) > 5000f || !modPlayer.ZoneVoid)
			{
				base.npc.TargetClosest(true);
				if ((Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) > 5000f || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) > 5000f) && this.internalAI[3] != 1f)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("Sagittarius2"), Color.PaleVioletRed, true);
					}
					this.internalAI[3] = 1f;
				}
			}
			base.npc.ai[1] += 1f;
			if (base.npc.ai[1] >= 300f)
			{
				this.internalAI[1] = 0f;
				this.internalAI[2] = (float)Main.rand.Next(3);
				base.npc.ai = new float[4];
				base.npc.netUpdate = true;
			}
			if (this.internalAI[2] == 1f)
			{
				BaseAI.AIElemental(base.npc, ref base.npc.ai, null, 120, false, false, 10f, 10f, 10, 2.5f);
			}
			else
			{
				BaseAI.AISpaceOctopus(base.npc, ref base.npc.ai, Main.player[base.npc.target].Center, 0.3f, 10f, 250f, 70f, null);
			}
			base.npc.rotation = 0f;
			base.npc.noTileCollide = true;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0012956C File Offset: 0x0012776C
		public override void FindFrame(int frameHeight)
		{
			NPC npc = base.npc;
			double frameCounter = npc.frameCounter;
			npc.frameCounter = frameCounter + 1.0;
			if (frameCounter < (double)(16f - base.npc.velocity.X))
			{
				NPC npc2 = base.npc;
				npc2.frame.Y = npc2.frame.Y + frameHeight;
				if (base.npc.frame.Y > frameHeight * 3)
				{
					base.npc.frame.Y = 0;
				}
			}
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x001295EC File Offset: 0x001277EC
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

		// Token: 0x060019F6 RID: 6646 RVA: 0x00129770 File Offset: 0x00127970
		public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
			BaseDrawing.DrawTexture(sb, Main.npcTexture[base.npc.type], 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.direction, 4, base.npc.frame, new Color?(dColor), true, default(Vector2));
			BaseDrawing.DrawTexture(sb, base.mod.GetTexture("Glowmasks/Sagittarius_Glow"), 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.direction, 4, base.npc.frame, new Color?(ColorUtils.COLOR_GLOWPULSE), true, default(Vector2));
			return false;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x00129870 File Offset: 0x00127A70
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
				Item.NewItem(base.npc.Center, ModContent.ItemType<Doomite>(), Main.rand.Next(30, 40), false, 0, false, false);
				return;
			}
			base.npc.DropBossBags();
		}

		// Token: 0x04000509 RID: 1289
		public static float[] MovementType = new float[2];

		// Token: 0x0400050A RID: 1290
		public float[] shootAI = new float[1];

		// Token: 0x0400050B RID: 1291
		public float[] internalAI = new float[7];

		// Token: 0x0400050C RID: 1292
		public int ProbeCount = Main.expertMode ? 12 : 6;
	}
}
