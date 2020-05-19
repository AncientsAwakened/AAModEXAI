
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Shen
{
    public class ShenSpawn : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/ShenSpawn";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discord");
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 100;
            npc.alpha = 255;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ShenIntro");
            npc.lifeMax = 1000000000;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10000000;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        int dustx = 50;

        public override void AI()
        {
            if (!npc.HasNPCTarget)
            {
                npc.TargetClosest();
            }
            Player player = Main.player[npc.target];
            npc.Center = player.Center - new Vector2(0, 300f);
            npc.ai[0]++;
            if (npc.ai[0] <= 960)
            {
                for (int LOOP = 0; LOOP < 4; LOOP++)
                {
                    Dust dust1;
                    Dust dust2;
                    Vector2 position1 = new Vector2(npc.Center.X + dustx, npc.Center.Y);
                    Vector2 position2 = new Vector2(npc.Center.X - dustx, npc.Center.Y);
                    dust1 = Main.dust[Dust.NewDust(position1, 1, 1, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0, 0, 0, default, 1f)];
                    dust1.noGravity = false;
                    dust2 = Main.dust[Dust.NewDust(position2, 1, 1, ModLoader.GetMod("AAMod").DustType("YamataDust"), 0, 0, 0, default, 1f)];
                    dust2.noGravity = true;
                    dust2.scale *= 1.3f;
                    dust2.velocity.Y -= 6;
                }
            }
            else if (npc.ai[0] > 960 && npc.ai[0] < 1640)
            {
                for (int LOOP = 0; LOOP < 8; LOOP++)
                {
                    Dust dust1;
                    Vector2 position1 = npc.Center;
                    dust1 = Main.dust[Dust.NewDust(position1, 20, 20, ModLoader.GetMod("AAMod").DustType("Discord"), 0, 0, 0, default, 1f)];
                    dust1.noGravity = false;
                    dust1.scale *= 1.3f;
                    dust1.velocity.Y -= 6;
                }
            }

            if (npc.ai[0] == 180)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn1"), new Color(180, 41, 32));
            }

            if (npc.ai[0] == 360)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn2"), new Color(45, 46, 70));
            }

            if (npc.ai[0] == 540)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn3"), new Color(180, 41, 32));
            }

            if (npc.ai[0] == 720)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn4"), new Color(45, 46, 70));
            }
            if (npc.ai[0] == 900)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn5"), new Color(180, 41, 32));
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn5"), new Color(45, 46, 70));
            }

            if (dustx > 0 && npc.ai[0] >= 900)
            {
                dustx -= 1;
                if (dustx < 0)
                {
                    dustx = 0;
                }
            }

            if (npc.ai[0] == 960)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn6"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
            }

            if (npc.ai[0] == 1140)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn7"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
            }

            if (npc.ai[0] == 1320)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn8"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
            }

            if (npc.ai[0] >= 1500)
            {
                npc.alpha -= 5;
            }

            if (npc.ai[0] == 1520)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn9"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);

            }

            if (npc.ai[0] == 1700)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn10"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);

            }

            if (npc.ai[0] >= 1880)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenSpawn11"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                SummonShen();
                npc.active = false;
            }
        }

        public override bool PreAI()
        {
            if (AAConfigClient.Instance.NoBossDialogue)
            {
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                npc.Center = player.Center - new Vector2(0, 300f); ;
                npc.ai[0]++;

                if (npc.ai[0] <= 960)
                {
                    for (int LOOP = 0; LOOP < 4; LOOP++)
                    {
                        Dust dust1;
                        Dust dust2;
                        Vector2 position1 = new Vector2(npc.Center.X + dustx, npc.Center.Y);
                        Vector2 position2 = new Vector2(npc.Center.X - dustx, npc.Center.Y);
                        dust1 = Main.dust[Dust.NewDust(position1, 1, 1, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0, 0, 0, default, 1f)];
                        dust1.noGravity = false;
                        dust2 = Main.dust[Dust.NewDust(position2, 1, 1, ModLoader.GetMod("AAMod").DustType("YamataDust"), 0, 0, 0, default, 1f)];
                        dust2.noGravity = true;
                        dust2.scale *= 1.3f;
                        dust2.velocity.Y -= 6;
                    }
                }
                else if (npc.ai[0] > 960 && npc.ai[0] < 1640)
                {
                    for (int LOOP = 0; LOOP < 8; LOOP++)
                    {
                        Dust dust1;
                        Vector2 position1 = npc.Center;
                        dust1 = Main.dust[Dust.NewDust(position1, 20, 20, ModLoader.GetMod("AAMod").DustType("Discord"), 0, 0, 0, default, 1f)];
                        dust1.noGravity = false;
                        dust1.scale *= 1.3f;
                        dust1.velocity.Y -= 6;
                    }
                }

                if (npc.ai[0] >= 400)
                {
                    npc.alpha -= 5;
                }


                if (dustx > 0 && npc.ai[0] >= 900)
                {
                    dustx -= 1;
                    if (dustx < 0)
                    {
                        dustx = 0;
                    }
                }

                if (npc.ai[0] >= 600)
                {
                    SummonShen();
                    npc.active = false;
                    npc.netUpdate = true;
                }
                return false;
            }
            return true;
        }

        public void SummonShen()
        {
            AAModGlobalNPC.SpawnBoss(Main.player[npc.target], mod.NPCType("Shen"), false, npc.Center, "");
        }

        public Color GetColorAlpha()
        {
            return new Color(233, 0, 233) * (Main.mouseTextColor / 255f);
        }
        
        public float auraPercent = 0f;
        public bool auraDirection = true;
        public bool saythelinezero = false;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            if (npc.alpha <= 0)
            {
                BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, Color.White);
                BaseDrawing.DrawAura(sb, Main.npcTexture[npc.type], 0, npc, auraPercent, 1f, 0f, 0f, GetColorAlpha());
                BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, GetColorAlpha());
                return false;
            }
            return true;
        }
    }

    public class ShenDefeat : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/ShenSpawn";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discord's Defeat");
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.height = 100;
            npc.width = 444;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.alpha = 255;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if (npc.ai[1] > 240)
            {
                int i = AAWorld.downedShen ? 1 : 0;
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("ShenDeath"), 0, i);
                npc.active = false;
                npc.netUpdate = true;
            }
            else
            {
                npc.ai[1]++;
                npc.ai[0]++;
                if (npc.ai[0] > 4)
                {
                    npc.ai[0] = 0;
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 Pos = new Vector2(npc.position.X + Main.rand.Next(0, 444), npc.position.Y - Main.rand.Next(0, 100));
                        Projectile.NewProjectile(Pos, Vector2.Zero, mod.ProjectileType("ShenDeathBoom"), 0, 0, Main.myPlayer, Main.rand.Next(3));
                    }
                }
            }
        }
    }

    public class ShenDeath : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/ShenSpawn";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discord's Death");
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 100;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.alpha = 255;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ChaosSissy");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if (AAConfigClient.Instance.NoBossDialogue)
            {
                AAWorld.downedShen = true;
                npc.active = false;
                npc.netUpdate = true;
            }
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            npc.Center = player.Center;

            npc.ai[1]++;
            if (npc.ai[0] == 0)
            {
                if (npc.ai[1] == 180)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath1"), new Color(180, 41, 32), false);
                }

                if (npc.ai[1] == 360)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath2"), new Color(45, 46, 70), false);
                }

                if (npc.ai[1] == 540)
                {
                    string Name = Main.netMode != NetmodeID.SinglePlayer ? AAMod.Lang.BossChat("ShenDeath3") : player.name;
                    BaseUtility.Chat(Name + AAMod.Lang.BossChat("ShenDeath4"), new Color(180, 41, 32), false);
                }

                if (npc.ai[1] == 720)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath5"), new Color(45, 46, 70), false);
                }

                if (npc.ai[1] == 899)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath6"), new Color(45, 46, 70), false);
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath6"), new Color(180, 41, 32), false);
                }

                if (npc.ai[1] >= 900)
                {
                    AAWorld.downedShen = true;
                    npc.active = false;
                    npc.netUpdate = true;
                }
                return;
            }
            else
            {
                if (npc.ai[1] == 180)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath7"), new Color(45, 46, 70), false);
                }

                if (npc.ai[1] == 360)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath8"), new Color(180, 41, 32), false);
                }

                if (npc.ai[1] == 540)
                {
                    string Name = Main.netMode != NetmodeID.SinglePlayer ? AAMod.Lang.BossChat("ShenDeath9") : player.Male ? AAMod.Lang.BossChat("boy") : AAMod.Lang.BossChat("girl");
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath10") + Name + AAMod.Lang.BossChat("ShenDeath11"), new Color(45, 46, 70), false);
                }

                if (npc.ai[1] == 720)
                {
                    BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath12"), new Color(180, 41, 32), false);
                }

                if (npc.ai[1] == 899)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath13"), new Color(45, 46, 70), false);
                        BaseUtility.Chat(AAMod.Lang.BossChat("ShenDeath13"), new Color(180, 41, 32), false);
                    }
                }
                if (npc.ai[1] >= 900)
                {
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
        }
    }

    public class ShenTransition : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Shen/ShenTransition";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Awakening");
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 100;
            npc.friendly = false;
            npc.alpha = 255;
            npc.lifeMax = 10000000;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            npc.Center = player.Center - new Vector2(0, 300f); ;
            npc.ai[0]++;
            if (npc.timeLeft <= 10)
            {
                npc.timeLeft = 10;
            }
            if (npc.ai[0] > 350)
            {
                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ShenA");
                for (int LOOP = 0; LOOP < 8; LOOP++)
                {
                    Dust dust1;
                    Vector2 position1 = npc.Center;
                    dust1 = Main.dust[Dust.NewDust(position1, 20, 20, ModLoader.GetMod("AAMod").DustType("Discord"), 0, 0, 0, default, 1f)];
                    dust1.noGravity = false;
                    dust1.scale *= 1.3f;
                    dust1.velocity.Y -= 6;
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 375)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition1"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    npc.netUpdate = true;
                }
                if (npc.ai[0] == 475)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition2"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    npc.netUpdate = true;
                }
                if (npc.ai[0] == 600)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition3"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    npc.netUpdate = true;
                }
                if (npc.ai[0] == 820)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition4"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    npc.netUpdate = true;
                }
                if (npc.ai[0] == 960)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition5"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    npc.netUpdate = true;
                }
                if (npc.ai[0] >= 1100)
                {
                    npc.alpha -= 5;
                }
                if (npc.ai[0] == 1100)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition6"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    npc.netUpdate = true;
                }
                if (npc.ai[0] >= 1400)
                {
                    SummonShen();
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
        }

        public override bool PreAI()
        {
            if (AAConfigClient.Instance.NoBossDialogue)
            {
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                npc.Center = player.Center - new Vector2(0, 300f); ;
                npc.ai[0]++;
                if (npc.alpha < 255 && npc.ai[0] > 200)
                {
                    music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ShenA");
                    for (int LOOP = 0; LOOP < 8; LOOP++)
                    {
                        Dust dust1;
                        Vector2 position1 = npc.Center;
                        dust1 = Main.dust[Dust.NewDust(position1, 20, 20, ModLoader.GetMod("AAMod").DustType("Discord"), 0, 0, 0, default, 1f)];
                        dust1.noGravity = false;
                        dust1.scale *= 1.3f;
                        dust1.velocity.Y -= 6;
                    }
                }

                if (npc.ai[0] >= 400)
                {
                    npc.alpha -= 5;
                }

                if (npc.ai[0] >= 600)
                {
                    SummonShen();
                    npc.active = false;
                    npc.netUpdate = true;
                }
                return false;
            }
            return true;
        }

        public void SummonShen()
        {
            Player player = Main.player[npc.target];
            if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition7"), Color.Magenta.R, Color.Magenta.G, Color.Magenta.B);
            if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ShenTransition8"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);

            int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModLoader.GetMod("AAMod").ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 0);
            Main.projectile[b].Center = npc.Center;

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


            AAModGlobalNPC.SpawnBoss(player, mod.NPCType("ShenA"), false, npc.Center, "Shen Awakened", false);
        }

        public Color GetColorAlpha()
        {
            return new Color(233, 0, 233) * (Main.mouseTextColor / 255f);
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;
        public bool saythelinezero = false;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            if (npc.alpha <= 0)
            {
                BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, dColor);
                BaseDrawing.DrawAura(sb, Main.npcTexture[npc.type], 0, npc, auraPercent, 1f, 0f, 0f, GetColorAlpha());
                BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, GetColorAlpha());
                return false;
            }

            return true;
        }
    }
}