using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI
{
    public class AAGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool title = false;

        public override void SetDefaults(NPC npc)
		{
            //AH
            if(npc.type == ModContent.NPCType<Bosses.AH.Ashe.Ashe>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AH");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "AHBag").item.type;
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.AH.Haruka.Haruka>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AH");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "AHBag").item.type;
                return;
            }
            //Akuma
            if(npc.type == ModContent.NPCType<Bosses.Akuma.Akuma>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Akuma");
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Akuma.Awakened.AkumaA>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Akuma2");
                npc.modNPC.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Ancients");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "AkumaBag").item.type;
                return;
            }
            //Anubis
            if(npc.type == ModContent.NPCType<Bosses.Anubis.Anubis>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Anubis");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "AnubisBag").item.type;
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Anubis.Forsaken.ForsakenAnubis>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AnubisA");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "FAnubisBag").item.type;
                return;
            }
            

            
            //Rajah

            if(npc.type == ModContent.NPCType<Bosses.Rajah.Rajah>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/RajahTheme");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "RajahBag").item.type;
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Rajah.SupremeRajah>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/RajahTheme");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "RajahCache").item.type;
                return;
            }

            //Shen
            if(npc.type == ModContent.NPCType<Bosses.Shen.Shen>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Shen");
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Shen.ShenA>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ShenA");
                npc.modNPC.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SuperAncients");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "ShenCache").item.type;
                return;
            }

            //Yamata
            if(npc.type == ModContent.NPCType<Bosses.Yamata.Yamata>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata");
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Yamata.Awakened.YamataA>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata2");
                npc.modNPC.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Ancients");
                npc.modNPC.bossBag = ModSupport.GetModItem("AAMod", "YamataBag").item.type;
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Yamata.Awakened.YamataAHead>())
            {
                npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata2");
                npc.modNPC.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Ancients");
                return;
            }

		}

        public override void NPCLoot(NPC npc)
		{
            //AH
            if(npc.type == ModContent.NPCType<Bosses.AH.Ashe.Ashe>())
            {
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AsheTrophy").item.type);
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.AH.Haruka.Haruka>())
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "HarukaTrophy").item.type);
                return;
            }
            //Akuma
            if(npc.type == ModContent.NPCType<Bosses.Akuma.Akuma>())
            {
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AkumaTrophy").item.type);
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AkumaMask").item.type);
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Akuma.Awakened.AkumaA>())
            {
                if (!AAModEXAIWorld.downedAkuma)
                {
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "DraconianRune").item.type);
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AkumaATrophy").item.type);
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AkumaAMask").item.type);
                for(int i = 0; i < 10; i++) npc.DropBossBags();
                return;
            }
            //Anubis
            if(npc.type == ModContent.NPCType<Bosses.Anubis.Anubis>())
            {
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AnubisTrophy").item.type);
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "AnubisMask").item.type);
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Anubis.Forsaken.ForsakenAnubis>())
            {
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "FAnubisTrophy").item.type);
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "FAnubisMask").item.type);
                return;
            }
            //Rajah
            if(npc.type == ModContent.NPCType<Bosses.Rajah.Rajah>())
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "RajahTrophy").item.type);
                return;
            }
            if(npc.type == ModContent.NPCType<Bosses.Rajah.SupremeRajah>())
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModSupport.GetModItem("AAMod", "RajahTrophy").item.type);
                return;
            }

		}

        public override void AI(NPC npc)
        {
            if(npc.type == ModContent.NPCType<Bosses.AH.AHSpawn>())
            {
                if (npc.ai[1] == 820)
                {
                    npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AH");
                }
            }
            if(npc.type == ModContent.NPCType<Bosses.Akuma.AkumaTransition>())
            {
				if (npc.ai[0] >= 300) //after he says 'heh' on the server, change music on the client
				{
					npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Akuma2");
				}		
            }
            if(npc.type == ModContent.NPCType<Bosses.Anubis.Anubis>())
            {
                if (((Bosses.Anubis.Anubis)npc.modNPC).internalAI[0] == 1)
                {
                    npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Anubis");
                }
            }
            if(npc.type == ModContent.NPCType<Bosses.Anubis.Forsaken.FATransition>())
            {
                if (npc.ai[1] == 120)
                {
                    npc.modNPC.music = ModSupport.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AnubisA");
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            //AH
            if(npc.type == ModSupport.GetModNPC("AAMod", "Ashe").npc.type)
            {
                npc.dontTakeDamage = true;
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int id = ChangeToSAABoss(npc.whoAmI, ModContent.NPCType<Bosses.AH.Ashe.Ashe>());
                    if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                    Main.npc[id].netUpdate = true;
                }
                AAModEXAI.ShowSistersTitle(npc);
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModSupport.GetModNPC("AAMod", "Haruka").npc.type)
            {
                npc.dontTakeDamage = true;
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int id = ChangeToSAABoss(npc.whoAmI, ModContent.NPCType<Bosses.AH.Haruka.Haruka>());
                    if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                    Main.npc[id].netUpdate = true;
                }
                npc.netUpdate = true;
                return false;
            }
            //Akuma
            if(npc.type == ModSupport.GetModNPC("AAMod", "Akuma").npc.type)
            {
                npc.dontTakeDamage = true;
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int id = ChangeToSAABoss(npc.whoAmI, ModContent.NPCType<Bosses.Akuma.Akuma>());
                    if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                    Main.npc[id].netUpdate = true;
                }
                AAModEXAI.ShowTitle(npc, 7);
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModSupport.GetModNPC("AAMod", "AkumaA").npc.type)
            {
                npc.dontTakeDamage = true;
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int id = ChangeToSAABoss(npc.whoAmI, ModContent.NPCType<Bosses.Akuma.Awakened.AkumaA>());
                    if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                    Main.npc[id].netUpdate = true;
                }
                AAModEXAI.ShowTitle(npc, 8);
                npc.netUpdate = true;
                return false;
            }
            //Anubis
            if(npc.type == ModSupport.GetModNPC("AAMod", "Anubis").npc.type)
            {
                npc.dontTakeDamage = true;
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int id = ChangeToSAABoss(npc.whoAmI, ModContent.NPCType<Bosses.Anubis.Anubis>());
                    if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                    Main.npc[id].netUpdate = true;
                }
                AAModEXAI.ShowTitle(npc, 1);
                npc.netUpdate = true;
                return false;
            }
            if(npc.type == ModSupport.GetModNPC("AAMod", "ForsakenAnubis").npc.type)
            {
                npc.dontTakeDamage = true;
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int id = ChangeToSAABoss(npc.whoAmI, ModContent.NPCType<Bosses.Anubis.Forsaken.ForsakenAnubis>());
                    if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                    Main.npc[id].netUpdate = true;
                }
                AAModEXAI.ShowTitle(npc, 4);
                npc.netUpdate = true;
                return false;
            }

            if(npc.type == ModLoader.GetMod("AAMod").NPCType("Athena"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
            if(npc.type == ModLoader.GetMod("AAMod").NPCType("DaybringerHead"))
            {
                npc.boss = false;
                npc.life = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
            if(npc.type == mod.NPCType("Athena"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 2);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("AthenaA"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 5);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Greed"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 3);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Anubis"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 1);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("ForsakenAnubis"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 4);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == ModContent.NPCType<Bosses.AH.Ashe.Ashe>())
            {
                if(!title)
                {
                    AAModEXAI.ShowSistersTitle(npc);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Akuma"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 7);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == ModContent.NPCType<Bosses.Akuma.Awakened.AkumaA>())
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 8);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("DaybringerHead"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 17);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("GreedA"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 6);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("SupremeRajah"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 13);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Rajah"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 18);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Shen"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 14);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("ShenA"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 15);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("YamataA"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 10);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Yamata"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 9);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("Zero"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 11);
                    title = true;
                }
                return base.PreAI(npc);
            }
            if(npc.type == mod.NPCType("ZeroProtocol"))
            {
                if(!title)
                {
                    AAModEXAI.ShowTitle(npc, 12);
                    title = true;
                }
                return base.PreAI(npc);
            }
            return base.PreAI(npc);
        }
        
        public static int ChangeToSAABoss(int num, int Type)
		{
            if (num >= 0)
			{
                float X = Main.npc[num].Center.X;
                float Y = Main.npc[num].Center.Y;
                int Target = Main.npc[num].target;
				Main.npc[num].SetDefaults(Type, -1f);
				Main.npc[num].Center = new Vector2(X, Y);
				Main.npc[num].active = true;
				Main.npc[num].wet = Collision.WetCollision(Main.npc[num].position, Main.npc[num].width, Main.npc[num].height);
				Main.npc[num].target = Target;
				return num;
			}
            Main.npc[num].active = false;
            return 0;
        }
    }
}
