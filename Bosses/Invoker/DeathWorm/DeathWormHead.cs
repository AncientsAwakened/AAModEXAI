using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
 
using AAModEXAI.Dusts;
using AAModEXAI.Misc;
using AAModEXAI.Base;
using AAModEXAI.Localization;

namespace AAModEXAI.Bosses.Invoker.DeathWorm
{
    //[AutoloadBossHead]
    public class DeathWormHead : ModNPC
    {
        public bool loludided;
        private bool weakness;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked StarGod");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void SetDefaults()
        {
            npc.noTileCollide = true;
            npc.height = 240;
            npc.width = 240;
            npc.aiStyle = -1;
            npc.netAlways = true;
            npc.knockBackResist = 0f;
            npc.damage = 1000;
			npc.defense = 450;
			npc.lifeMax = 700000;
            if (Main.expertMode)
            {
                npc.value = Item.sellPrice(0, 0, 0, 0);
            }
            else
            {
                npc.value = Item.sellPrice(0, 30, 0, 0);
            }
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.aiStyle = -1;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath7;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.buffImmune[BuffID.Wet] = false;
            npc.alpha = 255;
            musicPriority = MusicPriority.BossHigh;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override bool PreAI()
        {
            Player player = Main.player[npc.target];
            if (player.dead || !player.active || (npc.position.X - Main.player[npc.target].position.X) > 6000f || (npc.position.X - Main.player[npc.target].position.X) < -6000f || (npc.position.Y - Main.player[npc.target].position.Y) > 6000f || (npc.position.Y - Main.player[npc.target].position.Y) < -6000f)
            {
                npc.TargetClosest(true);
            }
            if(npc.ai[3] > 1000 || Main.projectile[(int)npc.ai[3]].type != ModContent.ProjectileType<DeathWormBody>() || !AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<DeathWormBody>()))
            {
                int id = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<DeathWormBody>(), npc.damage / 4, 5, Main.myPlayer);
                Main.projectile[id].ai[0] = 0;
                Main.projectile[id].ai[1] = npc.whoAmI;
                npc.ai[3] = id;
            }

            if (npc.timeLeft < NPC.activeTime)
            {
                npc.timeLeft = NPC.activeTime;
            }

            Vector2 targetPos;

            switch ((int)npc.ai[0])
            {
                case 0:
                    MovementWorm();
                    if(Vector2.Distance(npc.Center, player.Center) < 4000f)
                    {
                        npc.ai[0] ++;
                        npc.ai[2] = 0;
                        npc.velocity = PredictPlayerMovement(35f, player);
                        if(npc.velocity.Length() < 35f) npc.velocity = Vector2.Normalize(npc.velocity) * 25f;
                        npc.netUpdate = true;
                    }
                    break;
                case 1:
                    if(Vector2.Distance(npc.Center, player.Center) > 4000f)
                    {
                        npc.ai[0] ++;
                        npc.velocity /= 2;
                        npc.ai[2] = player.velocity.X > 0? 1 : -1;
                        npc.netUpdate = true;
                    }
                    break;

                case 2: //prepare for dash
                    targetPos = player.Center;
                    targetPos.X += 1000 * npc.ai[2];
                    targetPos.Y += 500 * (npc.Center.Y > player.Center.Y? 1 : -1);
                    if(Vector2.Distance(npc.Center, player.Center) > 1500f)
                    {
                        MovementWorm(targetPos, player.velocity.Length() + 20f, 0.6f);
                    }
                    else
                    {
                        MovementWorm(targetPos, player.velocity.Length() + 8f, 0.3f);
                    }
                    if (++npc.ai[1] > 1200 && npc.Distance(targetPos) < 50) //initiate dash
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.velocity = PredictPlayerMovement(25f, player);
                        if(npc.velocity.Length() < 25f) npc.velocity = Vector2.Normalize(npc.velocity) * 25f;
                        npc.netUpdate = true;
                    }
                    break;

                case 3: //wait for new
                    if(Vector2.Distance(npc.Center, player.Center) > 4000f)
                    {
                        npc.ai[0] ++;
                        npc.velocity /= 2;
                        npc.netUpdate = true;
                    }
                    break;

                case 4:
                    MovementWorm();
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 5:
                    targetPos = new Vector2(Main.leftWorld, player.Center.Y);
                    MovementWorm(targetPos, 50f, 0.6f);
                    if (npc.Distance(targetPos) < 50)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 6:
                    targetPos = new Vector2(Main.rightWorld, player.Center.Y);
                    MovementWorm(targetPos, 50f, 0.6f);
                    if (npc.Distance(targetPos) < 50)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;
                
                default:
                    npc.ai[0] = 0;
                    goto case 0;
                
            }

            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = 1;

            }
            else
            {
                npc.spriteDirection = -1;
            }
            return false;
        }

        public void MovementWorm()
        {
            bool collision = true;

            float speed;
            float acceleration;

            speed = 30f;
            acceleration = 0.28f;

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            float targetRoundedPosX = (int)(targetXPos / 16.0) * 16;
            float targetRoundedPosY = (int)(targetYPos / 16.0) * 16;
            npcCenter.X = (int)(npcCenter.X / 16.0) * 16;
            npcCenter.Y = (int)(npcCenter.Y / 16.0) * 16;
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (!collision)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.11f;
                if (npc.velocity.Y > speed)
                    npc.velocity.Y = speed;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    else
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                }
                else if (npc.velocity.Y == speed)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
                else if (npc.velocity.Y > 4.0)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    npc.soundDelay = (int)num1;
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = speed / length;
                dirX *= newSpeed;
                dirY *= newSpeed;
                if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                    if (npc.velocity.Y < dirY)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                    else if (npc.velocity.Y > dirY)
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                    if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                    {
                        if (npc.velocity.Y > 0.0)
                            npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                        else
                            npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                    {
                        if (npc.velocity.X > 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration * 2f;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                    {
                        if (npc.velocity.Y > 0.0)
                            npc.velocity.Y = npc.velocity.Y + acceleration;
                        else
                            npc.velocity.Y = npc.velocity.Y - acceleration;
                    }
                }
                else
                {
                    if (npc.velocity.Y < dirY)
                        npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
                    else if (npc.velocity.Y > dirY)
                        npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                    {
                        if (npc.velocity.X > 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration;
                    }
                }
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
        }

        public void MovementWorm(Vector2 target, float speed, float acceleration)
        {
            Vector2 npcCenter = npc.Center;// new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            //float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            //float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            float targetRoundedPosX = target.X;// (int)(targetXPos / 16.0) * 16;
            float targetRoundedPosY = target.Y;// (int)(targetYPos / 16.0) * 16;
            //npcCenter.X = (int)(npcCenter.X / 16.0) * 16;
            //npcCenter.Y = (int)(npcCenter.Y / 16.0) * 16;
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;
            npc.TargetClosest(true);
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;
            dirX *= newSpeed;
            dirY *= newSpeed;
            if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0)
            {
                if (npc.velocity.X < dirX)
                    npc.velocity.X = npc.velocity.X + acceleration;
                else if (npc.velocity.X > dirX)
                    npc.velocity.X = npc.velocity.X - acceleration;
                if (npc.velocity.Y < dirY)
                    npc.velocity.Y = npc.velocity.Y + acceleration;
                else if (npc.velocity.Y > dirY)
                    npc.velocity.Y = npc.velocity.Y - acceleration;
                if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                {
                    if (npc.velocity.Y > 0.0)
                        npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                    else
                        npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                }
                if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                {
                    if (npc.velocity.X > 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 2f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                if (npc.velocity.X < dirX)
                    npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                else if (npc.velocity.X > dirX)
                    npc.velocity.X = npc.velocity.X - acceleration * 1.1f;

                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                {
                    if (npc.velocity.Y > 0.0)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                    else
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                }
            }
            else
            {
                if (npc.velocity.Y < dirY)
                    npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
                else if (npc.velocity.Y > dirY)
                    npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;

                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                {
                    if (npc.velocity.X > 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
            }
        }

        public Vector2 PredictPlayerMovement(float speed, Player player)
        {
            Vector2 npctoplayer = player.Center - npc.Center;
            float playerspeed = (float)Math.Sqrt(player.velocity.X * player.velocity.X + player.velocity.Y * player.velocity.Y);
            float distance = (float)Math.Sqrt(npctoplayer.X * npctoplayer.X + npctoplayer.Y * npctoplayer.Y);
            float deg = player.velocity.ToRotation() - npctoplayer.ToRotation();
            float speedtoplayer = 2f * speed * (float)Math.Cos(deg) + 2f * (float)Math.Sqrt(speed * speed * Math.Cos(deg) * Math.Cos(deg) + speed * speed - playerspeed * playerspeed);
            float movetime = distance / speedtoplayer;
            
            
            if(speed <= playerspeed)
            {
                float newspeed = playerspeed + 10f;
                speedtoplayer = 2f * newspeed * (float)Math.Cos(deg) + 2f * (float)Math.Sqrt(newspeed * newspeed * Math.Cos(deg) * Math.Cos(deg) + newspeed * newspeed - playerspeed * playerspeed);
                movetime = distance / speedtoplayer;
                Vector2 velocity = player.velocity + npctoplayer / movetime;
                velocity.Normalize();
                velocity *= speed;
                return velocity;
            }

            return player.velocity + npctoplayer / movetime;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

        public override void NPCLoot()
        {
            npc.value = 0f;
            npc.boss = false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.position.X + npc.width / 2;
                npc.position.Y = npc.position.Y + npc.height / 2;
                npc.position.X = npc.position.X - npc.width / 2;
                npc.position.Y = npc.position.Y - npc.height / 2;
                int dust1 = ModContent.DustType<AkumaDust>();
                int dust2 = ModContent.DustType<AkumaDust>();
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
        }


        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
    }
    public class DeathWormBody : ModProjectile
    {
        public override string Texture => "AAModEXAI/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked StarGod");
        }

        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 160;
            projectile.hostile = true;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
            cooldownSlot = 1;
        }

        //The worm length

        public static Vector2[] bodyPos = new Vector2[2000];

        public override void AI()
        {
            if (projectile.ai[1] > 200 || projectile.ai[1] < 0)
            {
                projectile.active = false;
                projectile.netUpdate = true;
                return;
            }
            
            NPC headnpc = Main.npc[(int)projectile.ai[1]];

            if (!headnpc.active || headnpc.type != ModContent.NPCType<DeathWormHead>())
            {
                projectile.active = false;
                projectile.netUpdate = true;
            }

            projectile.rotation = headnpc.rotation;
            projectile.direction = headnpc.direction;
            projectile.spriteDirection = headnpc.spriteDirection;

            bodyPos[0] = headnpc.position;

            if(projectile.ai[0] == 0f)
            {
                for(int i = 1; i < bodyPos.Length - 1; i ++)
                {
                    bodyPos[i] = headnpc.position + new Vector2(.1f,0f);
                }
                projectile.ai[0] = 1f;
            }

            for(int i = 1; i < bodyPos.Length - 1; i ++)
            {
                Vector2 bodytobody = bodyPos[i-1] - bodyPos[i];
                float dist = (bodytobody.Length() - projectile.height) / bodytobody.Length();
                float posX = bodytobody.X * dist;
                float posY = bodytobody.Y * dist;

                bodyPos[i].X = bodyPos[i].X + posX;
                bodyPos[i].Y = bodyPos[i].Y + posY;
            }

            projectile.timeLeft ++;

            return;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(projectile.ai[0] < 1f) return false;
            for(int i = 1; i < bodyPos.Length - 1; i ++)
            {
                Vector2 bodytobody = bodyPos[bodyPos.Length - 1 - i - 1] - bodyPos[bodyPos.Length - 1 - i];
                float rotation = (float)Math.Atan2(bodytobody.Y, bodytobody.X) + 1.57f;

                int spriteDirection = 1;

                if (bodytobody.X < 0f)
                {
                    spriteDirection = 1;

                }
                else
                {
                    spriteDirection = -1;
                }

                int frame = 0;
                frame = i % 2;
                if(i == 1) frame = 2;

                string bodypath = "Bosses/Invoker/DeathWorm/DeathWormBody";
                Texture2D bodyTex = mod.GetTexture(bodypath + frame);
                Texture2D bodyglowTex = mod.GetTexture(bodypath + frame + "Glow");
                Texture2D bodyglowTex2 = mod.GetTexture(bodypath + frame + "Glow2");

                Rectangle Rectframe = BaseDrawing.GetFrame(0, bodyTex.Width, bodyTex.Height, 0, 0);

                //if (Main.mapStyle == 1 || Main.mapStyle == 2)
                //{
                //    drawBossHead(bodypath + frame, bodyPos.Length - 1 - i, rotation, spriteDirection);
                //}

                BaseDrawing.DrawTexture(spriteBatch, bodyTex, 0, bodyPos[bodyPos.Length - 1 - i], projectile.width, projectile.height, 1f, rotation, spriteDirection, 1, Rectframe, drawColor, true);
                BaseDrawing.DrawTexture(spriteBatch, bodyglowTex, 0, bodyPos[bodyPos.Length - 1 - i], projectile.width, projectile.height, 1f, rotation, spriteDirection, 1, Rectframe, Color.White, true);
                BaseDrawing.DrawTexture(spriteBatch, bodyglowTex2, 0, bodyPos[bodyPos.Length - 1 - i], projectile.width, projectile.height, 1f, rotation, spriteDirection, 1, Rectframe, Color.White, true);
            }

            string headpath = "Bosses/Invoker/DeathWorm/DeathWormHead";
            Texture2D headTex = mod.GetTexture(headpath + "");
            Texture2D headglowTex = mod.GetTexture(headpath + "Glow");
            Texture2D headglowTex2 = mod.GetTexture(headpath + "Glow2");

            Rectangle Rectheadframe = BaseDrawing.GetFrame(0, headTex.Width, headTex.Height, 0, 0);
            spriteBatch.Draw(headTex, bodyPos[0] + new Vector2(headTex.Width/2, headTex.Height/2) + new Vector2(-75f, -67f) - Main.screenPosition, Rectheadframe, drawColor, projectile.rotation, Rectheadframe.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(headglowTex, bodyPos[0] + new Vector2(headTex.Width/2, headTex.Height/2) + new Vector2(-75f, -67f) - Main.screenPosition, Rectheadframe, Color.White, projectile.rotation, Rectheadframe.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(headglowTex2, bodyPos[0] + new Vector2(headTex.Width/2, headTex.Height/2) + new Vector2(-75f, -67f) - Main.screenPosition, Rectheadframe, Color.White, projectile.rotation, Rectheadframe.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            return false;
        }

        /*
        public void drawBossHead(string path, int i, float rotation, int spriteDirection)
        {
            Texture2D BossheadTex = mod.GetTexture(path + "_Head_Boss");
            Vector2 Center = bodyPos[i] + new Vector2(projectile.width / 2, projectile.height /2);

            Vector2 bodytobody = bodyPos[i-1] - bodyPos[i];
            float dist = (bodytobody.Length() - BossheadTex.Height) / bodytobody.Length();
            float posX = bodytobody.X * dist;
            float posY = bodytobody.Y * dist;
            Center.X = Center.X + posX;
            Center.Y = Center.Y + posY;

            SpriteEffects bossHeadSpriteEffects = spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float screenScale;
            byte alpha;
			if (Main.mapStyle == 1)
			{
				screenScale = Main.mapMinimapScale;
                alpha = (byte)(255f * Main.mapMinimapAlpha);
			}
			else
			{
				screenScale = Main.mapOverlayScale;
				alpha = (byte)(255f * Main.mapOverlayAlpha);
			}
            if (!Main.mapFullscreen)
			{
				if(Main.mapStyle == 1)
                {
                    float screenPosX = (Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                    float screenPosY = (Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                    float offsetX = -(screenPosX - (float)((int)((Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f))) * screenScale;
                    float offsetY = -(screenPosY - (float)((int)((Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f))) * screenScale;
                    
                    float PosX = (Center.X / 16f - (float)((int)screenPosX) + (float)Main.miniMapWidth / screenScale / 2f) * screenScale;
                    float PosY = (Center.Y / 16f - (float)((int)screenPosY) + (float)Main.miniMapHeight / screenScale / 2f) * screenScale;
                    PosX += (float)Main.miniMapX;
                    PosY += (float)Main.miniMapY;
                    PosY -= 2f * screenScale / 5f;

                    float Scale = (screenScale * 0.25f * 2f + 1f) / 3f;
					if (Scale > 1f)
					{
						Scale = 1f;
					}
                    Scale *= Main.UIScale;
                    bool flag = PosX > Main.miniMapX + 12 && PosX < Main.miniMapX + Main.miniMapWidth - 16 && PosY > Main.miniMapY + 10 && PosY < Main.miniMapY + Main.miniMapHeight - 14;
                    if (true)
                    {
                        Main.spriteBatch.Draw(BossheadTex, new Vector2(PosX + offsetX, PosY + offsetY), null, new Microsoft.Xna.Framework.Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), rotation, BossheadTex.Size() / 2f, Scale, bossHeadSpriteEffects, 0f);
                    }
                }
                else if (Main.mapStyle == 2)
				{
                    float PosX = Center.X / 16f * screenScale;
                    float PosY = Center.Y / 16f * screenScale;
                    PosX += -(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f * screenScale + (float)(Main.screenWidth / 2) + 10f * screenScale;
                    PosY += -(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f * screenScale + (float)(Main.screenHeight / 2) + 10f * screenScale;
                    PosX -= 10f * screenScale;
                    PosY -= 10f * screenScale;
                    float Scale = (screenScale * 0.2f * 2f + 1f) / 3f;
					if (Scale > 1f)
					{
						Scale = 1f;
					}
					Scale *= Main.UIScale;
                    Main.spriteBatch.Draw(BossheadTex, new Vector2(PosX, PosY), null, new Microsoft.Xna.Framework.Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), rotation, BossheadTex.Size() / 2f, Scale, bossHeadSpriteEffects, 0f);
                }
            }
            else
            {
                float PosX = Center.X / 16f * screenScale;
                float PosY = Center.Y / 16f * screenScale;
                PosX += -Main.mapFullscreenPos.X / 16f * screenScale + (float)(Main.screenWidth / 2) + 10f * screenScale;
                PosY += -Main.mapFullscreenPos.Y / 16f * screenScale + (float)(Main.screenHeight / 2) + 10f * screenScale;
                PosX -= 10f * screenScale;
                PosY -= 10f * screenScale;
                float Scale = (screenScale * 0.25f * 2f + 1f) / 3f;
				Scale = Main.UIScale;
                Main.spriteBatch.Draw(BossheadTex, new Vector2(PosX, PosY), null, new Microsoft.Xna.Framework.Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), rotation, BossheadTex.Size() / 2f, Scale, bossHeadSpriteEffects, 0f);
            }
        }
        */

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for(int i = 1; i < bodyPos.Length - 1; i ++)
            {
                Rectangle bodyhitbox = new Rectangle((int)bodyPos[i].X, (int)bodyPos[i].Y, projectile.width, projectile.height);
                if (bodyhitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public static void DrawHeadIcon(SpriteBatch spriteBatch, ref string text)
        {
            bool check = false;
            int id = 0;
            for(int i = 0; i < 200; i++)
            {
                if(Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<DeathWormHead>())
                {
                    id = (int)Main.npc[i].ai[3];
                    check = true;
                    break;
                }
            }
            if(!check || Main.projectile[id].type != ModContent.ProjectileType<DeathWormBody>())
            {
                return;
            }
            for(int i = 0; i < bodyPos.Length; i ++)
            {
                string path;
                Texture2D BossheadTex;
                float rotation;
                int spriteDirection = 1;

                Vector2 Center = bodyPos[bodyPos.Length - 1 - i] + new Vector2(Main.projectile[id].width / 2, Main.projectile[id].height /2);
                if(i < bodyPos.Length - 1)
                {
                    Vector2 bodytobody = bodyPos[bodyPos.Length - 1 - i - 1] - bodyPos[bodyPos.Length - 1 - i];

                    rotation = (float)Math.Atan2(bodytobody.Y, bodytobody.X) + 1.57f;

                    if (bodytobody.X < 0f)
                    {
                        spriteDirection = 1;

                    }
                    else
                    {
                        spriteDirection = -1;
                    }

                    int frame = 0;
                    frame = i % 2;
                    if(i == 0) frame = 2;
                    
                    path = "Bosses/Invoker/DeathWorm/DeathWormBody" + frame;
                    BossheadTex = AAModEXAI.instance.GetTexture(path + "_Head_Boss");
                    float dist = (bodytobody.Length() - BossheadTex.Height) / bodytobody.Length();
                    float posX = bodytobody.X * dist;
                    float posY = bodytobody.Y * dist;
                    Center.X = Center.X + posX;
                    Center.Y = Center.Y + posY;
                }
                else
                {
                    path = "Bosses/Invoker/DeathWorm/DeathWormHead";
                    BossheadTex = AAModEXAI.instance.GetTexture(path + "_Head_Boss");
                    spriteDirection = Main.projectile[id].spriteDirection;
                    rotation = Main.projectile[id].rotation;
                }

                SpriteEffects bossHeadSpriteEffects = spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                float screenScale;
                byte alpha;
                if (Main.mapFullscreen)
                {
                    screenScale = Main.mapFullscreenScale;
                    alpha = byte.MaxValue;
                }
                else if (Main.mapStyle == 1)
                {
                    screenScale = Main.mapMinimapScale;
                    alpha = (byte)(255f * Main.mapMinimapAlpha);
                }
                else
                {
                    screenScale = Main.mapOverlayScale;
                    alpha = (byte)(255f * Main.mapOverlayAlpha);
                }
                if (!Main.mapFullscreen)
                {
                    if(Main.mapStyle == 1)
                    {
                        float screenPosX = (Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                        float screenPosY = (Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                        float offsetX = -(screenPosX - (float)((int)((Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f))) * screenScale;
                        float offsetY = -(screenPosY - (float)((int)((Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f))) * screenScale;
                        
                        float PosX = (Center.X / 16f - (float)((int)screenPosX) + (float)Main.miniMapWidth / screenScale / 2f) * screenScale;
                        float PosY = (Center.Y / 16f - (float)((int)screenPosY) + (float)Main.miniMapHeight / screenScale / 2f) * screenScale;
                        PosX += (float)Main.miniMapX;
                        PosY += (float)Main.miniMapY;
                        PosY -= 2f * screenScale / 5f;

                        float Scale = (screenScale * 0.25f * 2f + 1f) / 3f;
                        if (Scale > 1f)
                        {
                            Scale = 1f;
                        }
                        Scale *= Main.UIScale;
                        bool flag = PosX > Main.miniMapX + 12 && PosX < Main.miniMapX + Main.miniMapWidth - 16 && PosY > Main.miniMapY + 10 && PosY < Main.miniMapY + Main.miniMapHeight - 14;
                        if (flag)
                        {
                            spriteBatch.Draw(BossheadTex, new Vector2(PosX + offsetX, PosY + offsetY), null, new Microsoft.Xna.Framework.Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), rotation, BossheadTex.Size() / 2f, Scale, bossHeadSpriteEffects, 0f);
                            float left = PosX - (BossheadTex.Width / 2) * Scale;
                            float right = PosY - (BossheadTex.Height / 2) * Scale;
                            float bottom = left + BossheadTex.Width * Scale;
                            float top = right + BossheadTex.Height * Scale;
                            if (Main.mouseX >= left && Main.mouseX <= bottom && Main.mouseY >= right && Main.mouseY <= top)
                            {
                                text = Main.projectile[id].Name;
                            }
                        }
                    }
                    else if (Main.mapStyle == 2)
                    {
                        float PosX = Center.X / 16f * screenScale;
                        float PosY = Center.Y / 16f * screenScale;
                        PosX += -(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f * screenScale + (float)(Main.screenWidth / 2) + 10f * screenScale;
                        PosY += -(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f * screenScale + (float)(Main.screenHeight / 2) + 10f * screenScale;
                        PosX -= 10f * screenScale;
                        PosY -= 10f * screenScale;
                        float Scale = (screenScale * 0.2f * 2f + 1f) / 3f;
                        Scale *= Main.UIScale;
                        spriteBatch.Draw(BossheadTex, new Vector2(PosX, PosY), null, new Microsoft.Xna.Framework.Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), rotation, BossheadTex.Size() / 2f, Scale, bossHeadSpriteEffects, 0f);
                    }
                }
                else
                {
                    float PosX = Center.X / 16f * screenScale;
                    float PosY = Center.Y / 16f * screenScale;
                    PosX += -Main.mapFullscreenPos.X * screenScale + (float)(Main.screenWidth / 2) + 10f * screenScale;
                    PosY += -Main.mapFullscreenPos.Y * screenScale + (float)(Main.screenHeight / 2) + 10f * screenScale;
                    //PosX += -(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f * screenScale + (float)(Main.screenWidth / 2) + 10f * screenScale;
                    //PosY += -(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f * screenScale + (float)(Main.screenHeight / 2) + 10f * screenScale;

                    PosX -= 10f * screenScale;
                    PosY -= 10f * screenScale;
                    float Scale = (screenScale * 0.25f * 2f + 1f) / 3f;
                    Scale *= Main.UIScale;
                    spriteBatch.Draw(BossheadTex, new Vector2(PosX, PosY), null, new Microsoft.Xna.Framework.Color((int)alpha, (int)alpha, (int)alpha, (int)alpha), rotation, BossheadTex.Size() / 2f, Scale, bossHeadSpriteEffects, 0f);
                    float left = PosX - (BossheadTex.Width / 2) * Scale;
                    float right = PosY - (BossheadTex.Height / 2) * Scale;
                    float bottom = left + BossheadTex.Width * Scale;
                    float top = right + BossheadTex.Height * Scale;
                    if (Main.mouseX >= left && Main.mouseX <= bottom && Main.mouseY >= right && Main.mouseY <= top)
                    {
                        text = Main.projectile[id].Name;
                    }
                }
            }
        }
    }
}

