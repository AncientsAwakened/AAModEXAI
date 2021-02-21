using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;
using AAMod.Items.Boss.Rajah;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RoyalRabbitShooter1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Rabbit Seven Shooter");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 58;
            npc.aiStyle = -1;
            npc.damage = 150;
            npc.defense = 130;
            npc.lifeMax = 70000;
            npc.knockBackResist = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.value = 0;
            npc.netAlways = true;
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

            if(npc.Center.Y > player.Center.Y - 100f && npc.Center.Y < player.Center.Y + 100f)
            {
                if(npc.ai[0] ++ > 15)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int shootdirection = npc.Center.X < player.Center.X ? 1 : -1;
                        float SpeedX = 24f * shootdirection + Main.rand.Next(-25, 26) * 0.05f;
                        float SpeedY = 0 + Main.rand.Next(-25, 26) * 0.05f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX, SpeedY, mod.ProjectileType("RajahEXShootProj"), (int)(npc.damage / 2 * 1.5f), 5, Main.myPlayer);
                        for (int i = 0; i <= 6; i++)
                        {
                            float SpeedNewX = 24f * shootdirection+ Main.rand.Next(-45, 46) * 0.05f;
                            float SpeedNewY = 0 + Main.rand.Next(-45, 46) * 0.05f;
                            int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedNewX, SpeedNewY, ProjectileID.Bullet, npc.damage / 2, 5, Main.myPlayer);
                            Main.projectile[proj].friendly = false;
                            Main.projectile[proj].hostile = true;
                        }
                    }
                    Main.PlaySound(SoundID.Item36, npc.Center);
                    npc.ai[0] = 0;
                }
            }

            Vector2 targetPos = player.Center;
            targetPos.X += 500 * (npc.Center.X < targetPos.X ? -1 : 1);

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
                bool check = Main.npc[num92].type == npc.type;
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
