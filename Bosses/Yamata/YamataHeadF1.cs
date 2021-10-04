using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using AAModEXAI.Bosses.Yamata.Awakened;

using System.IO;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata
{
    [AutoloadBossHead]
    public class YamataHeadF1 : ModNPC
    {
		public bool isAwakened = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yamata");
            Main.npcFrameCount[npc.type] = 3;
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 45000;
            npc.width = 64;
            npc.height = 48;
            npc.npcSlots = 0;
            npc.dontCountMe = true;
            npc.noTileCollide = true;
            npc.boss = false;
            npc.noGravity = true;
            npc.damage = 140;
            npc.defense = 100;
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/YamataRoar");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public float[] customAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(customAI[0]);
                writer.Write(customAI[1]);
                writer.Write(customAI[2]);
                writer.Write(customAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                customAI[0] = reader.ReadFloat();
                customAI[1] = reader.ReadFloat();
                customAI[2] = reader.ReadFloat();
                customAI[3] = reader.ReadFloat();
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

		public Yamata Body = null;
        public Yamata Head = null;
        public bool killedbyplayer = true;	
		public bool leftHead = false;
        public bool fireAttack = false;
		public int distFromBodyX = 110; //how far from the body to centeralize the movement points. (X coord)
		public int distFromBodyY = 150; //how far from the body to centeralize the movement points. (Y coord)
		public int movementVariance = 60; //how far from the center point to move.

        public override void AI()
        {
            npc.defDamage = isAwakened ? 200 : 180;
            if (Body == null)
            {
                NPC npcBody = Main.npc[(int)npc.ai[0]];
                if (npcBody.type == mod.NPCType("Yamata") || npcBody.type == mod.NPCType("YamataA"))
                {
                    Body = (Yamata)npcBody.modNPC;
                }
            }
            if (Body == null)
                return;			

            npc.alpha = Body.npc.alpha;

            if (npc.alpha > 0)
            {
                npc.damage = 0;
            }
            else
            {
                npc.damage = npc.defDamage;
            }
            npc.TargetClosest();
            Player targetPlayer = Main.player[npc.target];
            if (targetPlayer == null || !targetPlayer.active || targetPlayer.dead) targetPlayer = null; //deliberately set to null


            float playerDistance = targetPlayer == null ? 99999f : Vector2.Distance(targetPlayer.Center, npc.Center);
            if (!Body.npc.active)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) //force a kill to prevent 'ghost hands'
                {
                    npc.life = 0;
                    npc.checkDead();
                    npc.netUpdate = true;
                    killedbyplayer = false;
                }
                return;
            }
            Vector2 nextTarget = Body.npc.Center + new Vector2(leftHead ? -distFromBodyX : distFromBodyX, -distFromBodyY) + new Vector2(npc.ai[2], npc.ai[3]);
            float dist = Vector2.Distance(nextTarget, npc.Center);
            if (YamataHead.EATTHELITTLEMAGGOT && playerDistance < 300f)
            {
                BaseAI.AIFlier(npc, ref customAI, true, .5f, .8f, 5, 5, false, 300);
            }
            else
            if (dist < 60f)
            {
                npc.velocity *= 0.81f;
                if (Math.Abs(npc.velocity.X) < 0.05f) npc.velocity.X = 0f;
                if (Math.Abs(npc.velocity.Y) < 0.05f) npc.velocity.Y = 0f;
            }
            else
            {
                npc.velocity = Vector2.Normalize(nextTarget - npc.Center);
                npc.velocity *= 20f;
            }
            npc.position += Body.npc.position - Body.npc.oldPosition;
            npc.spriteDirection = -1;
            if (Body.TeleportMe1)
            {
                Body.TeleportMe1 = false;
                npc.Center = Body.npc.Center;
                return;
            }
            if (Body.TeleportMe2)
            {
                Body.TeleportMe2 = false;
                npc.Center = Body.npc.Center;
                return;
            }
            if (Body.TeleportMe3)
            {
                Body.TeleportMe3 = false;
                npc.Center = Body.npc.Center;
                for (int i = 0; i < 5; ++i)
                {
                    Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 20);
                    Vector2 dir = Vector2.Normalize(targetPlayer.Center - npc.Center);
                    dir *= 5f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X, dir.Y, isAwakened ? mod.ProjectileType("YamataABreath") : mod.ProjectileType("YamataBreath"), npc.damage / 4, 0f, Main.myPlayer);
                }
                return;
            }
            if (Body.TeleportMe4)
            {
                Body.TeleportMe4 = false;
                npc.Center = Body.npc.Center;
                return;
            }
            if (Body.TeleportMe5)
            {
                Body.TeleportMe5 = false;
                npc.Center = Body.npc.Center;
                return;
            }
            if (Body.TeleportMe6)
            {
                Body.TeleportMe6 = false;
                npc.Center = Body.npc.Center;
                return;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && !YamataHead.EATTHELITTLEMAGGOT)
            {
                if (npc.alpha <= 0)
                {
                    npc.ai[1]++;
                }
                int aiTimerFire = npc.whoAmI % 3 == 0 ? 50 : npc.whoAmI % 2 == 0 ? 150 : 100;
                if (leftHead) aiTimerFire += 30;
                if (targetPlayer != null && npc.ai[1] == aiTimerFire)
                {
                    fireAttack = true;
                    for (int i = 0; i < 5; ++i)
                    {
                        Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 20);
                        Vector2 dir = Vector2.Normalize(targetPlayer.Center - npc.Center);
                        dir *= 5f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X, dir.Y, isAwakened ? mod.ProjectileType("YamataABreath") : mod.ProjectileType("YamataBreath"), npc.damage / 4, 0f, Main.myPlayer);
                    }
                }
                else
                if (npc.ai[1] >= 200) //pick random spot to move head to
                {
                    fireAttack = false;
                    npc.ai[1] = 0;
                    npc.ai[2] = Main.rand.Next(-movementVariance, movementVariance);
                    npc.ai[3] = Main.rand.Next(-movementVariance, movementVariance);
                    npc.netUpdate = true;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (fireAttack || YamataHead.EATTHELITTLEMAGGOT)
            {
                if (npc.frameCounter < 5)
                {
                    npc.frame.Y = 1 * frameHeight;
                }
                else if (npc.frameCounter < 10)
                {
                    npc.frame.Y = 2 * frameHeight;
                }
            }
            else
            {
                npc.frameCounter = 0;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                CombatText.NewText(npc.getRect(), new Color(45, 46, 70), AAMod.Lang.BossChat("YamataHead"), true, true);
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity), mod.ProjectileType("Yanker"), npc.damage / 2, 0f, Main.myPlayer);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
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

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
        
		

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType("Yamata")) || NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                return false;
            }
            return true;
        }

        private int HomeOnTarget()
        {
            const float homingMaximumRangeInPixels = 400;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.type == NPCID.Bunny)
                {
                    float distance = npc.Distance(n.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                        (
                            selectedTarget == -1 || //there is no selected target
                            npc.Distance(Main.npc[selectedTarget].Center) > distance) 
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }
    }
}
