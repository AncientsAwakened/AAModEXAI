
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.AH
{
    public class AHSpawn : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sisters of Discord");
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            {
                npc.width = 100;
                npc.height = 100;
                npc.friendly = false;
                npc.lifeMax = 1;
                npc.dontTakeDamage = true;
                npc.noGravity = true;
                npc.aiStyle = -1;
                npc.timeLeft = 10;
                
                for (int k = 0; k < npc.buffImmune.Length; k++)
                {
                    npc.buffImmune[k] = true;
                }
            }
        }
        public bool ATransitionActive = false;
        public int RVal = 255;
        public int BVal = 0;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(RVal, 125, BVal);
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            if (AAConfigClient.Instance.NoBossDialogue)
            {
                SpawnBoss(player, "Ashe");
                SpawnBoss2(player, "Haruka");
                npc.active = false;
            }
            npc.ai[1]++;

            npc.Center = player.Center;

            if (npc.ai[1] == 60)          //if the timer has gotten to 7.5 seconds, this happens (60 = 1 second)
            {
                if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn1"), new Color(102, 20, 48));
                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ChaosSissy");
            }

            if (npc.ai[1] == 300)
            {
                if (AAWorld.downedBrood)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn2"), new Color(102, 20, 48));
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn3"), new Color(102, 20, 48));
                }
            }

            if (npc.ai[1] == 500)
            {
                if (AAWorld.downedHydra)
                {
                    if (AAWorld.downedBrood)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn4"), new Color(72, 78, 117));
                    }
                    else
                    {
                        if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn5"), new Color(72, 78, 117));
                    }
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn6"), new Color(72, 78, 117));
                }
            }

            if (npc.ai[1] == 550)
            {
                NPC.NewNPC((int)npc.position.X - 200, (int)npc.position.Y - 150, mod.NPCType("AsheSpawn"), 0, npc.whoAmI);
            }

            if (npc.ai[1] == 700)
            {
                if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn7"), new Color(102, 20, 48));
            }

            if (npc.ai[1] == 550)
            {
                NPC.NewNPC((int)npc.position.X + 200, (int)npc.position.Y - 150, mod.NPCType("HarukaSpawn"), 0, npc.whoAmI);
            }

            if (npc.ai[1] == 820)
            {
                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AH");
                Main.npc[BaseAI.GetNPC(npc.Center, mod.NPCType("AsheSpawn"), -1)].Transform(mod.NPCType("Ashe"));
                if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn8"), new Color(102, 20, 48));
                SpawnBoss(player, "Ashe");
            }

            if (npc.ai[1] >= 960)
            {
                if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("AHSpawn9"), new Color(72, 78, 117));
                Main.npc[BaseAI.GetNPC(npc.Center, mod.NPCType("HarukaSpawn"), -1)].Transform(mod.NPCType("Haruka"));
                SpawnBoss(player, "Haruka");
                npc.active = false;
            }
        }

        public void SpawnBoss(Player player, string name)
        {
            if (Main.netMode != 1)
            {
                int bossType = mod.NPCType(name);
                if (NPC.AnyNPCs(bossType)) { return; } //don't spawn if there's already a boss!
                int npcID = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, bossType, 0);
                Main.npc[npcID].Center = player.Center - new Vector2(MathHelper.Lerp(-100f, 100f, (float)Main.rand.NextDouble()), 800f);
                Main.npc[npcID].netUpdate2 = true; Main.npc[npcID].netUpdate = true;
            }
        }

        public void SpawnBoss2(Player player, string name)
        {
            if (Main.netMode != 1)
            {
                int bossType = mod.NPCType(name);
                if (NPC.AnyNPCs(bossType)) { return; } //don't spawn if there's already a boss!
                int npcID = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, bossType, 0);
                Main.npc[npcID].Center = player.Center - new Vector2(800f, 0);
                Main.npc[npcID].netUpdate2 = true; Main.npc[npcID].netUpdate = true;
            }
        }


    }
}