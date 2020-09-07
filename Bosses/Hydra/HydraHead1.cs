using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.Audio;
using System.IO;
using AAMod.NPCs.Enemies.Mire;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Hydra
{
    [AutoloadBossHead]
    public class HydraHead1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra");
            Main.npcFrameCount[npc.type] = 2;
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public float Shoot = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Shoot);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Shoot = reader.ReadFloat();
            }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 1300;
            npc.width = 42;
            npc.height = 54;
            npc.damage = 40;
            npc.npcSlots = 0;
            npc.dontCountMe = true;
            npc.noTileCollide = true;
            npc.boss = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = new LegacySoundStyle(2, 88, Terraria.Audio.SoundType.Sound);
            npc.noGravity = true;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            Head = 0;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.Hitbox, ModLoader.GetMod("AAMod").ItemType("Abyssium"), Main.rand.Next(16, 26));
            if (!Main.expertMode)
            {
                Item.NewItem(npc.Hitbox, ModLoader.GetMod("AAMod").ItemType("HydraHide"), Main.rand.Next(3, 7));
            }
            else
            {
                Item.NewItem(npc.Hitbox, ModLoader.GetMod("AAMod").ItemType("HydraHide"), Main.rand.Next(7, 17));
            }
        }

        public int Head = 0;
        public Hydra Body => (bodyNPC != null && bodyNPC.modNPC is Hydra) ? (Hydra)bodyNPC.modNPC : null;
        public NPC bodyNPC = null;
        public int damage = 0;

        public int movementVariance = 40;
        public bool fireAttack = false;

        public override bool PreAI()
        {

            return true;
        }

        public override void AI()
        {
            if (bodyNPC == null)
            {
                NPC npcBody = Main.npc[(int)npc.ai[0]];
                if (npcBody.type == mod.NPCType("Hydra"))
                {
                    bodyNPC = npcBody;
                }
            }

            if (!NPC.AnyNPCs(mod.NPCType("Hydra")))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) //force a kill to prevent 'ghosting'
                {
                    npc.life = 0;
                    npc.checkDead();
                    npc.netUpdate = true;
                }
                return;
            }
			if (bodyNPC == null)
				return;

            AssignHead();

            if (!bodyNPC.active)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) //force a kill to prevent 'ghosting'
                {
                    npc.life = 0;
                    npc.checkDead();
                    npc.netUpdate = true;
                }
                return;
            }

            npc.timeLeft = 100;

            npc.TargetClosest();
            
            Player targetPlayer = Main.player[npc.target];

            if (targetPlayer == null || !targetPlayer.active || targetPlayer.dead) targetPlayer = null; //deliberately set to null
            
            AAMod.AAPlayer modPlayer = targetPlayer.GetModPlayer<AAMod.AAPlayer>();
            if (!modPlayer.ZoneMire)
            {
                npc.damage = 80;
                npc.defense = 100;
            }
            else
            {
                npc.damage = 40;
                npc.defense = 0;
            }

            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[1]++;

                if (npc.ai[1] >= 200) //pick random spot to move head to
                {
                    npc.ai[1] = 0;
                    npc.ai[2] = Main.rand.Next(-movementVariance, movementVariance);
                    npc.ai[3] = Main.rand.Next(-movementVariance, movementVariance);
                    npc.netUpdate = true;
                }
            }

            Vector2 nextTarget = Body.npc.Center + HeadPos() + new Vector2(npc.ai[2], npc.ai[3]);

			float dist = Vector2.Distance(nextTarget, npc.Center);
            if (dist < 40f)
            {
                npc.velocity *= 0.9f;
                if (Math.Abs(npc.velocity.X) < 0.05f) npc.velocity.X = 0f;
                if (Math.Abs(npc.velocity.Y) < 0.05f) npc.velocity.Y = 0f;
            }else
            if (dist > 200f) //teleport to keep up with body
            {
                npc.Center = Body.npc.Center;
				npc.netUpdate = true;
            }	
            else
            {
                npc.velocity = Vector2.Normalize(nextTarget - npc.Center);
                npc.velocity *= 5f;
            }
            npc.position += Body.npc.position - Body.npc.oldPosition;
            npc.spriteDirection = -1;
        }

        public void AssignHead()
        {
            if (npc.type == mod.NPCType("HydraHead4") && Body.Head4 == null)
            {
                Body.Head4 = Main.npc[npc.whoAmI];
            }
            if (npc.type == mod.NPCType("HydraHead5") && Body.Head5 == null)
            {
                Body.Head5 = Main.npc[npc.whoAmI];
            }
            if (npc.type == mod.NPCType("HydraHead6") && Body.Head6 == null)
            {
                Body.Head6 = Main.npc[npc.whoAmI];
            }
            if (npc.type == mod.NPCType("HydraHead7") && Body.Head7 == null)
            {
                Body.Head7 = Main.npc[npc.whoAmI];
            }
            if (npc.type == mod.NPCType("HydraHead8") && Body.Head8 == null)
            {
                Body.Head8 = Main.npc[npc.whoAmI];
            }
            if (npc.type == mod.NPCType("HydraHead9") && Body.Head9 == null)
            {
                Body.Head9 = Main.npc[npc.whoAmI];
            }
        }

        public override void PostAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[npc.target];
                bool Red = Head == 5 || Head == 8;
                bool Yellow = Head == 4 || Head == 6;
                bool Blue = Head == 3 || Head == 7;
                bool Green = Head == 0;
                bool Orange = Head == 1;
                bool Purple = Head == 2;

                Shoot++;

                int Interval =
                    Red ? 120 :
                    Yellow ? 180 :
                    Blue ? 210 :
                    Green ? 195 :
                    Orange ? 150 :
                    Purple ? 165 :
                    210;

                int proj =
                    Red ? mod.ProjectileType("HydraBreath") :
                    Yellow ? mod.ProjectileType("AcidProj") :
                    mod.ProjectileType("HydraBomb");

                if (Green)
                {
                    proj = Main.rand.Next(2) == 0 ? mod.ProjectileType("AcidProj") : mod.ProjectileType("HydraBomb");
                }
                if (Orange)
                {
                    proj = Main.rand.Next(2) == 0 ? mod.ProjectileType("AcidProj") : mod.ProjectileType("HydraBreath");
                }
                if (Purple)
                {
                    proj = Main.rand.Next(2) == 0 ? mod.ProjectileType("HydraBomb") : mod.ProjectileType("HydraBreath");
                }

                if (Shoot == Interval)
                {
                    BaseAI.FireProjectile(player.position, npc.position, proj, npc.damage / 4, 2, 10, -1, Main.myPlayer);
                }

                if (Shoot >= Interval + 60)
                {
                    Shoot = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            bool Red = Head == 5 || Head == 8;
            bool Yellow = Head == 4 || Head == 6;
            bool Blue = Head == 3 || Head == 7;
            bool Green = Head == 0;
            bool Orange = Head == 1;
            bool Purple = Head == 2;

            int Interval =
                Red ? 120 :
                Yellow ? 180 :
                Blue ? 210 :
                Green ? 195 :
                Orange ? 150 :
                Purple ? 165 :
                210;
            if (Shoot >= Interval)
            {
                npc.frame.Y = 54;
            }
            else
            {
                npc.frame.Y = 0;
            }
        }

        public Vector2 HeadPos()
        {
            switch (Head)
            {
                default:
                    return new Vector2(0, -110);
                case 1:
                    return new Vector2(80, -100);
                case 2:
                    return new Vector2(-80, -100);
                case 3:
                    return new Vector2(-30, -110);
                case 4:
                    return new Vector2(30, -110);
                case 5:
                    return new Vector2(70, -100);
                case 6:
                    return new Vector2(90, -90);
                case 7:
                    return new Vector2(-70, -100);
                case 8:
                    return new Vector2(-90, -90);
            }
        }

        public float moveSpeed = 16f; 
        public void MoveToPoint(Vector2 point)
        {
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }

        public override bool PreNPCLoot()
        {
            if (bodyNPC != null || NPC.AnyNPCs(mod.NPCType("Hydra")))
            {
                if (npc.type == mod.NPCType("HydraHead1"))
                {
                    int a = NPC.NewNPC((int)bodyNPC.Center.X, (int)bodyNPC.Center.Y, mod.NPCType("HydraHead4"), 0, bodyNPC.whoAmI);
                    Body.Head4 = Main.npc[a];
                    int b = NPC.NewNPC((int)bodyNPC.Center.X, (int)bodyNPC.Center.Y, mod.NPCType("HydraHead5"), 0, bodyNPC.whoAmI);
                    Body.Head5 = Main.npc[b];
                    return false;
                }
                if (npc.type == mod.NPCType("HydraHead2"))
                {
                    int a = NPC.NewNPC((int)bodyNPC.Center.X, (int)bodyNPC.Center.Y, mod.NPCType("HydraHead6"), 0, bodyNPC.whoAmI);
                    Body.Head6 = Main.npc[a];
                    int b = NPC.NewNPC((int)bodyNPC.Center.X, (int)bodyNPC.Center.Y, mod.NPCType("HydraHead7"), 0, bodyNPC.whoAmI);
                    Body.Head7 = Main.npc[b];
                    return false;
                }
                if (npc.type == mod.NPCType("HydraHead3"))
                {
                    int a = NPC.NewNPC((int)bodyNPC.Center.X, (int)bodyNPC.Center.Y, mod.NPCType("HydraHead8"), 0, bodyNPC.whoAmI);
                    Body.Head8 = Main.npc[a];
                    int b = NPC.NewNPC((int)bodyNPC.Center.X, (int)bodyNPC.Center.Y, mod.NPCType("HydraHead9"), 0, bodyNPC.whoAmI);
                    Body.Head9 = Main.npc[b];
                    return false;
                }
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HydraGoreHead"), 1f);
            }
        }
    }
}
