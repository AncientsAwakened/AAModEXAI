using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.MushroomMonarch
{
    internal class FakeMonarchMushroom : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 14400;
        }

        private bool isGrabbing = false;

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
            if(isGrabbing)
            {
                projectile.velocity.X = 0f;
                projectile.velocity.Y = 0f;
            }
			return false;
		}

        public override void AI()
        {
            if (!NPC.AnyNPCs(mod.NPCType("MushroomMonarch")))
            {
                projectile.Kill();
                return;
            }

            float num = 0.1f;
            float num2 = 7f;

            projectile.velocity.Y = projectile.velocity.Y + num;
            if (projectile.velocity.Y > num2)
            {
                projectile.velocity.Y = num2;
            }
            projectile.velocity.X = projectile.velocity.X * 0.95f;
            if (projectile.velocity.X < 0.1 && projectile.velocity.X > -0.1)
            {
                projectile.velocity.X = 0f;
            }

            Vector2 tile = new Vector2(projectile.Center.X, projectile.Center.Y + projectile.height / 2);
            bool tileCheck = TileID.Sets.Platforms[Main.tile[(int)(tile.X / 16), (int)(tile.Y / 16)].type];
            if (tileCheck) 
            {
                projectile.velocity.X = 0f;
                projectile.velocity.Y = 0f;
            }

            for(int i = 0; i < 200; i++)
            {
                if(Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() < 88)
                {
                    if (Main.player[i].position.X + Main.player[i].width * 0.5 > projectile.position.X + projectile.width * 0.5)
                    {
                        if (projectile.velocity.X < 4f + Main.player[i].velocity.X)
                        {
                            projectile.velocity.X = projectile.velocity.X + 0.45f;
                        }
                        if (projectile.velocity.X < 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X + 0.45f * 0.75f;
                        }
                    }
                    else
                    {
                        if (projectile.velocity.X > -4f + Main.player[i].velocity.X)
                        {
                            projectile.velocity.X = projectile.velocity.X - 0.45f;
                        }
                        if (projectile.velocity.X > 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X - 0.45f * 0.75f;
                        }
                    }
                    if (Main.player[i].position.Y + Main.player[i].height * 0.5 > projectile.position.Y + projectile.height * 0.5)
                    {
                        if (projectile.velocity.Y < 4f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y + 0.45f;
                        }
                        if (projectile.velocity.Y < 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y + 0.45f * 0.75f;
                        }
                    }
                    else
                    {
                        if (projectile.velocity.Y > -4f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y - 0.45f;
                        }
                        if (projectile.velocity.Y > 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y - 0.45f * 0.75f;
                        }
                    }
                    isGrabbing = true;
                }

                if(Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() < 10)
                {
                    Main.PlaySound(SoundID.Item2, projectile.position);
                    Main.player[i].HealEffect(-15, false);
                    Main.player[i].statLife -= 15;
                    NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, i, -5, 0f, 0f, 0, 0, 0);
                    if (Main.player[i].statLife <= 0)
                    {
                        Main.player[i].KillMe(PlayerDeathReason.ByProjectile(i, projectile.whoAmI), 1000.0, 0, false);
                    }
                    projectile.Kill();
                }
            }
            
        }
    }
}