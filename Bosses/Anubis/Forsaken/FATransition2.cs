using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class FATransition2 : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anubis Legendscribe");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 72;
            npc.height = 100;
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.value = 0;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        readonly int frameHeight = 100;

        public override void AI()
        {
            npc.dontTakeDamage = true;

            npc.ai[3] = 39;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                if (npc.velocity.Y == 0)
                {
                    for (int a = 0; a < 8; a++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 200, default, 1.3f);
                    }
                    npc.ai[1]++;
                    npc.frameCounter++;
                    if (npc.frameCounter > 6)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y += frameHeight;
                    }
                    if (npc.frame.Y > frameHeight * 3)
                    {
                        npc.frame.Y = 0;
                    }
                    if (npc.ai[1] >= 90)
                    {
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
                        int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModLoader.GetMod("AAMod").ProjectileType("ShockwaveBoom"), 0, 0, Main.myPlayer, 0, 10);
                        Main.projectile[b].Center = npc.Center;

                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("ForsakenAnubis"));
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
            }
        }
    }
}