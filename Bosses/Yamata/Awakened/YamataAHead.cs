using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.ID;

using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    [AutoloadBossHead]
    public class YamataAHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
			base.SetStaticDefaults();
            DisplayName.SetDefault("Yamata no Orochi");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * .8f);
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 650000;
			npc.damage = 210;
			npc.defense = 100;
            npc.dontTakeDamage = true;
            npc.width = 78;
            npc.height = 60;
            npc.npcSlots = 0;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.DeathSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/YamataRoar");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata2");
            npc.knockBackResist *= 0.05f;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public YamataA Body = null;

        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
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
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.alpha == 0;
        }

        bool spawnHaruka = false;

        public override void AI()
        {
            if (Body == null)
            {
                NPC npcBody = Main.npc[(int)npc.ai[0]];
                if (npcBody.type == mod.NPCType("YamataA"))
                {
                    Body = (YamataA)npcBody.modNPC;
                }
            }
            if (Body == null)
                return;

            npc.alpha = Body.npc.alpha;


            if (!Body.npc.active)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) //force a kill to prevent 'ghost hands'
                {
                    npc.life = 0;
                    npc.checkDead();
                    npc.netUpdate = true;
                }
                return;
            }

            //npc.realLife = Body.npc.whoAmI;
            npc.timeLeft = 100;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            if (Yamata.TeleportMeBitch)
            {
                YamataA.TeleportMeBitch = false;
                npc.Center = Body.npc.Center;
                return;
            }

            Laugh();

            int roarSound = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Item, "Sounds/Sounds/YamataRoar");

            if (!player.active || player.dead || !Body.npc.active)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead || !Body.npc.active)
                {
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            npc.rotation = 0;
            Vector2 nextTarget = new Vector2(Body.npc.Center.X + npc.ai[1], Body.npc.Center.Y + npc.ai[2]);
            float dist = Vector2.Distance(nextTarget, npc.Center);
            if (dist < 100)
            {
                npc.velocity *= 0.9f;
                if (Math.Abs(npc.velocity.X) < 0.05f) npc.velocity.X = 0f;
                if (Math.Abs(npc.velocity.Y) < 0.05f) npc.velocity.Y = 0f;
            }
            else if (dist < 500)
            {
                npc.velocity = Vector2.Normalize(nextTarget - npc.Center);
                npc.velocity *= 10f;
            }
            else
            {
                npc.velocity = Vector2.Normalize(nextTarget - npc.Center);
                npc.velocity *= 30f;
            }
            //npc.position += Body.position - Body.oldPosition;

            switch ((int)internalAI[0])
            {
                case 0: //while other heads are charging
                    if (internalAI[3] == 0)
                    {
                        internalAI[3] = 1;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("YamataHarukaProj"), npc.damage / 4, 0f, Main.myPlayer, npc.target);
                    }
                    if (++internalAI[2] > 60)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY * 5, mod.ProjectileType("YamataAShockBomb"), npc.damage / 6, 0f, Main.myPlayer, npc.target);
                    }
                    if (++internalAI[1] > 180)
                    {
                        if (laughTimer <= 0)
                        {
                            laughTimer = 120;
                        }
                        internalAI[0]++;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        internalAI[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 1: //while other heads are shooting waveray
                    if (++internalAI[1] > 300)
                    {
                        internalAI[0]++;
                        internalAI[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 2: //shoot shit
                    if (++internalAI[2] > 20)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 5f, mod.ProjectileType("YamataAVenom2"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    if (++internalAI[1] > 240)
                    {
                        internalAI[0]++;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 3: //breathe lingering flame
                    if (++internalAI[2] > 60)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 7f, mod.ProjectileType("YamataABomb"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    if (++internalAI[1] > 180)
                    {
                        if (laughTimer <= 0)
                        {
                            laughTimer = 120;
                        }
                        internalAI[0]++;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 4: //shoot direct aim deathrays
                    if (internalAI[1] == npc.ai[3] * 60 - 30)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center), mod.ProjectileType("YamataDeathraySmall"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                    }
                    if (++internalAI[1] > 360)
                    {
                        internalAI[0]++;
                        internalAI[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 5: //shoot the shit again
                    goto case 2;

                case 6: //drop meteor that creates ripples across ground
                    if (++internalAI[2] > 90)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            for (int i = -1; i <= 1; i++)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center).RotatedBy(MathHelper.ToRadians(i * 5)) * 5f, mod.ProjectileType("YamataAVenom2"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    if (++internalAI[1] > 420)
                    {
                        if (laughTimer <= 0)
                        {
                            laughTimer = 120;
                        }
                        internalAI[0]++;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 7: //pause, let previous waves disperse
                    if (++internalAI[1] > 120)
                    {
                        internalAI[0]++;
                        internalAI[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 8: //breathe the lingering shit
                    goto case 3;

                case 9: //some mix of 2 attacks he already does, something homing + something directly aimed
                    if (--internalAI[2] < 0)
                    {
                        internalAI[2] = 120;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 7f, mod.ProjectileType("YamataABomb"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    if (++internalAI[1] > 360)
                    {
                        internalAI[0]++;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 10: //shoot the shit again
                    goto case 2;

                default:
                    internalAI[0] = 0;
                    npc.netUpdate = true;
                    goto case 0;
            }
            
            if (YamataA.TeleportMeBitch)
            {
                YamataA.TeleportMeBitch = false;
                npc.Center = Body.npc.Center;
                return;
            }
        }

        int laughTimer = 0;
        bool Laughing = false;

        public void Laugh()
        {
            if (laughTimer > 0 && !Laughing)
            {
                CombatText.NewText(npc.getRect(), new Color(146, 30, 68), "NYEH", true, true);
                Laughing = true;
            }
            else if (laughTimer <= 0)
            {
                Laughing = false;
            }
            if (Laughing)
            {
                laughTimer--;
                if (laughTimer % 20 == 0 && laughTimer != 120)
                {
                    CombatText.NewText(npc.getRect(), new Color(146, 30, 68), "HEH", true, true);
                }
            }
        }
    }
}
