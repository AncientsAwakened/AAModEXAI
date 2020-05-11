using System;
using System.IO;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Serpent
{
    [AutoloadBossHead]	
	public class SerpentHead : ModNPC
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subzero Serpent");
            Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.npcSlots = 5f;
            npc.width = 38;
            npc.height = 38;
            npc.damage = 35;
            npc.defense = 25;
            npc.lifeMax = 6000;
            npc.value = 50000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            animationType = 10;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
            npc.boss = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("SerpentBag");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Boss6");
            npc.alpha = 50;
            npc.buffImmune[BuffID.Frostburn] = true;
        }

        private bool fireAttack;
        private int attackCounter;
        private int attackTimer;

		public bool tongueFlick = false;
		public bool tongueFlickDir = false;
		public int tongueFlickCounter = 0;
        private int RunOnce = 0;
        private int StopSnow = 0;

        public float[] internalAI = new float[5];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(internalAI[4]);
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
                internalAI[3] = reader.ReadFloat();
                internalAI[4] = reader.ReadFloat();
            }
        }

        public override void AI()
        {
            Rain();
            Player player = Main.player[npc.target];
            RunAway(player);
            Attack(player);

            int tileX = (int)(npc.position.X / 16f) - 1;
            int tileCenterX = (int)(npc.Center.X / 16f) + 2;
            int tileY = (int)(npc.position.Y / 16f) - 1;
            int tileCenterY = (int)(npc.Center.Y / 16f) + 2;
            if (tileX < 0) { tileX = 0; }
            if (tileCenterX > Main.maxTilesX) { tileCenterX = Main.maxTilesX; }
            if (tileY < 0) { tileY = 0; }
            if (tileCenterY > Main.maxTilesY) { tileCenterY = Main.maxTilesY; }
            for (int tX = tileX; tX < tileCenterX; tX++)
            {
                for (int tY = tileY; tY < tileCenterY; tY++)
                {
                    Tile checkTile = BaseWorldGen.GetTileSafely(tX, tY);
                    if (checkTile != null && ((checkTile.nactive() && (Main.tileSolid[checkTile.type] || (Main.tileSolidTop[checkTile.type] && checkTile.frameY == 0))) || checkTile.liquid > 64))
                    {
                        Vector2 tPos;
                        tPos.X = tX * 16;
                        tPos.Y = tY * 16;
                        if (npc.position.X + npc.width > tPos.X && npc.position.X < tPos.X + 16f && npc.position.Y + npc.height > tPos.Y && npc.position.Y < tPos.Y + 16f)
                        {
                            if (Main.rand.Next(100) == 0 && checkTile.nactive())
                            {
                                WorldGen.KillTile(tX, tY, true, true, false);
                            }
                        }
                    }
                }
            }

            if (npc.ai[3] > 0f)
            {
                npc.realLife = (int)npc.ai[3];
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            npc.velocity.Length();
            if (Main.netMode != 1)
            {
                if (internalAI[4] != 1)
                {
                    npc.ai[3] = npc.whoAmI;
                    npc.realLife = npc.whoAmI;
                    int whoamI = npc.whoAmI;
                    int Length = 12;
                    for (int a = 0; a <= Length; a++)
                    {
                        int type = mod.NPCType("SerpentBody");
                        if (a == Length)
                        {
                            type = mod.NPCType("SerpentTail");
                        }
                        int segment = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), type, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[segment].ai[3] = npc.whoAmI;
                        Main.npc[segment].realLife = npc.whoAmI;
                        Main.npc[segment].ai[1] = whoamI;
                        Main.npc[whoamI].ai[0] = segment;
                        NetMessage.SendData(23, -1, -1, null, segment, 0f, 0f, 0f, 0, 0, 0);
                        whoamI = segment;
                    }
                    internalAI[4] = 1;
                    npc.netUpdate = true;
                }
            }
            int posX = (int)(npc.position.X / 16f);
            int centerX = (int)((npc.position.X + npc.width) / 16f);
            int posY = (int)(npc.position.Y / 16f);
            int centerY = (int)((npc.position.Y + npc.height) / 16f);
            if (posX < 0)
            {
                posX = 0;
            }
            if (centerX > Main.maxTilesX)
            {
                centerX = Main.maxTilesX;
            }
            if (posY < 0)
            {
                posY = 0;
            }
            if (centerY > Main.maxTilesY)
            {
                centerY = Main.maxTilesY;
            }
            bool inRange = false;
            if (!inRange)
            {
                for (int x = posX; x < centerX; x++)
                {
                    for (int y = posY; y < centerY; y++)
                    {
                        if (Main.tile[x, y] != null && ((Main.tile[x, y].nactive() && (Main.tileSolid[Main.tile[x, y].type] || (Main.tileSolidTop[Main.tile[x, y].type] && Main.tile[x, y].frameY == 0))) || Main.tile[x, y].liquid > 64))
                        {
                            Vector2 vector2;
                            vector2.X = x * 16;
                            vector2.Y = y * 16;
                            if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                            {
                                inRange = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!inRange)
            {
                npc.localAI[1] = 1f;
                Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int num46 = 1000;
                bool flag3 = true;
                if (npc.position.Y > Main.player[npc.target].position.Y)
                {
                    for (int target = 0; target < 255; target++)
                    {
                        if (Main.player[target].active)
                        {
                            Rectangle rectangle2 = new Rectangle((int)Main.player[target].position.X - num46, (int)Main.player[target].position.Y - num46, num46 * 2, num46 * 2);
                            if (rectangle.Intersects(rectangle2))
                            {
                                flag3 = false;
                                break;
                            }
                        }
                    }
                    if (flag3)
                    {
                        inRange = true;
                    }
                }
            }
            else
            {
                npc.localAI[1] = 0f;
            }
            float maxDistance = 16f;
            float num48 = 0.1f;
            float num49 = 0.15f;
            Vector2 center = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
            float targetY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;
            targetX = (int)(targetX / 16f) * 16;
            targetY = (int)(targetY / 16f) * 16;
            center.X = (int)(center.X / 16f) * 16;
            center.Y = (int)(center.Y / 16f) * 16;
            targetX -= center.X;
            targetY -= center.Y;
            float num52 = (float)Math.Sqrt(targetX * targetX + targetY * targetY);
            if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
            {
                try
                {
                    center = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    targetX = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - center.X;
                    targetY = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - center.Y;
                }
                catch
                {
                }
                npc.rotation = (float)Math.Atan2(targetY, targetX) + 1.57f;
                num52 = (float)Math.Sqrt(targetX * targetX + targetY * targetY);
                int num53 = (int)(44f * npc.scale);
                num52 = (num52 - num53) / num52;
                targetX *= num52;
                targetY *= num52;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + targetX;
                npc.position.Y = npc.position.Y + targetY;
                return;
            }
            if (!inRange)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.15f;
                if (npc.velocity.Y > maxDistance)
                {
                    npc.velocity.Y = maxDistance;
                }
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < maxDistance * 0.4)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X - num48 * 1.1f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X + num48 * 1.1f;
                    }
                }
                else if (npc.velocity.Y == maxDistance)
                {
                    if (npc.velocity.X < targetX)
                    {
                        npc.velocity.X = npc.velocity.X + num48;
                    }
                    else if (npc.velocity.X > targetX)
                    {
                        npc.velocity.X = npc.velocity.X - num48;
                    }
                }
                else if (npc.velocity.Y > 4f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X + num48 * 0.9f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X - num48 * 0.9f;
                    }
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num54 = num52 / 40f;
                    if (num54 < 10f)
                    {
                        num54 = 10f;
                    }
                    if (num54 > 20f)
                    {
                        num54 = 20f;
                    }
                    npc.soundDelay = (int)num54;
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
                }
                num52 = (float)Math.Sqrt(targetX * targetX + targetY * targetY);
                float num55 = Math.Abs(targetX);
                float num56 = Math.Abs(targetY);
                float num57 = maxDistance / num52;
                targetX *= num57;
                targetY *= num57;
                if (((npc.velocity.X > 0f && targetX > 0f) || (npc.velocity.X < 0f && targetX < 0f)) && ((npc.velocity.Y > 0f && targetY > 0f) || (npc.velocity.Y < 0f && targetY < 0f)))
                {
                    if (npc.velocity.X < targetX)
                    {
                        npc.velocity.X = npc.velocity.X + num49;
                    }
                    else if (npc.velocity.X > targetX)
                    {
                        npc.velocity.X = npc.velocity.X - num49;
                    }
                    if (npc.velocity.Y < targetY)
                    {
                        npc.velocity.Y = npc.velocity.Y + num49;
                    }
                    else if (npc.velocity.Y > targetY)
                    {
                        npc.velocity.Y = npc.velocity.Y - num49;
                    }
                }
                if ((npc.velocity.X > 0f && targetX > 0f) || (npc.velocity.X < 0f && targetX < 0f) || (npc.velocity.Y > 0f && targetY > 0f) || (npc.velocity.Y < 0f && targetY < 0f))
                {
                    if (npc.velocity.X < targetX)
                    {
                        npc.velocity.X = npc.velocity.X + num48;
                    }
                    else if (npc.velocity.X > targetX)
                    {
                        npc.velocity.X = npc.velocity.X - num48;
                    }
                    if (npc.velocity.Y < targetY)
                    {
                        npc.velocity.Y = npc.velocity.Y + num48;
                    }
                    else if (npc.velocity.Y > targetY)
                    {
                        npc.velocity.Y = npc.velocity.Y - num48;
                    }
                    if (Math.Abs(targetY) < maxDistance * 0.2 && ((npc.velocity.X > 0f && targetX < 0f) || (npc.velocity.X < 0f && targetX > 0f)))
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num48 * 2f;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y - num48 * 2f;
                        }
                    }
                    if (Math.Abs(targetX) < maxDistance * 0.2 && ((npc.velocity.Y > 0f && targetY < 0f) || (npc.velocity.Y < 0f && targetY > 0f)))
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num48 * 2f;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num48 * 2f;
                        }
                    }
                }
                else if (num55 > num56)
                {
                    if (npc.velocity.X < targetX)
                    {
                        npc.velocity.X = npc.velocity.X + num48 * 1.1f;
                    }
                    else if (npc.velocity.X > targetX)
                    {
                        npc.velocity.X = npc.velocity.X - num48 * 1.1f;
                    }
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < maxDistance * 0.5)
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num48;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y - num48;
                        }
                    }
                }
                else
                {
                    if (npc.velocity.Y < targetY)
                    {
                        npc.velocity.Y = npc.velocity.Y + num48 * 1.1f;
                    }
                    else if (npc.velocity.Y > targetY)
                    {
                        npc.velocity.Y = npc.velocity.Y - num48 * 1.1f;
                    }
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < maxDistance * 0.5)
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num48;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num48;
                        }
                    }
                }
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            if (inRange)
            {
                if (npc.localAI[0] != 1f)
                {
                    npc.netUpdate = true;
                }
                npc.localAI[0] = 1f;
            }
            else
            {
                if (npc.localAI[0] != 0f)
                {
                    npc.netUpdate = true;
                }
                npc.localAI[0] = 0f;
            }
            if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
            {
                npc.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (Main.netMode != 2 && !tongueFlick && Main.rand.Next(20) == 0)
            {
                tongueFlick = true;
            }
            if (tongueFlick)
            {
                if (tongueFlickDir)
                {
                    tongueFlickCounter--;
                    if (tongueFlickCounter <= 0)
                    {
                        tongueFlickCounter = 8;
                        npc.frame.Y -= npc.frame.Height;
                        if (npc.frame.Y <= 0)
                            tongueFlick = tongueFlickDir = false;
                    }
                }
                else
                {
                    tongueFlickCounter--;
                    if (tongueFlickCounter <= 0)
                    {
                        tongueFlickCounter = 8;
                        npc.frame.Y += npc.frame.Height;
                        if (npc.frame.Y >= (npc.frame.Height * 3))
                            tongueFlickDir = true;
                    }
                }
            }
        }

        public void RunAway(Player player)
        {
            if (player.dead || !player.active || !player.ZoneSnow)
            {
                npc.TargetClosest(true);
                if (player.dead || !player.active || !player.ZoneSnow)
                {
                    internalAI[0]++;
                    npc.velocity.Y = npc.velocity.Y + 0.8f;
                    if (internalAI[0] >= 300)
                    {
                        npc.active = false;
                    }
                }
                else
                {
                    internalAI[0] = 0;
                }
            }
            else
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 4;
                }
                else
                {
                    npc.alpha = 0;
                }
            }
        }

        private void Rain()
        {
            if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f || Main.player[npc.target].dead)
            {
                if (StopSnow == 0)
                {
                    RainStop();
                    StopSnow = 1;
                }
            }

            if (RunOnce == 0)
            {
                if (!Main.raining)
                {
                    int num = 86400;
                    int num5 = num / 24;
                    Main.rainTime = Main.rand.Next(num5 * 8, num);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.rainTime += Main.rand.Next(0, num5);
                    }
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.rainTime += Main.rand.Next(0, num5 * 2);
                    }
                    if (Main.rand.Next(5) == 0)
                    {
                        Main.rainTime += Main.rand.Next(0, num5 * 2);
                    }
                    if (Main.rand.Next(6) == 0)
                    {
                        Main.rainTime += Main.rand.Next(0, num5 * 3);
                    }
                    if (Main.rand.Next(7) == 0)
                    {
                        Main.rainTime += Main.rand.Next(0, num5 * 4);
                    }
                    if (Main.rand.Next(8) == 0)
                    {
                        Main.rainTime += Main.rand.Next(0, num5 * 5);
                    }
                    float num1 = 1f;
                    if (Main.rand.Next(2) == 0)
                    {
                        num1 += 0.05f;
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        num1 += 0.1f;
                    }
                    if (Main.rand.Next(4) == 0)
                    {
                        num1 += 0.15f;
                    }
                    if (Main.rand.Next(5) == 0)
                    {
                        num1 += 0.2f;
                    }
                    Main.rainTime = (int)(Main.rainTime * num1);
                    Main.raining = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                RunOnce = 1;
            }
        }

        private void RainStop()
        {
            if (Main.raining)
            {
                Main.rainTime = 0;
                Main.raining = false;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public void Attack(Player player)
        {
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }

            if (internalAI[1]++ > 300)
            {
                internalAI[1] = 0;
                internalAI[2] = Main.rand.Next(3);
                npc.netUpdate = true;
            }

            if (internalAI[2] == 0)
            {
                if (NPC.CountNPCS(mod.NPCType("IceCrystal")) < 3)
                {
                    NPC.NewNPC((int)player.position.X + Main.rand.Next(-500, 500), (int)player.position.Y + 500, mod.NPCType("IceCrystal"), 0, 0, 0, 0, 0, npc.target);
                }
                internalAI[2] = 2;
                npc.netUpdate = true;
            }
            else if (internalAI[2] == 1)
            {
                attackCounter++;
                if (attackCounter >= 180 && fireAttack == false)
                {
                    attackCounter = 0;
                    fireAttack = true;
                    npc.netUpdate = true;
                }
                if (fireAttack == true && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    attackTimer++;
                    if (attackTimer == 20 || attackTimer == 50 || attackTimer == 79)
                    {
                        BaseAI.FireProjectile(Main.player[npc.target].Center, npc, mod.ProjectileType("IceBall2"), damage, 3, 14f, 0, 0, -1);
                        npc.netUpdate = true;
                    }
                    if (attackTimer >= 80)
                    {
                        fireAttack = false;
                        attackTimer = 0;
                        attackCounter = 0;
                        npc.netUpdate = true;
                    }
                }
            }
            else
            {
                attackCounter++;
                if (attackCounter == 400 && fireAttack == false)
                {
                    attackCounter = 0;
                    fireAttack = true;
                    npc.netUpdate = true;
                }
                if (fireAttack == true)
                {
                    attackTimer++;
                    if ((attackTimer == 8 || attackTimer == 16 || attackTimer == 24 || attackTimer == 32 || attackTimer == 40 || attackTimer == 48 || attackTimer == 56 || attackTimer == 64 || attackTimer == 72 || attackTimer == 79) && !npc.HasBuff(103))
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            float num433 = 6f;
                            Vector2 PlayerDistance = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                            float PlayerPosX = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - PlayerDistance.X;
                            float PlayerPosY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - PlayerDistance.Y;
                            float PlayerPos = (float)Math.Sqrt(PlayerPosX * PlayerPosX + PlayerPosY * PlayerPosY);
                            PlayerPos = num433 / PlayerPos;
                            PlayerPosX *= PlayerPos;
                            PlayerPosY *= PlayerPos;
                            PlayerPosY += Main.rand.Next(-40, 41) * 0.01f;
                            PlayerPosX += Main.rand.Next(-40, 41) * 0.01f;
                            PlayerPosY += npc.velocity.Y * 0.5f;
                            PlayerPosX += npc.velocity.X * 0.5f;
                            PlayerDistance.X -= PlayerPosX * 1f;
                            PlayerDistance.Y -= PlayerPosY * 1f;
                            Projectile.NewProjectile(PlayerDistance.X, PlayerDistance.Y, npc.velocity.X * 2f, npc.velocity.Y * 2f, mod.ProjectileType("SnowBreath"), damage, 0, Main.myPlayer);
                        }
                    }
                    if (attackTimer >= 80)
                    {
                        fireAttack = false;
                        attackTimer = 0;
                        attackCounter = 0;
                        npc.netUpdate = true;
                    }
                }
            }

            if (internalAI[3]++ > 400 && NPC.CountNPCS(mod.NPCType("SnakeHead")) < 3)
            {
                for (int i = 0; i < 3 - NPC.CountNPCS(mod.NPCType("SnakeHead")); i++)
                {
                    AAModGlobalNPC.SpawnBoss(player, mod.NPCType("SnakeHead"), false, 0, 0, "Snake", false);
                }
                internalAI[3] = 0;
            }
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode)
			{
				player.AddBuff(BuffID.Chilled, 200, true);
			}
			else
			{
				player.AddBuff(BuffID.Chilled, 100, true);
			}
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;   //boss drops
            AAWorld.downedSerpent = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.85f);
		}

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int x = 0; x < 5; x++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("IceDust"), hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life == 0)
            {
                for (int x = 0; x < 5; x++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("SnowDustLight"), hitDirection, -1f, 0, default, 1f);
                }

                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SZSGoreHead"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (!Main.expertMode)
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("SerpentMask"));
                }
                AAWorld.downedSerpent = true;
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("SnowMana"), 10, 15);
                string[] lootTable = { "BlizardBuster", "SerpentSpike", "Icepick", "SerpentSting", "Sickle", "SickleShot", "SnakeStaff", "SubzeroSlasher" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(AAMod.Items.Vanity.Mask.SerpentMask.type, 1f / 7);
                if (Main.rand.Next(9) == 0)
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("SnowflakeShuriken"), 90, 120);
                }
                else
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
                }
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("SerpentTrophy"));
            }
            npc.value = 0f;
            npc.boss = false;
        }
    }

    public class SerpentBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Subzero Serpent");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 5f;
            npc.width = 38;
            npc.height = 38;
            npc.damage = 35;
            npc.defense = 25;
            npc.lifeMax = 6000;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            animationType = 10;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
            npc.boss = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Boss6");
            npc.alpha = 50;
            npc.dontCountMe = true;
        }

        public override void AI()
        {
            int tileX = (int)(npc.position.X / 16f) - 1;
            int tileCenterX = (int)(npc.Center.X / 16f) + 2;
            int tileY = (int)(npc.position.Y / 16f) - 1;
            int tileCenterY = (int)(npc.Center.Y / 16f) + 2;
            if (tileX < 0) { tileX = 0; }
            if (tileCenterX > Main.maxTilesX) { tileCenterX = Main.maxTilesX; }
            if (tileY < 0) { tileY = 0; }
            if (tileCenterY > Main.maxTilesY) { tileCenterY = Main.maxTilesY; }
            for (int tX = tileX; tX < tileCenterX; tX++)
            {
                for (int tY = tileY; tY < tileCenterY; tY++)
                {
                    Tile checkTile = BaseWorldGen.GetTileSafely(tX, tY);
                    if (checkTile != null && ((checkTile.nactive() && (Main.tileSolid[checkTile.type] || (Main.tileSolidTop[checkTile.type] && checkTile.frameY == 0))) || checkTile.liquid > 64))
                    {
                        Vector2 tPos;
                        tPos.X = tX * 16;
                        tPos.Y = tY * 16;
                        if (npc.position.X + npc.width > tPos.X && npc.position.X < tPos.X + 16f && npc.position.Y + npc.height > tPos.Y && npc.position.Y < tPos.Y + 16f)
                        {
                            if (Main.rand.Next(100) == 0 && checkTile.nactive())
                            {
                                WorldGen.KillTile(tX, tY, true, true, false);
                            }
                        }
                    }
                }
            }

            if (npc.ai[3] > 0f)
            {
                npc.realLife = (int)npc.ai[3];
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            npc.velocity.Length();
            bool flag = false;
            if (npc.ai[1] <= 0f)
            {
                flag = true;
            }
            else if (Main.npc[(int)npc.ai[1]].life <= 0)
            {
                flag = true;
            }
            if (flag)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.checkDead();
            }
            int num12 = (int)(npc.position.X / 16f) - 1;
            int num13 = (int)((npc.position.X + npc.width) / 16f) + 2;
            int num14 = (int)(npc.position.Y / 16f) - 1;
            int num15 = (int)((npc.position.Y + npc.height) / 16f) + 2;
            if (num12 < 0)
            {
                num12 = 0;
            }
            if (num13 > Main.maxTilesX)
            {
                num13 = Main.maxTilesX;
            }
            if (num14 < 0)
            {
                num14 = 0;
            }
            if (num15 > Main.maxTilesY)
            {
                num15 = Main.maxTilesY;
            }
            bool flag2 = false;
            if (!flag2)
            {
                for (int k = num12; k < num13; k++)
                {
                    for (int l = num14; l < num15; l++)
                    {
                        if (Main.tile[k, l] != null && ((Main.tile[k, l].nactive() && (Main.tileSolid[Main.tile[k, l].type] || (Main.tileSolidTop[Main.tile[k, l].type] && Main.tile[k, l].frameY == 0))) || Main.tile[k, l].liquid > 64))
                        {
                            Vector2 vector2;
                            vector2.X = k * 16;
                            vector2.Y = l * 16;
                            if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!flag2)
            {
                npc.localAI[1] = 1f;
            }
            else
            {
                npc.localAI[1] = 0f;
            }
            float num17 = 16f;
            if (Main.player[npc.target].dead || Main.player[npc.target].position.Y < Main.rockLayer)
            {
                flag2 = false;
                npc.velocity.Y = npc.velocity.Y + 1f;
                if (npc.position.Y > (double)((Main.maxTilesY - 200) * 16))
                {
                    npc.velocity.Y = npc.velocity.Y + 1f;
                    num17 = 32f;
                }
                if (npc.position.Y > (double)((Main.maxTilesY - 200) * 16))
                {
                    for (int a = 0; a < 200; a++)
                    {
                        if (Main.npc[a].type == mod.NPCType("ArmoredDiggerHead") || Main.npc[a].type == mod.NPCType("ArmoredDiggerBody") ||
                            Main.npc[a].type == mod.NPCType("ArmoredDiggerTail"))
                        {
                            Main.npc[a].active = false;
                        }
                    }
                }
            }
            float num18 = 0.1f;
            float num19 = 0.15f;
            Vector2 vector3 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num20 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
            float num21 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;
            num20 = (int)(num20 / 16f) * 16;
            num21 = (int)(num21 / 16f) * 16;
            vector3.X = (int)(vector3.X / 16f) * 16;
            vector3.Y = (int)(vector3.Y / 16f) * 16;
            num20 -= vector3.X;
            num21 -= vector3.Y;
            float num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
            if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
            {
                try
                {
                    vector3 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    num20 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector3.X;
                    num21 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector3.Y;
                }
                catch
                {
                }
                npc.rotation = (float)Math.Atan2(num21, num20) + 1.57f;
                num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
                int num23 = (int)(44f * npc.scale);
                num22 = (num22 - num23) / num22;
                num20 *= num22;
                num21 *= num22;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + num20;
                npc.position.Y = npc.position.Y + num21;
                return;
            }
            if (!flag2)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.15f;
                if (npc.velocity.Y > num17)
                {
                    npc.velocity.Y = num17;
                }
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num17 * 0.4)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X - num18 * 1.1f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X + num18 * 1.1f;
                    }
                }
                else if (npc.velocity.Y == num17)
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num18;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num18;
                    }
                }
                else if (npc.velocity.Y > 4f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X + num18 * 0.9f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X - num18 * 0.9f;
                    }
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num24 = num22 / 40f;
                    if (num24 < 10f)
                    {
                        num24 = 10f;
                    }
                    if (num24 > 20f)
                    {
                        num24 = 20f;
                    }
                    npc.soundDelay = (int)num24;
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
                }
                num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
                float num25 = Math.Abs(num20);
                float num26 = Math.Abs(num21);
                float num27 = num17 / num22;
                num20 *= num27;
                num21 *= num27;
                if (((npc.velocity.X > 0f && num20 > 0f) || (npc.velocity.X < 0f && num20 < 0f)) && ((npc.velocity.Y > 0f && num21 > 0f) || (npc.velocity.Y < 0f && num21 < 0f)))
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num19;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num19;
                    }
                    if (npc.velocity.Y < num21)
                    {
                        npc.velocity.Y = npc.velocity.Y + num19;
                    }
                    else if (npc.velocity.Y > num21)
                    {
                        npc.velocity.Y = npc.velocity.Y - num19;
                    }
                }
                if ((npc.velocity.X > 0f && num20 > 0f) || (npc.velocity.X < 0f && num20 < 0f) || (npc.velocity.Y > 0f && num21 > 0f) || (npc.velocity.Y < 0f && num21 < 0f))
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num18;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num18;
                    }
                    if (npc.velocity.Y < num21)
                    {
                        npc.velocity.Y = npc.velocity.Y + num18;
                    }
                    else if (npc.velocity.Y > num21)
                    {
                        npc.velocity.Y = npc.velocity.Y - num18;
                    }
                    if (Math.Abs(num21) < num17 * 0.2 && ((npc.velocity.X > 0f && num20 < 0f) || (npc.velocity.X < 0f && num20 > 0f)))
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num18 * 2f;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y - num18 * 2f;
                        }
                    }
                    if (Math.Abs(num20) < num17 * 0.2 && ((npc.velocity.Y > 0f && num21 < 0f) || (npc.velocity.Y < 0f && num21 > 0f)))
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num18 * 2f;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num18 * 2f;
                        }
                    }
                }
                else if (num25 > num26)
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num18 * 1.1f;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num18 * 1.1f;
                    }
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num17 * 0.5)
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num18;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y - num18;
                        }
                    }
                }
                else
                {
                    if (npc.velocity.Y < num21)
                    {
                        npc.velocity.Y = npc.velocity.Y + num18 * 1.1f;
                    }
                    else if (npc.velocity.Y > num21)
                    {
                        npc.velocity.Y = npc.velocity.Y - num18 * 1.1f;
                    }
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num17 * 0.5)
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num18;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num18;
                        }
                    }
                }
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.localAI[0] == 0)
            {
                npc.localAI[0] = 1;
                npc.localAI[1] = Main.rand.Next(4);
            }
            npc.frame.Y = (int)npc.localAI[1] * npc.frame.Height;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType("SerpentHead")))
            {
                return false;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int x = 0; x < 5; x++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("IceDust"), hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life == 0)
            {
                for (int x = 0; x < 5; x++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("SnowDustLight"), hitDirection, -1f, 0, default, 1f);
                }

                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SZSGoreBody"), 1f);
            }
        }
    }

    public class SerpentTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Subzero Serpent");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 5f;
            npc.width = 38;
            npc.height = 38;
            npc.damage = 35;
            npc.defense = 25;
            npc.lifeMax = 6000;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            animationType = 10;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
            npc.boss = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("SerpentBag");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Boss6");
            npc.alpha = 50;
            npc.dontCountMe = true;
        }


        public override void AI()
        {
            int tileX = (int)(npc.position.X / 16f) - 1;
            int tileCenterX = (int)(npc.Center.X / 16f) + 2;
            int tileY = (int)(npc.position.Y / 16f) - 1;
            int tileCenterY = (int)(npc.Center.Y / 16f) + 2;
            if (tileX < 0) { tileX = 0; }
            if (tileCenterX > Main.maxTilesX) { tileCenterX = Main.maxTilesX; }
            if (tileY < 0) { tileY = 0; }
            if (tileCenterY > Main.maxTilesY) { tileCenterY = Main.maxTilesY; }
            for (int tX = tileX; tX < tileCenterX; tX++)
            {
                for (int tY = tileY; tY < tileCenterY; tY++)
                {
                    Tile checkTile = BaseWorldGen.GetTileSafely(tX, tY);
                    if (checkTile != null && ((checkTile.nactive() && (Main.tileSolid[checkTile.type] || (Main.tileSolidTop[checkTile.type] && checkTile.frameY == 0))) || checkTile.liquid > 64))
                    {
                        Vector2 tPos;
                        tPos.X = tX * 16;
                        tPos.Y = tY * 16;
                        if (npc.position.X + npc.width > tPos.X && npc.position.X < tPos.X + 16f && npc.position.Y + npc.height > tPos.Y && npc.position.Y < tPos.Y + 16f)
                        {
                            if (Main.rand.Next(100) == 0 && checkTile.nactive())
                            {
                                WorldGen.KillTile(tX, tY, true, true, false);
                            }
                        }
                    }
                }
            }

            if (npc.ai[3] > 0f)
            {
                npc.realLife = (int)npc.ai[3];
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            npc.velocity.Length();
            bool headActive = false;
            if (npc.ai[1] <= 0f)
            {
                headActive = true;
            }
            else if (Main.npc[(int)npc.ai[1]].life <= 0)
            {
                headActive = true;
            }
            if (headActive)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.checkDead();
            }
            int num12 = (int)(npc.position.X / 16f) - 1;
            int num13 = (int)((npc.position.X + npc.width) / 16f) + 2;
            int num14 = (int)(npc.position.Y / 16f) - 1;
            int num15 = (int)((npc.position.Y + npc.height) / 16f) + 2;
            if (num12 < 0)
            {
                num12 = 0;
            }
            if (num13 > Main.maxTilesX)
            {
                num13 = Main.maxTilesX;
            }
            if (num14 < 0)
            {
                num14 = 0;
            }
            if (num15 > Main.maxTilesY)
            {
                num15 = Main.maxTilesY;
            }
            bool flag2 = false;
            if (!flag2)
            {
                for (int k = num12; k < num13; k++)
                {
                    for (int l = num14; l < num15; l++)
                    {
                        if (Main.tile[k, l] != null && ((Main.tile[k, l].nactive() && (Main.tileSolid[Main.tile[k, l].type] || (Main.tileSolidTop[Main.tile[k, l].type] && Main.tile[k, l].frameY == 0))) || Main.tile[k, l].liquid > 64))
                        {
                            Vector2 vector2;
                            vector2.X = k * 16;
                            vector2.Y = l * 16;
                            if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!flag2)
            {
                npc.localAI[1] = 1f;
            }
            else
            {
                npc.localAI[1] = 0f;
            }
            float num17 = 16f;
            float num18 = 0.1f;
            float num19 = 0.15f;
            Vector2 vector3 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num20 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
            float num21 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;
            num20 = (int)(num20 / 16f) * 16;
            num21 = (int)(num21 / 16f) * 16;
            vector3.X = (int)(vector3.X / 16f) * 16;
            vector3.Y = (int)(vector3.Y / 16f) * 16;
            num20 -= vector3.X;
            num21 -= vector3.Y;
            float num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
            if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
            {
                try
                {
                    vector3 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    num20 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector3.X;
                    num21 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector3.Y;
                }
                catch
                {
                }
                npc.rotation = (float)Math.Atan2(num21, num20) + 1.57f;
                num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
                int num23 = (int)(44f * npc.scale);
                num22 = (num22 - num23) / num22;
                num20 *= num22;
                num21 *= num22;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + num20;
                npc.position.Y = npc.position.Y + num21;
                return;
            }
            if (!flag2)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.15f;
                if (npc.velocity.Y > num17)
                {
                    npc.velocity.Y = num17;
                }
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num17 * 0.4)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X - num18 * 1.1f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X + num18 * 1.1f;
                    }
                }
                else if (npc.velocity.Y == num17)
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num18;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num18;
                    }
                }
                else if (npc.velocity.Y > 4f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X + num18 * 0.9f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X - num18 * 0.9f;
                    }
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num24 = num22 / 40f;
                    if (num24 < 10f)
                    {
                        num24 = 10f;
                    }
                    if (num24 > 20f)
                    {
                        num24 = 20f;
                    }
                    npc.soundDelay = (int)num24;
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
                }
                num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
                float num25 = Math.Abs(num20);
                float num26 = Math.Abs(num21);
                float num27 = num17 / num22;
                num20 *= num27;
                num21 *= num27;
                if (((npc.velocity.X > 0f && num20 > 0f) || (npc.velocity.X < 0f && num20 < 0f)) && ((npc.velocity.Y > 0f && num21 > 0f) || (npc.velocity.Y < 0f && num21 < 0f)))
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num19;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num19;
                    }
                    if (npc.velocity.Y < num21)
                    {
                        npc.velocity.Y = npc.velocity.Y + num19;
                    }
                    else if (npc.velocity.Y > num21)
                    {
                        npc.velocity.Y = npc.velocity.Y - num19;
                    }
                }
                if ((npc.velocity.X > 0f && num20 > 0f) || (npc.velocity.X < 0f && num20 < 0f) || (npc.velocity.Y > 0f && num21 > 0f) || (npc.velocity.Y < 0f && num21 < 0f))
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num18;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num18;
                    }
                    if (npc.velocity.Y < num21)
                    {
                        npc.velocity.Y = npc.velocity.Y + num18;
                    }
                    else if (npc.velocity.Y > num21)
                    {
                        npc.velocity.Y = npc.velocity.Y - num18;
                    }
                    if (Math.Abs(num21) < num17 * 0.2 && ((npc.velocity.X > 0f && num20 < 0f) || (npc.velocity.X < 0f && num20 > 0f)))
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num18 * 2f;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y - num18 * 2f;
                        }
                    }
                    if (Math.Abs(num20) < num17 * 0.2 && ((npc.velocity.Y > 0f && num21 < 0f) || (npc.velocity.Y < 0f && num21 > 0f)))
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num18 * 2f;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num18 * 2f;
                        }
                    }
                }
                else if (num25 > num26)
                {
                    if (npc.velocity.X < num20)
                    {
                        npc.velocity.X = npc.velocity.X + num18 * 1.1f;
                    }
                    else if (npc.velocity.X > num20)
                    {
                        npc.velocity.X = npc.velocity.X - num18 * 1.1f;
                    }
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num17 * 0.5)
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num18;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y - num18;
                        }
                    }
                }
                else
                {
                    if (npc.velocity.Y < num21)
                    {
                        npc.velocity.Y = npc.velocity.Y + num18 * 1.1f;
                    }
                    else if (npc.velocity.Y > num21)
                    {
                        npc.velocity.Y = npc.velocity.Y - num18 * 1.1f;
                    }
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num17 * 0.5)
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num18;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num18;
                        }
                    }
                }
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType("SerpentHead")))
            {
                return false;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int x = 0; x < 5; x++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("IceDust"), hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life == 0)
            {
                for (int x = 0; x < 5; x++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("SnowDustLight"), hitDirection, -1f, 0, default, 1f);
                }

                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SZSGoreTail"), 1f);
            }
        }
    }
}