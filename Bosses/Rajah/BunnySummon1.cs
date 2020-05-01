using System;
using AAMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000453 RID: 1107
	public class BunnySummon1 : ModProjectile
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x000263D8 File Offset: 0x000245D8
		public override string Texture
		{
			get
			{
				return "AAMod/BlankTex";
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0012C66F File Offset: 0x0012A86F
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Bunny Summon");
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0012C684 File Offset: 0x0012A884
		public override void SetDefaults()
		{
			base.projectile.width = 98;
			base.projectile.height = 98;
			base.projectile.penetrate = -1;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.tileCollide = false;
			base.projectile.ignoreWater = true;
			base.projectile.alpha = 255;
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0012C6F8 File Offset: 0x0012A8F8
		public override void AI()
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, ModContent.DustType<AbyssDust>(), 0f, 0f, 0, NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()) ? Main.DiscoColor : new Color(107, 137, 179), 1f);
				Main.dust[num].noGravity = true;
			}
			base.projectile.damage = 0;
			base.projectile.knockBack = 0f;
			this.Move(new Vector2(base.projectile.ai[0], base.projectile.ai[1]));
			if (Vector2.Distance(base.projectile.Center, new Vector2(base.projectile.ai[0], base.projectile.ai[1])) < 10f)
			{
				this.Kill(base.projectile.timeLeft);
			}
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0012C810 File Offset: 0x0012AA10
		public override void Kill(int timeLeft)
		{
			int num = ModContent.NPCType<RabbitcopterSoldier>();
			if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
			{
				num = ModContent.NPCType<RabbitcopterSoldier4>();
			}
			else if (NPC.AnyNPCs(ModContent.NPCType<Rajah7>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah8>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah9>()))
			{
				num = ModContent.NPCType<RabbitcopterSoldier3>();
			}
			else if (NPC.AnyNPCs(ModContent.NPCType<Rajah4>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah5>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah6>()))
			{
				num = ModContent.NPCType<RabbitcopterSoldier2>();
			}
			int num2 = NPC.NewNPC((int)base.projectile.Center.X, (int)base.projectile.Center.Y, num, 0, 0f, 0f, 0f, 0f, 255);
			Main.npc[num2].netUpdate2 = true;
			base.projectile.active = false;
			base.projectile.netUpdate2 = true;
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0012C8F8 File Offset: 0x0012AAF8
		public void Move(Vector2 point)
		{
			float num = 13f;
			float scaleFactor = 1f;
			Vector2 vector = point - base.projectile.Center;
			float num2 = (vector == Vector2.Zero) ? 0f : vector.Length();
			if (num2 < num)
			{
				scaleFactor = MathHelper.Lerp(0f, 1f, num2 / num);
			}
			if (num2 < 200f)
			{
				num *= 0.5f;
			}
			if (num2 < 100f)
			{
				num *= 0.5f;
			}
			if (num2 < 50f)
			{
				num *= 0.5f;
			}
			base.projectile.velocity = ((num2 == 0f) ? Vector2.Zero : Vector2.Normalize(vector));
			base.projectile.velocity *= num;
			base.projectile.velocity *= scaleFactor;
		}
	}
}
