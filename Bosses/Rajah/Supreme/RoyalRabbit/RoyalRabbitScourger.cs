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
    public class RoyalRabbitScourger : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Rabbit Scourger");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 32;
            npc.height = 68;
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

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D Tex = Main.npcTexture[npc.type];
            Player player = Main.player[npc.target];

            if(npc.Center.Y > player.Center.Y - 50f && npc.Center.Y < player.Center.Y + 50f)
            {
                Tex = mod.GetTexture("Bosses/Rajah/Supreme/RoyalRabbit/RoyalRabbitScourgerAttack");
            }
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 3, npc.frame, dColor, true);

            return false;
        }

        public override void AI()
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            
            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.Center.X > player.Center.X ? 1 : -1;

            if(npc.Center.Y > player.Center.Y - 50f && npc.Center.Y < player.Center.Y + 50f)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient && (!Main.projectile[(int)npc.ai[2]].active || Main.projectile[(int)npc.ai[2]].type != ModContent.ProjectileType<RajahScourgerLine>()))
                {
                    int shootdirection = npc.Center.X < player.Center.X ? 1 : -1;
                    float ai3 = npc.whoAmI;
                    float spread = 45f * 0.0174f;
                    float speedX = 22f * shootdirection;
                    float speedY = 0;
                    float baseSpeed = (float)Math.Sqrt((speedX * speedX) + (speedY * speedY));
                    double startAngle = Math.Atan2(speedX, speedY) - .1d;
                    double deltaAngle = spread / 6f;
                    double offsetAngle;
                    offsetAngle = startAngle + (deltaAngle * 1);
                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<RajahScourgerLine>(), npc.damage / 2, 5, Main.myPlayer, 0.0f, ai3);
                    npc.ai[2] = proj;
                }
                Main.PlaySound(SoundID.Item116, npc.Center);
            }

            Vector2 targetPos = player.Center;
            targetPos.X += 300 * (npc.Center.X < targetPos.X ? -1 : 1);

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
