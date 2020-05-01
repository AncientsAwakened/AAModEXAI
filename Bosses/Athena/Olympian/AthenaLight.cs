using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Athena.Olympian
{
    public class AthenaLight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angel Clone");
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
            NPCID.Sets.TrailingMode[npc.type] = 1;
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
			npc.alpha = 255;
			npc.dontTakeDamage = true;
            npc.lifeMax = 2000;
            npc.aiStyle = 0;
            npc.damage = 70;
            npc.defense = 60;
            npc.knockBackResist = 0.2f;
            npc.width = 152;
            npc.height = 84;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.lavaImmune = true;
            npc.noTileCollide = true;
        }
        public override void AI()
        {
            bool Athena = NPC.AnyNPCs(mod.NPCType("AthenaA"));
            if (!Athena)
            {
                npc.life = 0;
                npc.checkDead();
            }
            else
            {
                npc.alpha = 100;
            }
            Player player = Main.player[npc.target];
            if (!Main.player[npc.target].dead)
            {
                Vector2 tPos;
                npc.ai[1] = 0;
                tPos.X = player.Center.X;
                tPos.Y = player.Center.Y - 70;
                npc.velocity.X += npc.DirectionTo(tPos).X * Vector2.Distance(npc.Center, tPos) / 600 / 2;
                npc.velocity.Y += npc.DirectionTo(tPos).Y * Vector2.Distance(npc.Center, tPos) / 600 / 2 * 3;
            }
            else
            {
                npc.velocity.Y -= npc.ai[1];
                npc.ai[1]++;
                if (npc.ai[1] > 40 && Main.netMode != 1)
                {
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 6)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 7)
            {
                npc.frame.Y = 0;
            }
        }
    }
}