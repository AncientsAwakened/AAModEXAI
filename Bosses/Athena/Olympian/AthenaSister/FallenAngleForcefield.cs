using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class FallenAngleForcefield : ModNPC
	{
		public override void SetDefaults()
		{
			npc.reflectingProjectiles = true;
			npc.dontTakeDamage = true;
			npc.scale = 0f;
			npc.width = 160;
			npc.height = 160;
			npc.alpha = 235;
			npc.lifeMax = (Main.expertMode ? 100 : 150);
			npc.aiStyle = 0;
			npc.damage = (Main.expertMode ? 50 : 84);
			npc.defense = 99999;
			npc.noGravity = true;
			npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			npc.lavaImmune = true;
			npc.noTileCollide = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forcefield");
		}

		public override Color? GetAlpha(Color drawColor)
		{
			return new Color?(Color.White);
		}

		public override bool PreAI()
		{
			ReflectProjectiles(npc.Hitbox);
			parent = Main.npc[(int)npc.ai[0]];
			if (!parent.active)
			{
				npc.life = 0;
				npc.checkDead();
			}
			npc.position.X = parent.Center.X;
			npc.position.Y = parent.Center.Y + 114f;
			if (Main.netMode != 2)
			{
				timeAlive++;
			}
			npc.life = npc.lifeMax;
			if (timeAlive <= 60)
			{
				npc.dontTakeDamage = true;
			}
			if (timeAlive > 60)
			{
				npc.dontTakeDamage = false;
				npc.alpha -= 10;
			}
			if (timeAlive > 190)
			{
				npc.scale -= 0.1f;
			}
			else
			{
				npc.scale += 0.3f;
			}
			if (timeAlive > 200)
			{
				npc.active = false;
			}
			if (npc.scale >= 1.5f)
			{
				npc.scale = 1.5f;
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
		}

		public void ReflectProjectiles(Rectangle myRect)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].hostile)
                {
                    Rectangle hitbox = Main.projectile[i].Hitbox;
                    if (myRect.Intersects(hitbox))
                    {
                        Main.PlaySound(SoundID.NPCHit4, Main.projectile[i].position);
                        for (int j = 0; j < 3; j++)
                        {
                            int num = Dust.NewDust(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, ModContent.DustType<Feather>(), 0f, 0f, 0, default, 1f);
                            Main.dust[num].velocity *= 0.3f;
                        }
                        Main.projectile[i].hostile = true;
                        Main.projectile[i].friendly = false;
                        Vector2 vector = Main.player[Main.projectile[i].owner].Center - Main.projectile[i].Center;
                        vector.Normalize();
                        vector *= Main.projectile[i].oldVelocity.Length();
                        Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        reflectvelocity.Normalize();
                        reflectvelocity *= vector.Length();
                        reflectvelocity += vector * 20f;
                        reflectvelocity.Normalize();
                        reflectvelocity *= 25f;
                        Main.projectile[i].penetrate = 1;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                    }
                }
            }
        }

		private NPC parent;

		private int timeAlive;

		public float auraPercent;

		public bool auraDirection = true;
	}
}
