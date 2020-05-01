using System;
using AAMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000454 RID: 1108
	public class BunnySummon2 : ModProjectile
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x000263D8 File Offset: 0x000245D8
		public override string Texture
		{
			get
			{
				return "AAMod/BlankTex";
			}
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0012C66F File Offset: 0x0012A86F
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Bunny Summon");
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0012C9D4 File Offset: 0x0012ABD4
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

		// Token: 0x06001A51 RID: 6737 RVA: 0x0012CA48 File Offset: 0x0012AC48
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

		// Token: 0x06001A52 RID: 6738 RVA: 0x0012CB60 File Offset: 0x0012AD60
		public override void Kill(int timeLeft)
		{
			int num = ModContent.NPCType<BunnyBrawler>();
			if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
			{
				num = ModContent.NPCType<BunnyBrawler4>();
			}
			else if (NPC.AnyNPCs(ModContent.NPCType<Rajah7>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah8>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah9>()))
			{
				num = ModContent.NPCType<BunnyBrawler3>();
			}
			else if (NPC.AnyNPCs(ModContent.NPCType<Rajah4>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah5>()) || NPC.AnyNPCs(ModContent.NPCType<Rajah6>()))
			{
				num = ModContent.NPCType<BunnyBrawler2>();
			}
			int num2 = NPC.NewNPC((int)base.projectile.Center.X, (int)base.projectile.Center.Y, num, 0, 0f, 0f, 0f, 0f, 255);
			Main.npc[num2].netUpdate2 = true;
			base.projectile.active = false;
			base.projectile.netUpdate2 = true;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0012CC48 File Offset: 0x0012AE48
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
