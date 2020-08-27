
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Athena
{
    public class ElderDragon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elder Dragon");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 38;
            npc.aiStyle = 0;
            npc.damage = 30;
            npc.defense = 30;
            npc.lifeMax = 800;
            npc.HitSound = SoundID.DD2_WyvernHurt;
            npc.DeathSound = SoundID.DD2_WyvernDeath;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0.05f;
            npc.npcSlots = 0f;
            npc.lavaImmune = true;
            npc.netAlways = true;
            banner = npc.type;
			bannerItem = mod.ItemType("ElderDragonBanner");
        }

        public override void AI()
        {
            BaseAI.AIFlier(npc, ref npc.ai, true, 0.8f, 0.04f, 8f, 7f, false, 300);
            Player player = Main.player[npc.target];
            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }
            npc.frameCounter++;
            if (npc.frameCounter >= 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 96;
                if (npc.frame.Y > (96 * 3))
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 0;
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe || !Main.hardMode)
            {
                return 0f;
            }
            return SpawnCondition.Sky.Chance * 0.10f;
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DragonSpirit"));
        }
    }
}