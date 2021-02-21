
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah
{
    public class RabbitcopterSoldier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rabbitcopter Soldier");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 36;
            npc.aiStyle = -1;
            npc.damage = 50;
            npc.defense = 20;
            npc.lifeMax = 400;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.3f;
            npc.value = 0f;
            npc.npcSlots = 0.1f;
            animationType = NPCID.MothronSpawn;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			bool isDead = npc.life <= 0;		
            if (isDead)          //this make so when the npc has 0 life(dead) he will spawn this
            {

            }
			for (int m = 0; m < (isDead ? 10 : 3); m++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }			
        }

        public override void PostAI()
        {
            if (NPC.AnyNPCs(mod.NPCType("Rajah")) ||
                   NPC.AnyNPCs(mod.NPCType("SupremeRajah")))
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                }
                else
                {
                    npc.alpha = 0;
                }
            }
            else
            {
                npc.dontTakeDamage = true;
                if (npc.alpha < 255)
                {
                    npc.alpha += 5;
                }
                else
                {
                    npc.active = false;
                }
            }
        }

        public bool SetLife = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(SetLife);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SetLife = reader.ReadBool(); //Set Lifex
            }
        }

        public override void AI()
        {
            npc.noTileCollide = false;
            npc.knockBackResist = 0.4f * Main.knockBackMultiplier;
            npc.noGravity = true;
            npc.rotation = ((npc.rotation * 9f) + (npc.velocity.X * 0.1f)) / 10f;
            if (!(NPC.AnyNPCs(mod.NPCType("Rajah")) ||
                NPC.AnyNPCs(mod.NPCType("SupremeRajah"))))
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
    }
    public class RabbitcopterSoldier2 : RabbitcopterSoldier
    {
        public override string Texture => "AAModEXAI/Bosses/Rajah/RabbitcopterSoldier";
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.damage = 170;
            npc.defense = 70;
            npc.lifeMax = 900;
        }
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage /= 2;
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (NPC.AnyNPCs(mod.NPCType("SupremeRajah")))
            {
                BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc, 1f, 1f, 10, false, 0f, 0f, AAColor.Rainbow3);
            }
            return false;
        }
    }
}