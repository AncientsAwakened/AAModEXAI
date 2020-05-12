using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AAMod;

namespace AAModEXAI
{
    public class AAGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool title = false;
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
                AAModEXAI.ShowTitle(npc, 2);
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
                AAModEXAI.ShowTitle(npc, 5);
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
                AAModEXAI.ShowTitle(npc, 3);
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
                AAModEXAI.ShowTitle(npc, 1);
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
                AAModEXAI.ShowTitle(npc, 4);
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
                AAModEXAI.ShowSistersTitle(npc);
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
                AAModEXAI.ShowTitle(npc, 7);
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
                AAModEXAI.ShowTitle(npc, 8);
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
                AAModEXAI.ShowTitle(npc, 17);
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
                AAModEXAI.ShowTitle(npc, 6);
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
                AAModEXAI.ShowTitle(npc, 13);
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Rajah"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Rajah"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                AAModEXAI.ShowTitle(npc, 18);
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
                AAModEXAI.ShowTitle(npc, 14);
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
                AAModEXAI.ShowTitle(npc, 15);
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
                AAModEXAI.ShowTitle(npc, 9);
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
                AAModEXAI.ShowTitle(npc, 10);
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
                AAModEXAI.ShowTitle(npc, 11);
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("ZeroProtocol"))
            {
                npc.boss = false;
                npc.life = 0;
                for(int proj = 0; proj < 1000; proj ++)
                {
                    if (Main.projectile[proj].active && Main.projectile[proj].friendly && !Main.projectile[proj].hostile)
                    {
                        Main.projectile[proj].hostile = true;
                        Main.projectile[proj].friendly = false;
                        Vector2 vector = Main.projectile[proj].Center - npc.Center;
                        vector.Normalize();
                        Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        reflectvelocity.Normalize();
                        reflectvelocity *= vector.Length();
                        reflectvelocity += vector * 20f;
                        reflectvelocity.Normalize();
                        reflectvelocity *= vector.Length();
                        if(reflectvelocity.Length() < 20f)
                        {
                            reflectvelocity.Normalize();
                            reflectvelocity *= 20f;
                        }

                        Main.projectile[proj].penetrate = 1;

                        Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                        Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                        Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                    }
                }
                if(Main.netMode != 1)
                {
                    int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ZeroProtocol"));
                    Main.npc[id].position = npc.position;
                    npc.netUpdate = true;
                }
                AAModEXAI.ShowTitle(npc, 12);
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            return base.PreAI(npc);
        }
    }
}
