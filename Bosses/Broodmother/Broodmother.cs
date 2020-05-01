using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;
using AAMod;

namespace AAModEXAI.Bosses.Broodmother
{
    [AutoloadBossHead]
    public class Broodmother : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Broodmother");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 0;
            npc.width = 130;
            npc.height = 164;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.chaseable = true;
            npc.damage = 35;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/BroodTheme");
            npc.defense = 10;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.netAlways = true;
            npc.friendly = false;
            npc.lifeMax = 6000;
            npc.value = Item.sellPrice(0, 5, 0, 0);
            npc.behindTiles = true;
            npc.knockBackResist = 0f;
            npc.HitSound = new LegacySoundStyle(3, 6, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 8, Terraria.Audio.SoundType.Sound);
            bossBag = ModLoader.GetMod("AAMod").ItemType("BroodBag");
            npc.npcSlots = 200;
        }

        public int frame = 0;

        public int FrameTex = 0;

        public Texture2D Tex = null;
        public Texture2D Glow = null;

        public int damage = 0;

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 3)
            {
                npc.frame.Y += 296;
                npc.frameCounter = 0;
                if (npc.frame.Y >= 1776)
                {
                    npc.frame.Y = 0;
                    FrameTex += 1;
                    if (FrameTex > 1)
                    {
                        FrameTex = 0;
                    }
                }
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public float[] internalAI = new float[6];
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
                writer.Write(internalAI[5]);
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
                internalAI[5] = reader.ReadFloat();
            }
        }


        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("BroodmotherTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("BroodMask"));
                }
                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("BroodEgg"));
                }
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("Incinerite"), 75, 100);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("BroodScale"), 50, 75);
            }
        }      

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (FrameTex == 0)
            {
                Tex = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Broodmother/Broodmother");
                Glow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Broodmother_Glow");
            }
            else
            {
                Tex = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Broodmother/Broodmother0");
                Glow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Broodmother0_Glow");
            }			

            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 6, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, Glow, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 6, npc.frame, ColorUtils.COLOR_GLOWPULSE, true);
            return false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
            AAWorld.downedBrood = true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
			bool isDead = npc.life <= 0;
            if (isDead)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreBack"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreHand"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreHand"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreHead"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGorePlate1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGorePlate2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGorePlate3"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk3"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk4"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk3"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGoreWingchunk4"), 1f);
                for (int m = 0; m < 12; m++)
				{
					Vector2 offset = new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height));
					Gore.NewGore(npc.position + offset, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGore3"), 1f + (float)Main.rand.NextDouble() * 0.5f);
				}
            }
			for (int m = 0; m < (isDead ? 45 : 6); m++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, isDead? 3f : 1.5f);
			}	
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.rand.Next(2) == 0 || (Main.expertMode && Main.rand.Next(0) == 0))       //Chances for it to inflict the debuff
            {
                target.AddBuff(BuffID.OnFire, Main.rand.Next(100, 180));       //Main.rand.Next part is the length of the buff, so 8.3 seconds to 16.6 seconds
            }
        }

		public int projectileInterval = 300; //how long until you fire projectiles
        private int projectileTimer = 0;
        private float pos = 250;
        private readonly int MaxMinions = Main.hardMode ? 4 : 3;
		public const float AISTATE_RUNAWAY = -1f, AISTATE_FLYABOVEPLAYER = 0f, AISTATE_FIREBREATH = 1f, AISTATE_FIREBOMB = 2f, AISTATE_SPAWNEGGS = 3f;

		public override void AI()
        {
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }
            if (internalAI[1] == AISTATE_RUNAWAY)
            {
                npc.noTileCollide = true;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                internalAI[0]++;

                if (npc.timeLeft < 10)
                    npc.timeLeft = 10;
                npc.velocity.X *= 0.9f;

                if (internalAI[0] > 300)
                {
                    npc.velocity.Y -= 0.1f;
                    if (npc.velocity.Y > 15f) npc.velocity.Y = 15f;
                    npc.rotation = 0f;
                    if(npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != 1){ BaseAI.KillNPC(npc); npc.netUpdate = true; }
                }
                return;
            }

            int Minions = NPC.CountNPCS(mod.NPCType("BroodEgg")) + NPC.CountNPCS(mod.NPCType("Broodmini"));

            if (Main.netMode != 1 && internalAI[0]++ >= 120)
            {
                internalAI[0] = 0;
                internalAI[1] = Minions < MaxMinions ? Main.rand.Next(4) : Main.rand.Next(3);
                npc.ai = new float[4];
                if (internalAI[1] == AISTATE_FLYABOVEPLAYER)
                {
                    npc.ai[1] = 1 + Main.rand.Next(2);
                }
                else
                if (internalAI[1] == AISTATE_SPAWNEGGS)
                {
                    npc.ai[1] = npc.ai[1] == 0 ? 1 : 0;
                }
                npc.netUpdate = true;
            }
            pos = npc.ai[1] == 0 ? -250 : 250;

            if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 4000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 4000f)
            {
                npc.active = false;
            }

            AAMod.AAPlayer modPlayer = Main.player[npc.target].GetModPlayer<AAMod.AAPlayer>();
            if (!modPlayer.ZoneInferno)
            {
                npc.dontTakeDamage = true;
                npc.damage = 130;
            }
            else
            {
                npc.dontTakeDamage = false;
                npc.damage = npc.defDamage;
            }

            npc.TargetClosest();
            Player player = Main.player[npc.target];
            if (internalAI[1] != AISTATE_RUNAWAY)
            {
                if (!Main.dayTime)
                {
                    internalAI[1] = AISTATE_RUNAWAY;
                    npc.ai = new float[4];
                    return;
                }
                if (player.dead || !player.active)
                {
                    npc.TargetClosest();
                    if (player.dead || !player.active)
                    {
                        internalAI[1] = AISTATE_RUNAWAY;
                        npc.ai = new float[4];
                        return;
                    }
                }
            }

            Vector2 wantedVelocity = player.Center - new Vector2(pos, 250);
            MoveToPoint(wantedVelocity);

            if (internalAI[1] == AISTATE_FIREBREATH)
            {
                npc.localAI[2] += 1f;
                if (npc.localAI[2] > 22f)
                {
                    npc.localAI[2] = 0f;
                    Main.PlaySound(SoundID.Item34, npc.position);
                }
                if (Main.netMode != 1)
                {
                    internalAI[2]++;
                    if (internalAI[2] > 10f)
                    {
                        if(Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("BroodBreath"), ref internalAI[3], 5, damage, 12, true, new Vector2(0, 40f));
                        }
                        else
                        {
                            int j = (int) npc.position.Y / 16;
                            int i = (int) player.position.Y / 16;
                            if(i > j && internalAI[2] % 90 == 0 && Main.netMode != 1)
                            {
                                for(int index = -2; index < 2; index++)
                                {
                                    for(int loop = i; loop > j; loop--)
                                    {
                                        if(Main.tile[(int) player.position.X / 16 + index * 20, loop].active() && Main.tileSolid[Main.tile[(int) player.position.X / 16 + index * 10, loop].type] && (Main.tile[(int) player.position.X / 16 + index * 20, loop + 1].active() || !Main.tileSolid[Main.tile[(int) player.position.X / 16 + index * 20, loop + 1].type]))
                                        {
                                            int id = Projectile.NewProjectile(player.position.X + index * 320, loop * 16, 0, 12f, 654, damage, 0, Main.myPlayer, 0f, 0f);
                                            Main.projectile[id].hostile = true;
                                            Main.projectile[id].friendly = false;
                                            break;
                                        }
                                    }
                                    for(int loop = i + 20; loop > j; loop--)
                                    {
                                        if(Main.tile[(int) player.position.X / 16 + index * 20 - 10, loop].active() && Main.tileSolid[Main.tile[(int) player.position.X / 16 + index * 10 - 10, loop].type] && (Main.tile[(int) player.position.X / 16 + index * 20 - 10, loop - 1].active() || !Main.tileSolid[Main.tile[(int) player.position.X / 16 + index * 20 - 10, loop - 1].type]))
                                        {
                                            int id = Projectile.NewProjectile(player.position.X + index * 320 - 160, loop * 16, 0, -12f, 654, damage, 0, Main.myPlayer, 0f, 0f);
                                            Main.projectile[id].hostile = true;
                                            Main.projectile[id].friendly = false;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (internalAI[2] > 180)
                    {
                        internalAI[0] = 0;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        npc.ai = new float[4];
                        npc.netUpdate = true;
                    }
                }
            }
            else if (internalAI[1] == AISTATE_SPAWNEGGS)
            {
                if (Main.netMode != 1)
                {
                    projectileTimer++;
                    if (projectileTimer >= projectileInterval && projectileTimer % 20 == 0)
                    {
                        if (projectileTimer > (projectileInterval + 60))
                            projectileTimer = 0;
                        Vector2 firePos = new Vector2(npc.Center.X + (32 * npc.direction), npc.Center.Y + 40f);
                        firePos = BaseUtility.RotateVector(npc.Center, firePos, npc.rotation); //+ (npc.direction == -1 ? (float)Math.PI : 0f)));
                        if (Minions < MaxMinions)
                        {
                            int NPCID = NPC.NewNPC((int)firePos.X, (int)firePos.Y, mod.NPCType("BroodEgg"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                            Main.npc[NPCID].velocity.Y = 4f;
                            Main.npc[NPCID].netUpdate = true;
                        }
                    }
                }
            }
            else if (internalAI[1] == AISTATE_FIREBOMB)
            {
                if (Main.netMode != 1) //only fire bombs when (attempting to) fly above the player
                {
                    projectileTimer++;
                    if (projectileTimer >= projectileInterval && projectileTimer % 10 == 0)
                    {
                        if (projectileTimer > (projectileInterval + 50))
                            projectileTimer = 0;
                        Vector2 dir = new Vector2(npc.velocity.X * 2f + (4f * npc.direction), npc.velocity.Y * 0.5f + 1f);
                        Vector2 firePos = new Vector2(npc.Center.X + (64 * npc.direction), npc.Center.Y + 10f);
                        firePos = BaseUtility.RotateVector(npc.Center, firePos, npc.rotation); //+ (npc.direction == -1 ? (float)Math.PI : 0f)));
                        int projID = Projectile.NewProjectile(firePos, dir, mod.ProjectileType("BroodBall"), damage, 1, 255);
                        Main.projectile[projID].netUpdate = true;
                    }
                }
            }
        }

		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 9f;
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
    }
}