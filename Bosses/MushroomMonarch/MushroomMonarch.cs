using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;

namespace AAModEXAI.Bosses.MushroomMonarch
{
    [AutoloadBossHead]
    public class MushroomMonarch : ModNPC
    {
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if(Main.netMode == NetmodeID.Server || Main.dedServ)
			{
				writer.Write(internalAI[0]);
				writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if(Main.netMode == NetmodeID.MultiplayerClient)
			{
				internalAI[0] = reader.ReadFloat();
				internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }	
		}	

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Monarch");
            Main.npcFrameCount[npc.type] = 12;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 1200;   //boss life
            npc.damage = 24;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0f;   //this boss will behavior like the DemonEye  //boss frame/animation 
            npc.value = Item.sellPrice(0, 0, 50, 0);
            npc.aiStyle = -1;
            npc.width = 74;
            npc.height = 108;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            bossBag = ModLoader.GetMod("AAMod").ItemType("MonarchBag");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Monarch");

        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public static int AISTATE_WALK = 0, AISTATE_JUMP = 1, AISTATE_CHARGE = 2, AISTATE_FLY = 3;
		public float[] internalAI = new float[4];
		
        public override void AI()
        {
            npc.TargetClosest();

            Player player = Main.player[npc.target];

            if (player == null)
            {
                npc.TargetClosest();
            }

            if (player.dead || !player.active || Vector2.Distance(player.Center, npc.Center) > 5000)
            {
                npc.TargetClosest();

                if (player.dead || !player.active || Vector2.Distance(player.Center, npc.Center) > 5000)
                {
                    Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("MonarchRUNAWAY"), 0, 0);
                    npc.active = false;
                    return;
                }
            }

            float dist = npc.Distance(player.Center);

            npc.frameCounter++;
            if (internalAI[1] != AISTATE_JUMP && internalAI[1] != AISTATE_FLY) //walk or charge
            {
                int FrameSpeed = 10;
                if (internalAI[1] == AISTATE_CHARGE)
                {
                    FrameSpeed = 6;
                }

                if (npc.frameCounter >= FrameSpeed)
				{
					npc.frameCounter = 0;
					npc.frame.Y += 108;
					if (npc.frame.Y > (108 * 4))
					{
						npc.frameCounter = 0;
						npc.frame.Y = 0;
					}
				}
                if(npc.velocity.Y != 0)
                {
                    if (npc.velocity.Y < 0)
                    {
                        npc.frame.Y = 648;
                    }else
                    if (npc.velocity.Y > 0)
                    {
                        npc.frame.Y = 756;
                    }
                }
            }
            else if (internalAI[1] == AISTATE_FLY)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 108;
                if (npc.frame.Y > (108 * 11) || npc.frame.Y < (108 * 8))
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 108 * 8;
                }

            }
            else //jump
            {
                if (npc.velocity.Y == 0)
                {
                    npc.frame.Y = 540;
                }else
                {
                    if (npc.velocity.Y < 0)
                    {
                        npc.frame.Y = 648;
                    }else
                    if (npc.velocity.Y > 0)
                    {
                        npc.frame.Y = 756;
                    }
                }
            }
            if (player.Center.X > npc.Center.X) // so it faces the player
            {
                npc.spriteDirection = -1;
            }else
            {
                npc.spriteDirection = 1;
            }

            if (npc.collideX && npc.velocity.Y <= 0)
            {
                npc.velocity.Y = -4f;
                internalAI[1] = AISTATE_CHARGE;
            }
            else if (((player.Center.Y - npc.Center.Y) < -150f && (internalAI[1] == AISTATE_WALK || internalAI[1] == AISTATE_CHARGE)) || Collision.SolidCollision(new Vector2(npc.Center.X, npc.position.Y - npc.height/2 + 10), npc.width, npc.height))
            {
                internalAI[1] = AISTATE_FLY;
                npc.ai = new float[4];
                npc.netUpdate = true;
            }
            else if ((player.Center.Y - npc.Center.Y) > 100f && internalAI[1] != AISTATE_FLY) // player is below the npc.
            {
                internalAI[3] = internalAI[1]; //record the action
                internalAI[1] = AISTATE_WALK;
                npc.ai = new float[4];
                npc.netUpdate = true;
            }
            else if(internalAI[1] != AISTATE_WALK)
            {
                internalAI[3] = internalAI[1];
            }
            else
            {
                internalAI[1] = internalAI[3];
            }

            
			if(Main.netMode != 1)
			{
                if (internalAI[1] != AISTATE_FLY)
                {
                    internalAI[0]++;
                }
                if (internalAI[0] >= 180)
                {
                    internalAI[0] = 0;
                    internalAI[1] = Main.rand.Next(3);
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
			}
			if(internalAI[1] == AISTATE_WALK) //fighter
			{
                npc.noGravity = false;
                if (Main.netMode != 1)
                {
                    internalAI[2]++;
                }
                if ((player.Center.Y - npc.Center.Y) > 60f) // player is below the npc.
                {
                    npc.noTileCollide = true;
                }
                else
                {
                    npc.noTileCollide = false;
                }

                if (NPC.CountNPCS(mod.NPCType("RedMushling")) < 4)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int Minion = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("RedMushling"), 0);
                        Main.npc[Minion].netUpdate = true;
                    }
                    internalAI[2] = 0;
                }
                AAAI.InfernoFighterAI(npc, ref npc.ai, true, false, 0, 0.07f, 3f, 3, 4, 60, true, 10, 60, true, null, false);	
			}else
			if(internalAI[1] == AISTATE_JUMP)//jumper
			{
                npc.noGravity = false;
                npc.noTileCollide = false;
                if(npc.ai[0] < -10) npc.ai[0] = -10; //force rapid jumping
                BaseAI.AISlime(npc, ref npc.ai, true, 30, 6f, -8f, 6f, -10f);
								
			}
            else if (internalAI[1] == AISTATE_FLY)//fly
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                if((player.Center.Y - npc.Center.Y) > 60f)
                {  
                    if (NPC.CountNPCS(mod.NPCType("RedMushling")) < 6)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int Minion = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("RedMushling"), 0);
                            Main.npc[Minion].netUpdate = true;
                        }
                    }
                    MoveToPoint(player.Center);
                    
                }
                else
                {
                    BaseAI.AISpaceOctopus(npc, ref npc.ai, .05f, 8, 250, 0, null);
                }
                
                
                npc.rotation = 0;
                if ((player.Center.Y - npc.Center.Y) > 30f && !Collision.SolidCollision(new Vector2(npc.Center.X, npc.position.Y - npc.height/2 + 10), npc.width, npc.height))
                {
                    npc.rotation = 0;
                    npc.noGravity = false;
                    internalAI[0] = 0;
                    internalAI[1] = Main.rand.Next(3);
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                    npc.noTileCollide = false;
                }
            }
            else //charger
			{
                BaseAI.AICharger(npc, ref npc.ai, 0.07f, 10f, false, 30);				
			}
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(Main.rand.Next(10) == 0)
            {
                int i = Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, 16, 16, 5, 1, false, 0, false, false);
                if (Main.netMode == 1 && i > 0)
                {
                    NetMessage.SendData(21, -1, -1, null, i, 1f, 0f, 0f, 0, 0, 0);
                }
            }
            else
            {
                Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("FakeMonarchMushroom"), 0, 0);
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if(Main.rand.Next(10) == 0)
            {
                int i = Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, 16, 16, 5, 1, false, 0, false, false);
                if (Main.netMode == 1 && i > 0)
                {
                    NetMessage.SendData(21, -1, -1, null, i, 1f, 0f, 0f, 0, 0, 0);
                }
            }
            else
            {
                Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("FakeMonarchMushroom"), 0, 0);
            }
        }
        
        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 8f;
            if (Vector2.Distance(npc.Center, point) > 500)
            {
                moveSpeed = 12f;
            }
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.LesserHealingPotion;
            AAWorld.downedMonarch = true;
            Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("MonarchRUNAWAY"), 0, 0);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("SporeSac"), Main.rand.Next(30, 35));
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("MonarchTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("MonarchMask"));
                }

                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("Mushium"), Main.rand.Next(25, 35));
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 0.6f);
        }
    }
}


