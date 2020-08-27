using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Shen.GripsShen
{
    public class AbyssGripOrbiter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyss Grip Orbiter");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 1200;
            projectile.alpha = 255;
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public float rotValue = -1f;

        public int timecount = 0;

        public int proj = 0;

        public bool shooting = false;

        public override void AI()
        {
            Vector2 Center = new Vector2();
            NPC centerNPC = Main.npc[(int)projectile.ai[0]];

            if(proj == 0) Center = centerNPC.Center;

            timecount ++;

            for(int i = 0; i<1000; i++)
            {
                if(Main.projectile[i].friendly && !Main.projectile[i].minion && Main.projectile[i].Hitbox.Intersects(projectile.Hitbox))
                {
                    Main.projectile[i].Kill();
                }
                if(Main.projectile[i].type == mod.ProjectileType("AbyssalBomb") && proj == 0)
                {
                    Center = Main.projectile[i].Center;
                    proj = Main.projectile[i].whoAmI;
                }
            }

            if (rotValue == -1f) rotValue = projectile.ai[1];
            rotValue += 0.05f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;
            



            if(proj != 0 && !shooting)
            {
                if(Main.projectile[proj].timeLeft <= 5 || !Main.projectile[proj].active) 
                {
                    projectile.velocity = 18f * Vector2.Normalize(Main.projectile[proj].DirectionTo(projectile.Center));
                    shooting = true;
                }
                else
                {
                    for (int m = projectile.oldPos.Length - 1; m > 0; m--)
                    {
                        projectile.oldPos[m] = projectile.oldPos[m - 1];
                    }
                    projectile.oldPos[0] = projectile.position;
                    Center = Main.projectile[proj].Center;
                    projectile.Center = BaseUtility.RotateVector(Center, Center + new Vector2(140f, 0f), rotValue);
                }
            }
            else if(centerNPC.active && centerNPC.life > 0 && centerNPC.type == mod.NPCType("AbyssGrip") && !shooting)
            {
                for (int m = projectile.oldPos.Length - 1; m > 0; m--)
                {
                    projectile.oldPos[m] = projectile.oldPos[m - 1];
                }
                projectile.oldPos[0] = projectile.position;
                projectile.Center = BaseUtility.RotateVector(Center, Center + new Vector2(140f, 0f), rotValue);
            }
            else if((!centerNPC.active || centerNPC.life <= 0 || centerNPC.type != mod.NPCType("AbyssGrip")) && !shooting)
            {
                projectile.velocity = 18f * Vector2.Normalize(centerNPC.DirectionTo(projectile.Center));
                shooting = true;
            }
            
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 200);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color dColor)
        {
            Color Alpha = dColor;
            if(timecount < 10)
            {
                Alpha.R = (byte)(0f);
                Alpha.G = (byte)(0f);
                Alpha.B = (byte)(0f);
                Alpha.A = (byte)(0f);
            }
            else if(timecount < 100)
            {
                Alpha.R = (byte)((float)(timecount * 2));
                Alpha.G = (byte)((float)(timecount * 2));
                Alpha.B = (byte)((float)(timecount * 2));
                Alpha.A = (byte)((float)(timecount * 2));
            }
            else
            {
                Alpha.R = (byte)200f;
                Alpha.G = (byte)200f;
                Alpha.B = (byte)200f;
                Alpha.A = (byte)200f;
            }
            int blue = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingOceanDye);
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], blue, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, Color.White, true);
            return false;
        }
    }
}
