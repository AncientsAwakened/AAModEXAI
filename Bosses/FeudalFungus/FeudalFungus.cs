using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;

namespace AAModEXAI.Bosses.FeudalFungus
{
    [AutoloadBossHead]
    public class FeudalFungus : ModNPC
    {
        public int damage = 0;

		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if(Main.netMode == NetmodeID.Server || Main.dedServ)
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
			if(Main.netMode == NetmodeID.MultiplayerClient)
			{
				internalAI[0] = reader.ReadFloat();
				internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
                internalAI[4] = reader.ReadFloat();
            }	
		}	

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feudal Fungus");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 1200;   //boss life
            npc.damage = 24;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0f;   //this boss will behavior like the DemonEye  //boss frame/animation 
            npc.value = Item.sellPrice(0, 0, 50, 0);
            npc.aiStyle = 26;
            npc.width = 74;
            npc.height = 108;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            bossBag = ModLoader.GetMod("AAMod").ItemType("FungusBag");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Fungus");
            npc.alpha = 255;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public static int AISTATE_HOVER = 0, AISTATE_FLIER = 1, AISTATE_SHOOT = 2;
		public float[] internalAI = new float[5];
		
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
            Player player = Main.player[npc.target];
             
            if ((Main.dayTime && player.position.Y < Main.worldSurface) || !player.ZoneGlowshroom)
            {
                npc.velocity *= 0;

                if (npc.velocity.X <= .1f && npc.velocity.X >= -.1f)
                {
                    npc.velocity.X = 0;
                }
                if (npc.velocity.Y <= .1f && npc.velocity.Y >= -.1f)
                {
                    npc.velocity.Y = 0;
                }

                npc.alpha += 10;

                if (npc.alpha >= 255)
                {
                    npc.active = false;
                }
                return;
            }
            npc.alpha -= 10;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            npc.frameCounter++;
            if (npc.frameCounter >= 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 90;
                if (npc.frame.Y > (90 * 7))
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 0;
                }
            }

            npc.noTileCollide = true;

            if (Main.netMode != 1 && internalAI[1] != AISTATE_SHOOT)
			{
                internalAI[0]++;
                if (internalAI[0] >= 180)
                {
                    internalAI[0] = 0;
                    internalAI[1] = Main.rand.Next(3);
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
            }
			if(internalAI[1] == AISTATE_HOVER) 
            {
                BaseAI.AISpaceOctopus(npc, ref npc.ai, player.Center, 0.15f, 4f, 170, 56f, FireMagic);
            }
            else if (internalAI[1] == AISTATE_FLIER) 
            {
                BaseAI.AIFlier(npc, ref npc.ai, true, 0.1f,0.04f, 5f, 3f, false, 1);
            }
            else if (internalAI[1] == AISTATE_SHOOT)
            {
                BaseAI.AISpaceOctopus(npc, ref npc.ai, player.Center, 0.15f, 4f, 170, 56f, null);
                if (Main.netMode != 1)
                {
                    internalAI[0]++;
                }
                if (internalAI[0] >= 60)
                {
                    int attack = Main.rand.Next(4);
                    internalAI[1] = Main.rand.Next(3);
                    internalAI[0] = 0;
                    FungusAttack(attack);
                    npc.netUpdate = true;
                }
            }

            npc.rotation = 0;

            if (internalAI[4] ++ > 20 && Main.expertMode && Main.netMode != 1)
            {
                internalAI[4] = 0;
                Vector2 pos = new Vector2(player.Center.X + Main.rand.Next(70, 150) * (Main.rand.Next(2) == 0? 1: -1), player.Center.Y + Main.rand.Next(70, 150) * (Main.rand.Next(2) == 0? 1: -1));
                Vector2 velocity = Vector2.Normalize(player.Center - pos) * .1f;
                int proj = Projectile.NewProjectile(pos.X, pos.Y, velocity.X, velocity.Y, mod.ProjectileType("FungusCloud"), damage, 0, Main.myPlayer, 0f, 0f);
                Main.projectile[proj].timeLeft = 720;
                Main.projectile[proj].alpha = 255;
            }
        }


        public float[] shootAI = new float[4];

        public void FireMagic(NPC npc, Vector2 velocity)
        {
            Player player = Main.player[npc.target];
            BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjType("Mushshot"), ref shootAI[0], 5, damage, 8f, false, new Vector2(20f, 15f));
        }

        public override void BossLoot(ref string name, ref int potionType)
        {   //boss drops
            potionType = ItemID.ManaPotion;
            AAWorld.downedFungus = true;
            Projectile.NewProjectile(npc.Center, npc.velocity, mod.ProjectileType("FungusIGoNow"), 0, 0, 255, npc.scale);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("GlowingSporeSac"), Main.rand.Next(30, 35));
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("FungusTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("FungusMask"));
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("GlowingMushium"), Main.rand.Next(25, 35));
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public void FungusAttack(int Attack)
        {
            if (Attack == 0)
            {
                if (NPC.CountNPCS(mod.NPCType("Mushling")) < 4)
                {
                    for (int i = 0; i < (Main.expertMode ? 3 : 2); i++)
                    {
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Mushling"));
                    }
                }
                else
                {
                    float spread = 12f * 0.0174f;
                    double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread / 2;
                    double deltaAngle = spread / (Main.expertMode ? 5 : 4);
                    double offsetAngle;
                    for (int i = 0; i < (Main.expertMode ? 5 : 4); i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 6f), (float)(Math.Cos(offsetAngle) * 6f), mod.ProjectileType("FungusCloud"), damage, 0, Main.myPlayer, 0f, 1f);
                    }
                }
            }
            else if (Attack == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FungusFlier"));
                }
            }
            else if (Attack == 2)
            {
                float spread = 12f * 0.0174f;
                double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread / 2;
                double deltaAngle = spread / (Main.expertMode ? 5 : 4);
                double offsetAngle;
                for (int i = 0; i < (Main.expertMode ? 5 : 4); i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 6f), (float)(Math.Cos(offsetAngle) * 6f), mod.ProjectileType("FungusCloud"), damage, 0, Main.myPlayer, 0f, 1f);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FungusSpore"), 0, i);
                }
            }
        }

        public void MoveToPoint(Vector2 point, bool goUpFirst = false)
        {
            float moveSpeed = 4f;
            if (moveSpeed == 0f || npc.Center == point) return; //don't move if you have no move speed
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

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/FeudalFungus_Glow");
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 8, npc.frame, npc.GetAlpha(dColor), true);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 8, npc.frame, AAColor.Glow, true);
            return false;
        }
    }

    
}


