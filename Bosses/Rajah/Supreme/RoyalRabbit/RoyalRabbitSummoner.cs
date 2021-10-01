using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RoyalRabbitSummoner : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Rabbit Lung Summoner");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 32;
            npc.height = 58;
            npc.aiStyle = -1;
            npc.damage = 250;
            npc.defense = 200;
            npc.lifeMax = 200000;
            npc.knockBackResist = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.value = 0;
            npc.netAlways = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SuperAncients");
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 8f)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y > frameHeight * 2)
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override void AI()
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            
            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.Center.X > player.Center.X ? 1 : -1;

            if(npc.ai[0] ++ > 30)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num184 = -1;
                    int num185 = -1;
                    int num74 = ModContent.ProjectileType<RabbitDragonHead>();
                    int num76 = npc.damage;
                    float num77 = 5f;
                    Vector2 vector2 = npc.Center;
                    for (int num186 = 0; num186 < 1000; num186++)
                    {
                        if (Main.projectile[num186].active && Main.projectile[num186].owner == Main.myPlayer)
                        {
                            if (num184 == -1 && Main.projectile[num186].type == ModContent.ProjectileType<RabbitDragonHead>())
                            {
                                num184 = num186;
                            }
                            if (num185 == -1 && Main.projectile[num186].type == ModContent.ProjectileType<RabbitDragonTail>())
                            {
                                num185 = num186;
                            }
                            if (num184 != -1 && num185 != -1)
                            {
                                break;
                            }
                        }
                    }


                    if (num184 == -1 && num185 == -1)
                    {
                        float num81 = 0f;
                        float num82 = 0f;
                        vector2.X = Main.mouseX + Main.screenPosition.X;
                        vector2.Y = Main.mouseY + Main.screenPosition.Y;
                        int num187 = Projectile.NewProjectile(vector2.X, vector2.Y, num81, num82, num74, num76, num77, Main.myPlayer, npc.whoAmI, 0f);
                        num187 = Projectile.NewProjectile(vector2.X, vector2.Y, num81, num82, ModContent.ProjectileType<RabbitDragonBody>(), num76, num77, Main.myPlayer, num187, 0f);
                        int num188 = num187;
                        for (int z = 0; z < (int)(player.maxMinions + 5) * 2; z++)
                        {
                            num187 = Projectile.NewProjectile(vector2.X, vector2.Y, num81, num82, ModContent.ProjectileType<RabbitDragonBody>(), num76, num77, Main.myPlayer, num187, 0f);
                            Main.projectile[num188].localAI[1] = num187;
                            num188 = num187;
                        }
                        num187 = Projectile.NewProjectile(vector2.X, vector2.Y, num81, num82, ModContent.ProjectileType<RabbitDragonTail>(), num76, num77, Main.myPlayer, num187, 0f);
                        Main.projectile[num188].localAI[1] = num187;
                    }
                }
                Main.PlaySound(SoundID.Item44, npc.Center);
                npc.ai[0] = 0;
            }

            Vector2 targetPos = player.Center;
            targetPos.X += 700 * (npc.Center.X < targetPos.X ? -1 : 1);

            if (npc.Distance(targetPos) > 50)
            {
                float speedModifier = .5f;
                if (npc.Center.X < targetPos.X)
                {
                    npc.velocity.X += speedModifier;
                    if (npc.velocity.X < 0)
                        npc.velocity.X += speedModifier * 1.3f;
                }
                else
                {
                    npc.velocity.X -= speedModifier;
                    if (npc.velocity.X > 0)
                        npc.velocity.X -= speedModifier * 1.3f;
                }
                if (npc.Center.Y < targetPos.Y)
                {
                    npc.velocity.Y += speedModifier;
                    if (npc.velocity.Y < 0)
                        npc.velocity.Y += speedModifier * 1.3f;
                }
                else
                {
                    npc.velocity.Y -= speedModifier;
                    if (npc.velocity.Y > 0)
                        npc.velocity.Y -= speedModifier * 1.3f;
                }
            }

            for (int num92 = 0; num92 < 200; num92 ++)
            {
                bool check = npc.boss;
                if (num92 != npc.whoAmI && Main.npc[num92].active && check && Math.Abs(npc.position.X - Main.npc[num92].position.X) + Math.Abs(npc.position.Y - Main.npc[num92].position.Y) < (float)npc.width)
                {
                    if (npc.position.X < Main.npc[num92].position.X)
                    {
                        npc.velocity.X = npc.velocity.X - 0.1f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X + 0.1f;
                    }
                    if (npc.position.Y < Main.npc[num92].position.Y)
                    {
                        npc.velocity.Y = npc.velocity.Y - 0.1f;
                    }
                    else
                    {
                        npc.velocity.Y = npc.velocity.Y + 0.1f;
                    }
                }
            }
        }
    }
}
