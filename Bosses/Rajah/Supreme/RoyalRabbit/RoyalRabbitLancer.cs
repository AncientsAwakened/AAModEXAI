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
    public class RoyalRabbitLancer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Rabbit Lancer");
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

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D Tex = Main.npcTexture[npc.type];
            Player player = Main.player[npc.target];
            Rectangle Rectframe = npc.frame;

            if(AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<RoyalRabbitLancerSpear>()))
            {
                Tex = mod.GetTexture("Bosses/Rajah/Supreme/RoyalRabbit/RoyalRabbitLancerAttack");
            }
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, npc.position - new Vector2(npc.direction == 1 ? 12f : -12f, 14f), npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 3, Rectframe, dColor, true);

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

            if(npc.ai[0] ++ > 23)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 speed = npc.DirectionTo(player.Center) * 7f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<RoyalRabbitLancerSpear>(), (int)(npc.damage / 2 * 1.5f), 5, Main.myPlayer, 0f, npc.whoAmI);
                }
                Main.PlaySound(SoundID.Item20, npc.Center);
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
