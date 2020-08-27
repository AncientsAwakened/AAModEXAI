using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Shen.GripsShen
{
    public abstract class BaseShenGrips : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grip of Chaos");
            Main.npcFrameCount[npc.type] = 14;
        }

        public int damage = 0;

        public override void SetDefaults()
        {
            npc.width = 66;
            npc.height = 60;			
            npc.aiStyle = -1;
			npc.knockBackResist = 0f;	
            npc.value = Item.sellPrice(0, 4, 50, 0);
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.netAlways = true;
            npc.scale *= 1.4f;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Shen");
        }
        
        public override void FindFrame(int frameHeight)
        {
            if (!NPC.AnyNPCs(mod.NPCType("Shen")))
            {
                npc.life = 0;
            }
            npc.frameCounter++;
            if (npc.frameCounter > 9)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
                if (npc.ai[0] == 2 || npc.ai[0] == 3)
                {
                    if (npc.frame.Y < 4 * frameHeight || npc.frame.Y > 7 * frameHeight)
                    {
                        npc.frame.Y = 4 * frameHeight;
                    }

                }
                else if (npc.ai[0] == 5)
                {
                    if (npc.frame.Y < 8 * frameHeight || npc.frame.Y > 11 * frameHeight)
                    {
                        npc.frame.Y = 8 * frameHeight;
                    }
                }
                else if (npc.ai[0] == 6)
                {
                    npc.frame.Y = frameHeight * 8;
                    if (internalAI[0] > 8)
                    {npc.frame.Y = frameHeight * 9;}
                    if (internalAI[0] > 16)
                    {npc.frame.Y = frameHeight * 10;}
                    if (internalAI[0] > 24)
                    { npc.frame.Y = frameHeight * 11; }
                    if (internalAI[0] > 32)
                    { npc.frame.Y = frameHeight * 12; }
                    if (internalAI[0] > 40)
                    { npc.frame.Y = frameHeight * 13 ;}
                }
                else
                {
                    if (npc.frame.Y > 3 * frameHeight)
                    {
                        npc.frame.Y = 0;
                    }
                }
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = 0;
        }


        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(mod.NPCType("Shen"));
        }


        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8f);
        }

		public override void BossHeadRotation(ref float rotation)
		{
			rotation = npc.rotation;
		}
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			spriteEffects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
		}

		public Vector2 offsetBasePoint = Vector2.Zero;

        public Vector2 Keepmove = Vector2.Zero;
		public float moveSpeed = 14f;
        public int MinionTimer = 0;

        public float[] internalAI = new float[3];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            if(npc.ai[0] == 3)
            {
                if(npc.type == mod.NPCType("BlazeGrip"))
                {
                    BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Red);
                }
                else
                {
                    BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Navy);
                }
            }
            return false;
        }

        public override void AI()
		{
            bool BlazeGrip = npc.type == mod.NPCType("BlazeGrip");

            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }
            npc.TargetClosest();
			Player targetPlayer = Main.player[npc.target];

            float ChangingPosX = 350f * (targetPlayer.Center.X > npc.Center.X? 1:-1);
            float ChangingPosY = 350f * (targetPlayer.Center.Y > npc.Center.Y? 1:-1);

            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
            {
                npc.TargetClosest(false);
                DespawnHandler();
                return;
            }

			if(npc.ai[0] == 1) //move to starting charge position
			{
				moveSpeed = 14f;
				Vector2 point = targetPlayer.Center + offsetBasePoint + new Vector2(0f, -ChangingPosY);
				MoveToPoint(point);
                internalAI[0] ++;
				if(Main.netMode != NetmodeID.MultiplayerClient && (Vector2.Distance(npc.Center, point) < 10f || internalAI[0] > 100))
				{
                    npc.ai[0] = 4;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
					npc.ai[3] = 0;
                    internalAI[0] = 0;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
					npc.netUpdate = true;
				}
				BaseAI.LookAt(targetPlayer.Center, npc, 0, 0f, 0.1f, false);			
			}else
			if(npc.ai[0] == 2) //dive prepare
			{
                if(internalAI[2] >= 1) internalAI[2] ++;
				moveSpeed = 22f;
				Vector2 targetCenter = new Vector2(npc.ai[1], npc.ai[2]);
				Vector2 point = targetCenter + new Vector2(ChangingPosX, ChangingPosY);
                Keepmove = point;
                if(Keepmove != new Vector2(0,0))
                {
                    MoveToPoint(Keepmove);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Main.PlaySound(SoundID.Roar, npc.position, 0);
                        npc.ai[0] = 3;			
                        npc.netUpdate = true;
                    }
                } 
				else
                {
                    npc.ai[0] = 0;
                    npc.ai[3] = 0;				
                    npc.netUpdate = true;
                }
				BaseAI.Look(npc, 0, 0f, 0.1f, false);				
			}else
			if(npc.ai[0] == 3) //diving
			{
                if(internalAI[2] >= 1) internalAI[2] ++;
				moveSpeed = 22f;
				MoveToPoint(Keepmove);
				if(Main.netMode != NetmodeID.MultiplayerClient && (Vector2.Distance(npc.Center, Keepmove) < 10f || npc.ai[3] ++ > 60))
				{
                    npc.ai[0] = 2;
                    npc.ai[1] = targetPlayer.Center.X;
                    npc.ai[2] = targetPlayer.Center.Y;
                    npc.ai[3] = 0;
                    if(internalAI[1] ++ > 4 && internalAI[2] == 0)
                    {
                        npc.ai[0] = 0;
                        internalAI[1] = 0;
                    }
                    if(internalAI[2] > 200)
                    {
                        npc.ai[0] = 6;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                    }
					npc.netUpdate = true;
				}			
			}else
            if (npc.ai[0] == 4) //Projectile skill
            {
                npc.direction = npc.spriteDirection = npc.position.X < targetPlayer.position.X ? -1 : 1;
                npc.rotation = npc.DirectionTo(targetPlayer.Center).ToRotation() + (npc.position.X < targetPlayer.position.X ? 0 : (float)Math.PI);
                moveSpeed = 14f;
				Vector2 point = targetPlayer.Center + offsetBasePoint + new Vector2(-ChangingPosX, 0);
				MoveToPoint(point);
                internalAI[0] ++;
                if(internalAI[0] == 100 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if(BlazeGrip)
                    {
                        Projectile.NewProjectile(npc.Center + 50f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center)), new Vector2(0, 0), mod.ProjectileType("BlazeCloneClaw"), damage / 2, 0f, Main.myPlayer, npc.whoAmI, 0);
                        Projectile.NewProjectile(npc.Center + 50f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center)) + 200f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center).RotatedBy(Math.PI / 2)), new Vector2(0, 0), mod.ProjectileType("BlazeCloneClaw"), damage / 2, 0f, Main.myPlayer, npc.whoAmI, 1f);
                        Projectile.NewProjectile(npc.Center + 50f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center)) + 400f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center).RotatedBy(Math.PI / 2)), new Vector2(0, 0), mod.ProjectileType("BlazeCloneClaw"), damage / 2, 0f, Main.myPlayer, npc.whoAmI, 2f);
                        Projectile.NewProjectile(npc.Center + 50f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center)) - 200f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center).RotatedBy(Math.PI / 2)), new Vector2(0, 0), mod.ProjectileType("BlazeCloneClaw"), damage / 2, 0f, Main.myPlayer, npc.whoAmI, -1f);
                        Projectile.NewProjectile(npc.Center + 50f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center)) - 400f * Vector2.Normalize(npc.DirectionTo(targetPlayer.Center).RotatedBy(Math.PI / 2)), new Vector2(0, 0), mod.ProjectileType("BlazeCloneClaw"), damage / 2, 0f, Main.myPlayer, npc.whoAmI, -2f);
                    }
                    else
                    {
                        for (int m = 0; m < 16; m++)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 0), mod.ProjectileType("AbyssGripOrbiter"), npc.damage / 2, 0f, Main.myPlayer, npc.whoAmI, 2f * (float)Math.PI / 16 * m);
                        }
                    }
                }
                
                if(internalAI[0] > 200)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        npc.ai[0] = 5;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        internalAI[0] = 0;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                    }
                    npc.netUpdate = true;
                }
            }
            else
            if (npc.ai[0] == 5) //Projectile skill2
            {
                if(BlazeGrip)
                {
                    if(internalAI[2] < 160)
                    {
                        BaseAI.LookAt(targetPlayer.Center, npc, 0, 0f, 0.1f, false);
                        moveSpeed = 18f;
                        Vector2 point = targetPlayer.Center + offsetBasePoint + new Vector2(0f, -ChangingPosY);
                        MoveToPoint(point);
                    }
                    else
                    {
                        npc.direction = npc.spriteDirection = npc.position.X < targetPlayer.position.X ? -1 : 1;
                        npc.rotation += (npc.DirectionTo(targetPlayer.Center).ToRotation()  + (npc.position.X < targetPlayer.position.X ? 0 : (float)Math.PI))/100f;
                        npc.velocity = new Vector2(0,0);
                    }
                    

                    internalAI[2] ++;
                    if(internalAI[2] == 160 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 dir = Vector2.Normalize(targetPlayer.Center - npc.Center);
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y);
                        double deltaAngle = 45f * 0.0174f;
                        for (int i = -1; i < 2; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Vector2 shootdir = new Vector2(baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle));
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(shootdir), mod.ProjectileType("BlazeGripRay"), npc.damage / 4, 0f, Main.myPlayer, i, npc.whoAmI);
                        }
                    }
                    if(internalAI[2] > 200)
                    {
                        npc.ai[0] = 6;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        internalAI[0] = 0;
                        internalAI[1] = 0;
                        internalAI[2] = 0;		
                        npc.netUpdate = true;
                    }

                }
                else
                {
                    moveSpeed = 22f;
                    Vector2 targetCenter = npc.position;
                    Vector2 point = targetCenter + new Vector2(ChangingPosX, ChangingPosY);
                    Keepmove = point;
                    if(Keepmove != new Vector2(0,0))
                    {
                        MoveToPoint(Keepmove);
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Main.PlaySound(SoundID.Roar, npc.position, 0);
                            npc.ai[0] = 3;			
                            internalAI[2] ++;
                            internalAI[1] = 0;
                            npc.netUpdate = true;
                        }
                    } 
                    else
                    {
                        Keepmove = targetCenter;
                    }
                }

            }
            else
            if (npc.ai[0] == 6) //Fire Projectile (Shen Grips)
            {
                internalAI[0]++;
                if(BlazeGrip && internalAI[0] < 40)
                {
                    npc.direction = npc.spriteDirection = npc.position.X < targetPlayer.position.X ? -1 : 1;
                    npc.rotation += (npc.DirectionTo(targetPlayer.Center).ToRotation()  + (npc.position.X < targetPlayer.position.X ? 0 : (float)Math.PI))/100f;
                    npc.velocity = new Vector2(0,0);
                }
                else
                {
                    moveSpeed = 22f;
                    Vector2 point = targetPlayer.Center + offsetBasePoint + new Vector2(0f, -ChangingPosY);
                    MoveToPoint(point);
                }
                if (internalAI[0] == 40)
                {
                    BaseAI.FireProjectile(targetPlayer.Center, npc.Center, BlazeGrip ? mod.ProjectileType("BlazeBomb") : mod.ProjectileType("AbyssalBomb"), damage, 2, 9f, -1, Main.myPlayer);
                }
                if (internalAI[0] > 50)
                {
                    npc.ai[0] = 2;
                    npc.ai[1] = targetPlayer.Center.X;
                    npc.ai[2] = targetPlayer.Center.Y;
                    npc.ai[3] = 0;
                    internalAI[0] = 0;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    npc.netUpdate = true;
                }
                BaseAI.LookAt(targetPlayer.Center, npc, 0, 0f, 0.1f, false);
            }
            else //standard movement
			{
				moveSpeed = 14f;
				Vector2 point = targetPlayer.Center + offsetBasePoint;
				MoveToPoint(point);
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.ai[1]++;
					if(npc.ai[1] > 90)
					{
						npc.ai[0] = 1;
						npc.ai[1] = 0;	
						npc.ai[2] = 0;
						npc.ai[3] = 0;	
                        internalAI[1] = 0;
                        internalAI[2] = 0;			
						npc.netUpdate = true;				
					}
				}		
				BaseAI.LookAt(targetPlayer.Center, npc, 0, 0f, 0.1f, false);			
			}
            if (npc.ai[0] == 0)
            {
                if (npc.alpha >= 50)
                {
                    npc.defense = 500;
                }
            }
            else
            {
                npc.alpha -= 5;
                if (npc.alpha <= 0)
                {
                    if (BlazeGrip)
                    {
                        npc.defense = 110;
                    }
                    else
                    {
                        npc.defense = 90;
                    }
                }
            }
        }

      

        public void MoveToPoint(Vector2 point)
		{
			if(moveSpeed == 0f || npc.Center == point) return; //don't move if you have no move speed
			float velMultiplier = 1f;
			Vector2 dist = point - npc.Center;
			float length = dist == Vector2.Zero ? 0f : dist.Length();
			if(length < moveSpeed)
			{
				velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
			}
			if(length < 200f)
			{
				moveSpeed *= 0.5f;
			}
			if(length < 100f)
			{
				moveSpeed *= 0.5f;
			}	
			if(length < 50f)
			{
				moveSpeed *= 0.5f;
			}
			npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
			npc.velocity *= moveSpeed;
			npc.velocity *= velMultiplier;
		}

        private void DespawnHandler()
        {
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            if (!player.active || player.dead || Main.dayTime)        // If the player is dead and not active, the npc flies off-screen and despawns
            {
                npc.velocity.X = 0;
                npc.velocity.Y -= 1;
            }
        }
    }
}
