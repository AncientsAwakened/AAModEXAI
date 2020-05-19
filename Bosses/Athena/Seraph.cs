using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAModEXAI.Bosses.Athena.Olympian;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Athena
{
	public class Seraph : ModNPC
	{
        public override void SetStaticDefaults()
		{
            Main.npcFrameCount[npc.type] = 4;		
		}			
		
        public override void SetDefaults()
        {
            npc.width = 60;
            npc.height = 40;
            npc.value = BaseUtility.CalcValue(0, 0, 10, 0);
            npc.npcSlots = 1;
			npc.aiStyle = -1;
            npc.lifeMax = 500;
            npc.defense = 20;
            npc.damage = 55;
            npc.knockBackResist = 0.3f;
			npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noTileCollide = true;
            if (npc.type == ModContent.NPCType<SeraphA>())
            {
                npc.alpha = 255;
            }
            banner = npc.type;
			bannerItem = mod.ItemType("SeraphBanner");
        }

        public override bool PreAI()
        {
            if (npc.type == ModContent.NPCType<SeraphA>() && !(NPC.AnyNPCs(ModContent.NPCType<Athena>()) || NPC.AnyNPCs(ModContent.NPCType<AthenaA>())))
            {
                npc.velocity.Y -= .2f;
                npc.velocity.X *= .95f;
                if (npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != NetmodeID.MultiplayerClient) { BaseAI.KillNPC(npc); npc.netUpdate = true; }
                return false;
            }
            return true;
        }

		public override void AI()
		{

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            Player player = Main.player[npc.target];

            BaseAI.AIFlier(npc, ref npc.ai, true, 0.15f, 0.08f, 8f, 7f, false, 300);

            if (npc.alpha > 0)
            {
                npc.alpha -= 4;
            }
            else
            {
                npc.alpha = 0;
            }

            if (npc.ai[3]++ > 30 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int projType = ModContent.ProjectileType<SeraphFeather>();
                float spread = 30f * 0.0174f;
                Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                dir *= 14f;
                float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                double deltaAngle = spread / 6f;
                for (int i = 0; i < 3; i++)
                {
                    double offsetAngle = startAngle + (deltaAngle * i);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 4, 2, Main.myPlayer);
                }
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }

            if (!player.GetModPlayer<AAPlayer>().ZoneAcropolis || player.dead)
            {
                npc.TargetClosest();
                if (!player.GetModPlayer<AAPlayer>().ZoneAcropolis || player.dead)
                {
                    if (!player.GetModPlayer<AAPlayer>().ZoneAcropolis)
                    {
                        CombatText.NewText(npc.Hitbox, Color.CadetBlue, SeraphBitching(), true);
                    }
                    else if (player.dead)
                    {
                        CombatText.NewText(npc.Hitbox, Color.CadetBlue, SeraphBitchingKill(), true);
                    }
                    for (int a = 0; a < 8; a++)
                    {
                        Dust.NewDust(npc.Center, 60, 40, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0);
                    }
                    BaseAI.KillNPC(npc);
                }
            }

            npc.spriteDirection = npc.direction;
            npc.rotation = npc.velocity.X * 0.05f;
        }

		public override void FindFrame(int frameHeight)
		{
            if (npc.velocity.X > 0f)
            {
                npc.spriteDirection = 1;
            }
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = -1;
            }
            npc.rotation = npc.velocity.X * 0.1f;
            npc.frameCounter += 1.0;
            if (npc.frameCounter >= 6.0)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(30) <= SeraphChance.SeraphKills && !NPC.AnyNPCs(ModContent.NPCType<SeraphHurt>()))
            {
                SeraphChance.SeraphKills = 0;
                int a = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SeraphHurt>());
                Main.npc[a].velocity = npc.velocity;
            }
            SeraphChance.SeraphKills++;
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SeraphFeather"));
        }

        public string SeraphBitching()
        {
            switch (Main.rand.Next(5))
            {
                case 0: return AAMod.Lang.EnemyChat("SeraphChat1");
                case 1: return AAMod.Lang.EnemyChat("SeraphChat2");
                case 2: return AAMod.Lang.EnemyChat("SeraphChat3");
                case 3: return AAMod.Lang.EnemyChat("SeraphChat4");
                default: return AAMod.Lang.EnemyChat("SeraphChat5");
            }
        }
        public string SeraphBitchingKill()
        {
            switch (Main.rand.Next(5))
            {
                case 0: return AAMod.Lang.EnemyChat("SeraphKillChat1");
                case 1: return AAMod.Lang.EnemyChat("SeraphKillChat2");
                case 2: return AAMod.Lang.EnemyChat("SeraphKillChat3");
                case 3: return AAMod.Lang.EnemyChat("SeraphKillChat4");
                default: return AAMod.Lang.EnemyChat("SeraphKillChat5");
            }
        }
    }

    public class SeraphChance : ModWorld
    {
        public static int SeraphKills = 0;

        public override void Initialize()
        {
            SeraphKills = 0;
        }

        public override void PostUpdate()
        {
            if (SeraphKills > 30)
            {
                SeraphKills = 30;
            }
        }
    }
}