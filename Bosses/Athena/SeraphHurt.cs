using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.Localization;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena
{
    public class SeraphHurt : ModNPC
	{
        public override void SetStaticDefaults()
		{
            Main.npcFrameCount[npc.type] = 5;		
		}			
		
        public override void SetDefaults()
        {
            npc.width = 60;
            npc.height = 40;
            npc.value = 0;
            npc.npcSlots = 1;
			npc.aiStyle = -1;
            npc.lifeMax = 120;
            npc.defense = 20;
            npc.damage = 55;
            npc.knockBackResist = 0.3f;
			npc.noGravity = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noTileCollide = false;
            banner = npc.type;
			bannerItem = mod.ItemType("SeraphBanner");
            npc.dontTakeDamage = true;
        }

        public Vector2 Origin = new Vector2((int)(Main.maxTilesX * 0.65f), 100) * 16;

        public override void AI()
		{
            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            Player player = Main.player[npc.target];

            npc.ai[0]++;

            if (npc.ai[0] == 120 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.netUpdate = true;
            }
            if (npc.ai[0] == 180)
            {
                CombatText.NewText(npc.Hitbox, Color.CadetBlue, SeraphBitching(), true);
                npc.netUpdate = true;
            }
            if (npc.ai[0] >= 240 && npc.dontTakeDamage && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.dontTakeDamage = false;
                npc.netUpdate = true;
            }

            if (npc.ai[0] >= 120 && npc.ai[0] < 240)
            {
                npc.velocity *= .97f;
            }
            else if (npc.ai[0] >= 240)
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                npc.dontTakeDamage = false;
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -8f) npc.velocity.Y = -8f;

                if (player.Center.X > npc.Center.X)
                {
                    npc.velocity.X -= 0.2f;
                    if (npc.velocity.X < -8f) npc.velocity.Y = -8f;
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.velocity.X += 0.2f;
                    if (npc.velocity.X > 8f) npc.velocity.Y = 8f;
                    npc.spriteDirection = -1;
                }

                Vector2 Acropolis = new Vector2(Origin.X + (80 * 16), Origin.Y + (79 * 16));

                if (Vector2.Distance(npc.Center, Acropolis) > 90 * 16 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int a = 0; a < 8; a++)
                    {
                        Dust.NewDust(npc.Center, 60, 40, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0);
                    }
                    BaseAI.KillNPC(npc); 
                    npc.netUpdate = true; 
                }
            }
            
            if (npc.ai[0] < 120 && npc.collideY)
            {
                npc.rotation += npc.velocity.X * 0.05f;
            }
            else
            {
                npc.spriteDirection = npc.direction;
                npc.rotation = npc.velocity.X * 0.05f;
            }
        }

        public string SeraphBitching()
        {
            switch (Main.rand.Next(5))
            {
                case 0: return AAMod.Lang.EnemyChat("SeraphHurtChat1");
                case 1: return AAMod.Lang.EnemyChat("SeraphHurtChat2");
                case 2: return AAMod.Lang.EnemyChat("SeraphHurtChat3");
                case 3: return AAMod.Lang.EnemyChat("SeraphHurtChat4");
                default: return AAMod.Lang.EnemyChat("SeraphHurtChat5");
            }
        }

		public override void FindFrame(int frameHeight)
		{
            if (npc.ai[0] < 120)
            {
                npc.frame.Y = 0;
            }
            else
            {
                if (npc.velocity.X > 0f)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }
                npc.rotation = npc.velocity.X * 0.1f;
                npc.frameCounter++;
                if (npc.frameCounter >= 6)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
                {
                    npc.frame.Y = frameHeight;
                }
            }
        }
    }
}