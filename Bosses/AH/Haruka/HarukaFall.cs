using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.AH.Haruka
{
    public class HarukaFall : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haruka Yamata");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.noGravity = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.value = 0;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
            bossBag = ModLoader.GetMod("AAMod").ItemType("AHBag");
        }

        public override void AI()
        {
            npc.dontTakeDamage = true;

            if (npc.collideY)
            {
                npc.ai[0]++;
                if (npc.frame.Y < 78 * 4)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 78 * 4;
                }

                if (npc.frame.Y < 78 * 6)
                {
                    if (npc.frameCounter++ > 5)
                    {
                        npc.frame.Y += 78;
                        npc.frameCounter = 0;
                    }
                }

                if (npc.ai[0] == 60)
                {
                    CombatText.NewText(npc.Hitbox, new Color(72, 78, 117), "..?");
                }

                if (npc.ai[0] == 120)
                {
                    npc.frame.Y = 78 * 7;
                }
                if (npc.ai[0] == 180)
                {
                    CombatText.NewText(npc.Hitbox, new Color(72, 78, 117), "...Ashe?");
                    npc.frame.Y = 78 * 6;
                }
                if (npc.ai[0] == 240)
                {
                    npc.frame.Y = 78 * 7;
                }

                if (npc.ai[0] == 360)
                {
                    CombatText.NewText(npc.Hitbox, new Color(72, 78, 117), "...thanks for shutting her up.");

                    if (Main.expertMode)
                    {
                        for(int i = 0; i < 10; i++) npc.DropBossBags();
                    }

                    if (!Main.expertMode)
                    {
                        string[] lootTableH = { "HarukaKunai", "Masamune", "MizuArashi", "HarukaBox" };
                        int lootH = Main.rand.Next(lootTableH.Length);
                        npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTableH[lootH]));
                    }

                    if (Main.rand.Next(10) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("HarukaTrophy"));
                    }

                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HarukaVanish"), 0, 0, 4);
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
            else
            {
                if (npc.frameCounter++ > 6)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += 78;
                    if (npc.frame.Y > 78 * 3)
                    {
                        npc.frame.Y = 0;
                    }
                }
            }
        }
    }
}