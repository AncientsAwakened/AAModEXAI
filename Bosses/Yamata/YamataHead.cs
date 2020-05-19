using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

using AAModEXAI.Bosses.Yamata.Awakened;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Yamata
{
    [AutoloadBossHead]
    public class YamataHead : ModNPC
    {
        public int projDamage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yamata");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
			npc.lifeMax = 550000;
            npc.damage = 150;
            npc.defense = 100;
            npc.width = 78;
            npc.height = 60;
            npc.npcSlots = 0;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.DeathSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/YamataRoar");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public int varTime = 0;

        public int YvarOld = 0;

        public int XvarOld = 0;
        public int numberOfAttacks = 0;
        public int endAttack = 0;
        public int damage = 0;
        public float moveSpeedBoost = .04f;
        public NPC Body;
        public Yamata yamata = null;
        public bool HoriSwitch = false;
        public int f = 1;
        public float TargetDirection = (float)Math.PI / 2;
        public float s = 1;
        public static bool fireAttack;
        private int attackFrame;
        private int attackCounter;
        private int attackTimer;
        public int fireTimer = 0;
        public static bool EATTHELITTLEMAGGOT = false;
        public bool Quote1;
        public bool Quote2;
        public bool Quote3;
        public bool Quote4;
        public bool Quote5;
        public bool Quote6;
        public bool QuoteSaid;
        public static int HeadFrame = 0;

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
                writer.Write(EATTHELITTLEMAGGOT);
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
                EATTHELITTLEMAGGOT = reader.ReadBool();
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * .8f);
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
        }

        public override void AI()
        {
            int attackpower = 130;
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }
	        if (Body == null)
            {
                NPC npcBody = Main.npc[(int)npc.ai[0]];
                if (npcBody.type == mod.NPCType("Yamata") || npcBody.type == mod.NPCType("YamataA"))
                {
                    Body = npcBody;
					yamata = (Yamata)npcBody.modNPC;
                }
            }
			if(Body == null)
				return;
            if (!Body.active)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) //force a kill to prevent 'ghost hands'
                {
                    npc.life = 0;
                    npc.checkDead();
                    npc.netUpdate = true;
                }
                return;
            }

            npc.realLife = Body.whoAmI;
			npc.timeLeft = 100;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            
            npc.alpha = Body.alpha;
            if (npc.alpha > 0)
            {
                npc.damage = 0;
            }
            else
            {
                npc.damage = attackpower;
            }

            Laugh();

            int roarSound = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Item, "Sounds/Sounds/YamataRoar");

            Vector2 PlayerDistance = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num433 = 6f;
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

            if (npc.alpha <= 0)
            {
                internalAI[2]++;
            }
            if (internalAI[2] == 399)
            {
                QuoteSaid = false;
                Main.PlaySound(roarSound, npc.Center);
                internalAI[1] = Main.rand.Next(4);
            }

            if (internalAI[2] >= 400)
            {
                Attacks(internalAI[1]);
            }

            if (internalAI[2] >= 600)
            {
                EATTHELITTLEMAGGOT = false;
                internalAI[2] = 0;
            }

            if (npc.ai[3] == 1)
            {
                attackCounter++;
                if (attackCounter > 10)
                {
                    attackFrame++;
                    attackCounter = 0;
                }
                if (attackFrame >= 3)
                {
                    attackFrame = 2;
                }
            }

            if (!player.active || player.dead || !Body.active)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead || !Body.active)
                {
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
            fireTimer++;
            if (fireTimer >= 240 && npc.ai[3] == 0)
            {
                Main.PlaySound(roarSound, npc.Center);
                npc.ai[3] = 1;
                fireTimer = 0;
            }
            projDamage = npc.damage / 6;
            if (npc.ai[3] == 1)
            {
                attackTimer++;
                if (Main.rand.Next(3) == 0)
                {
                    if (attackTimer == 40)
                    {
                        Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 20);
                        int proj2 = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-20, 20), npc.Center.Y + Main.rand.Next(-20, 20), npc.velocity.X * 2f, npc.velocity.Y * 2f, mod.ProjectileType("YamataBomb"), projDamage, 0, Main.myPlayer);
                        Main.projectile[proj2].damage = projDamage;
                        attackTimer = 0;
                        attackFrame = 0;
                        attackCounter = 0;
                    }
                    if (attackTimer >= 80)
                    {
                        npc.ai[3] = 0;
                    }
                }
                else
                {
                    if (attackTimer == 8 || attackTimer == 16 || attackTimer == 24 || attackTimer == 32 || attackTimer == 40 || attackTimer == 48 || attackTimer == 56 || attackTimer == 64 || attackTimer == 72 || attackTimer == 79)
                    {
                        Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 20);
                        for (int i = 0; i < 5; ++i)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(PlayerDistance.X, PlayerDistance.Y, PlayerPosX * 2f, PlayerPosY * 2f, mod.ProjectileType("YamataBreath"), projDamage, 0f, Main.myPlayer);
                            }
                        }
                        
                    }
                    if (attackTimer >= 80)
                    {
                        npc.ai[3] = 0;
                        attackTimer = 0;
                        attackFrame = 0;
                        attackCounter = 0;
                    }
                }

            }

            Vector2 moveTo = new Vector2(Body.Center.X + npc.ai[1], Body.Center.Y - (130f + npc.ai[2])) - npc.Center;
            npc.velocity = moveTo * moveSpeedBoost;
            npc.rotation = 0;
            npc.position += Body.position - Body.oldPosition;

            if (Yamata.TeleportMeBitch)
            {
                Yamata.TeleportMeBitch = false;
                npc.Center = yamata.npc.Center;
                return;
            }
        }


        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public void Attacks(float AttackType)
        {
            Player player = Main.player[npc.target];

            bool sayQuote = Main.rand.Next(3) == 0;
            if (AttackType == 0f)
            {
                if (!QuoteSaid && sayQuote)
                {
                    laughTimer = 120;
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat((!Quote1) ? AAMod.Lang.BossChat("YamataHead1") : AAMod.Lang.BossChat("YamataHead2"), new Color(45, 46, 70));
                    QuoteSaid = true;
                    Quote1 = true;
                }
                BaseAI.ShootPeriodic(npc, new Vector2(player.position.X, player.position.Y - 1), player.width, player.height, mod.ProjectileType("YamataVenom"), ref internalAI[3], 6, projDamage, 9f, true, new Vector2(20f, 15f));
            }
            if (AttackType == 1f)
            {
                if (!QuoteSaid && sayQuote)
                {
                    laughTimer = 120;
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat((!Quote3) ? AAMod.Lang.BossChat("YamataHead3") : AAMod.Lang.BossChat("YamataHead4"), new Color(45, 46, 70));
                    QuoteSaid = true;
                    Quote3 = true;
                }
                BaseAI.ShootPeriodic(npc, new Vector2(player.position.X, -4f), player.width, player.height, mod.ProjectileType("YamataStorm"), ref internalAI[3], 40, projDamage, 10f, true, new Vector2(20f, 15f));
            }
            if (AttackType == 2f)
            {
                if (!QuoteSaid && sayQuote)
                {
                    laughTimer = 120;
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat((!Quote3) ? AAMod.Lang.BossChat("YamataHead5") : AAMod.Lang.BossChat("YamataHead6"), new Color(45, 46, 70));
                    QuoteSaid = true;
                    Quote3 = true;
                }
                BaseAI.ShootPeriodic(npc, new Vector2(player.position.X, player.position.Y - 1), player.width, player.height, mod.ProjectileType("YamataBlast"), ref internalAI[3], 15, projDamage, 10f, true, new Vector2(20f, 15f));
            }
            if (AttackType == 3f)
            {
                if (!QuoteSaid && sayQuote)
                {
                    laughTimer = 120;
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat((!Quote4) ? (AAMod.Lang.BossChat("YamataHead7") + (player.Male ? AAMod.Lang.BossChat("male2") : AAMod.Lang.BossChat("fimale2")) + AAMod.Lang.BossChat("YamataHead8")) : AAMod.Lang.BossChat("YamataHead9"), new Color(45, 46, 70));
                    QuoteSaid = true;
                    Quote4 = true;
                }
                EATTHELITTLEMAGGOT = true;
            }
        }

        int laughTimer = 0;
        bool Laughing = false;

        public void Laugh()
        {
            if (laughTimer > 0 && !Laughing)
            {
                CombatText.NewText(npc.getRect(), new Color(45, 46, 70), "NYEH", true, true);
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
                    CombatText.NewText(npc.getRect(), new Color(45, 46, 70), "HEH", true, true);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.ai[3] == 1 || npc.ai[2] >= 400)
            {
                if (npc.frameCounter++ < 5)
                {
                    npc.frame.Y = 1 * frameHeight;
                }
                else
                {
                    npc.frame.Y = 2 * frameHeight;
                }
            }
            else
            {

                npc.frame.Y = 0 * frameHeight;
                npc.frameCounter = 0;
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[npc.target];
            if (player.vortexStealthActive && projectile.ranged)
            {
                damage /= 2;
                crit = false;
            }
            if (projectile.penetrate == -1 && !projectile.minion)
            {
                damage = (int)(damage * .2f);
            }
            else if(projectile.penetrate == -1 && projectile.minion)
            {
                damage = (int)(damage * .7f);
            }
            else if (projectile.penetrate >= 1)
            {
                projectile.damage *= (int).2;
            }
            else if (projectile.type == ProjectileID.LastPrismLaser)
            {
                damage = (int)(damage * .05f);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.type == ProjectileID.LastPrismLaser)
            {
                projectile.damage = (int)(projectile.damage * .01f);
            }
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
        // We use this hook to prevent any loot from dropping. We do this because this is a multistage npc and it shouldn't drop anything until the final form is dead.
        public override bool PreNPCLoot()
        {
            return false;
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType("Yamata")) || NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                return false;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
    }
}
