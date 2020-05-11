using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI
{
    public class AAGlobalNPC : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Athena"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Athena"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("AthenaA"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AthenaA"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Greed"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Greed"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Anubis") && npc.boss)
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Anubis"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("ForsakenAnubis"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ForsakenAnubis"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Ashe"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Ashe"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Haruka"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Haruka"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Akuma"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Akuma"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("AkumaA"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AkumaA"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("DaybringerHead"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DaybringerHead"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("NightcrawlerHead"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NightcrawlerHead"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("GreedA"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("GreedA"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("SupremeRajah"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SupremeRajah"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Shen"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Shen"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("ShenA"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenA"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("YamataA"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataA"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Yamata"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Yamata"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Zero"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Zero"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("ZeroProtocol"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ZeroProtocol"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            return base.PreAI(npc);
        }
    }
}
