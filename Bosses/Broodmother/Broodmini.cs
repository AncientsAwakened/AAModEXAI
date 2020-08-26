using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Broodmother
{
    public class Broodmini : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broodmini");
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 66;
            npc.height = 56;
            npc.aiStyle = -1;
            npc.damage = 30;
            npc.defense = 0;
            npc.lifeMax = 75;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.3f;
            npc.value = 0f;
            npc.npcSlots = 0.1f;
            animationType = NPCID.MothronSpawn;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			bool isDead = npc.life <= 0;		
            if (isDead)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BroodminiGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BroodminiGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BroodminiGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BroodminiGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BroodminiGore3"), 1f);
            }
			for (int m = 0; m < (isDead ? 35 : 6); m++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, isDead? 2f : 1.5f);
			}			
        }

        public override void NPCLoot()
        {
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("Incinerite"), 5, 6);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("BroodScale"), 2, 4);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.rand.Next(2) == 0 || (Main.expertMode && Main.rand.Next(0) == 0))       //Chances for it to inflict the debuff
            {
                target.AddBuff(BuffID.OnFire, Main.rand.Next(60, 120));       //Main.rand.Next part is the length of the buff, so 8.3 seconds to 16.6 seconds
            }
        }

        public override void AI()
        {
            npc.noTileCollide = false;
            npc.knockBackResist = 0.4f * Main.knockBackMultiplier;
            npc.noGravity = true;
            npc.rotation = ((npc.rotation * 9f) + (npc.velocity.X * 0.1f)) / 10f;
            AAMod.AAPlayer modPlayer = Main.player[npc.target].GetModPlayer<AAMod.AAPlayer>();
            if (!modPlayer.ZoneInferno)
            {
                if (npc.timeLeft > 5)
					npc.timeLeft = 5;
                npc.velocity.Y = npc.velocity.Y - 0.2f;
                if (npc.velocity.Y < -8f)
                {
                    npc.velocity.Y = -8f;
                }
                npc.noTileCollide = true;
                return;
            }
            if (npc.ai[0] == 0f || npc.ai[0] == 1f)
            {
                for (int num1328 = 0; num1328 < 200; num1328++)
                {
                    if (num1328 != npc.whoAmI && Main.npc[num1328].active && Main.npc[num1328].type == npc.type)
                    {
                        Vector2 value55 = Main.npc[num1328].Center - npc.Center;
                        if (value55.Length() < npc.width + npc.height)
                        {
                            value55.Normalize();
                            value55 *= -0.1f;
                            npc.velocity += value55;
                            Main.npc[num1328].velocity -= value55;
                        }
                    }
                }
            }
            if (npc.target < 0 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
                Vector2 vector209 = Main.player[npc.target].Center - npc.Center;
                if (Main.player[npc.target].dead || vector209.Length() > 3000f)
                {
                    npc.ai[0] = -1f;
                }
            }
            else
            {
                Vector2 vector210 = Main.player[npc.target].Center - npc.Center;
                if (npc.ai[0] > 1f && vector210.Length() > 1000f)
                {
                    npc.ai[0] = 1f;
                }
            }
            if (npc.ai[0] == -1f)
            {
                Vector2 value56 = new Vector2(0f, -8f);
                npc.velocity = ((npc.velocity * 9f) + value56) / 10f;
                npc.noTileCollide = true;
                npc.dontTakeDamage = true;
                return;
            }
            if (npc.ai[0] == 0f)
            {
                npc.TargetClosest(true);
                npc.spriteDirection = npc.direction;
                if (npc.collideX)
                {
                    npc.velocity.X = npc.velocity.X * (-npc.oldVelocity.X * 0.5f);
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X = 4f;
                    }
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X = -4f;
                    }
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.velocity.Y * (-npc.oldVelocity.Y * 0.5f);
                    if (npc.velocity.Y > 4f)
                    {
                        npc.velocity.Y = 4f;
                    }
                    if (npc.velocity.Y < -4f)
                    {
                        npc.velocity.Y = -4f;
                    }
                }
                Vector2 value57 = Main.player[npc.target].Center - npc.Center;
                if (value57.Length() > 800f)
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                }
                else if (value57.Length() > 200f)
                {
                    float scaleFactor20 = 5.5f + (value57.Length() / 100f) + (npc.ai[1] / 15f);
                    float num1329 = 40f;
                    value57.Normalize();
                    value57 *= scaleFactor20;
                    npc.velocity = ((npc.velocity * (num1329 - 1f)) + value57) / num1329;
                }
                else if (npc.velocity.Length() > 2f)
                {
                    npc.velocity *= 0.95f;
                }
                else if (npc.velocity.Length() < 1f)
                {
                    npc.velocity *= 1.05f;
                }
                npc.ai[1] += 1f;
                if (npc.ai[1] >= 90f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 2f;
                    return;
                }
            }
            else
            {
                if (npc.ai[0] == 1f)
                {
                    npc.collideX = false;
                    npc.collideY = false;
                    npc.noTileCollide = true;
                    npc.knockBackResist = 0f;
                    if (npc.target < 0 || !Main.player[npc.target].active || Main.player[npc.target].dead)
                    {
                        npc.TargetClosest(true);
                    }
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.direction = 1;
                    }
                    npc.spriteDirection = npc.direction;
                    npc.rotation = ((npc.rotation * 9f) + (npc.velocity.X * 0.08f)) / 10f;
                    Vector2 value58 = Main.player[npc.target].Center - npc.Center;
                    if (value58.Length() < 300f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                    {
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.ai[3] = 0f;
                    }
                    npc.ai[2] += 0.0166666675f;
                    float scaleFactor21 = 5.5f + npc.ai[2] + (value58.Length() / 150f);
                    float num1330 = 35f;
                    value58.Normalize();
                    value58 *= scaleFactor21;
                    npc.velocity = ((npc.velocity * (num1330 - 1f)) + value58) / num1330;
                    return;
                }
                if (npc.ai[0] == 2f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.direction = 1;
                    }
                    npc.spriteDirection = npc.direction;
                    npc.rotation = ((npc.rotation * 7f) + (npc.velocity.X * 0.1f)) / 8f;
                    npc.knockBackResist = 0f;
                    npc.noTileCollide = true;
                    Vector2 vector211 = Main.player[npc.target].Center - npc.Center;
                    vector211.Y -= 8f;
                    float scaleFactor22 = 9f;
                    float num1331 = 8f;
                    vector211.Normalize();
                    vector211 *= scaleFactor22;
                    npc.velocity = ((npc.velocity * (num1331 - 1f)) + vector211) / num1331;
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    else
                    {
                        npc.direction = 1;
                    }
                    npc.spriteDirection = npc.direction;
                    npc.ai[1] += 1f;
                    if (npc.ai[1] > 10f)
                    {
                        npc.velocity = vector211;
                        if (npc.velocity.X < 0f)
                        {
                            npc.direction = -1;
                        }
                        else
                        {
                            npc.direction = 1;
                        }
                        npc.ai[0] = 2.1f;
                        npc.ai[1] = 0f;
                        return;
                    }
                }
                else if (npc.ai[0] == 2.1f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.direction = 1;
                    }
                    npc.spriteDirection = npc.direction;
                    npc.velocity *= 1.01f;
                    npc.knockBackResist = 0f;
                    npc.noTileCollide = true;
                    npc.ai[1] += 1f;
                    int num1332 = 45;
                    if (npc.ai[1] > num1332)
                    {
                        if (!Collision.SolidCollision(npc.position, npc.width, npc.height))
                        {
                            npc.ai[0] = 0f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            return;
                        }
                        if (npc.ai[1] > num1332 * 2)
                        {
                            npc.ai[0] = 1f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            return;
                        }
                    }
                }
            }
        }
		
		
		public Color GetGlowAlpha()
		{
			return ColorUtils.COLOR_GLOWPULSE;
		}

        public override void PostDraw(SpriteBatch sb, Color dColor)
        {
			BaseDrawing.DrawTexture(sb, ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Broodmini_Glow"), 0, npc, GetGlowAlpha());
        }		
		
    }
}