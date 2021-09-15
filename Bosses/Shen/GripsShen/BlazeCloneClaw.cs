using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

using Terraria.Graphics.Shaders;using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Shen.GripsShen
{
    public class BlazeCloneClaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shen Blaze Claw");
        }

        public override void SetDefaults()
        {
            projectile.width = 66;
            projectile.height = 60;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
            projectile.scale = 1.5f;
            projectile.alpha = 255;
        }

        public int timecount = 0;

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.Glow;
        }

        public override void AI()
        {
            Player targetPlayer = Main.player[Main.npc[(int)projectile.ai[0]].target];

            timecount++;

            if(timecount < 100)
            {
                projectile.position = Main.npc[(int)projectile.ai[0]].Center + 100f * Vector2.Normalize(Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center)) + 200f * projectile.ai[1] * Vector2.Normalize(Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center).RotatedBy(3.1415926f / 2));
            }
            else if(timecount == 100)
            {
                projectile.position = Main.npc[(int)projectile.ai[0]].Center + 100f * Vector2.Normalize(Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center)) + 200f * projectile.ai[1] * Vector2.Normalize(Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center).RotatedBy(3.1415926f / 2));
                projectile.velocity = 24f * Vector2.Normalize(Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center));
            }
            else
            {
                projectile.velocity = projectile.oldVelocity;
            }

            for(int i = 0; i<1000; i++)
            {
                if(Main.projectile[i].friendly && !Main.projectile[i].minion && Main.projectile[i].Hitbox.Intersects(projectile.Hitbox))
                {
                    Main.projectile[i].Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<DeBuffs.DragonFire>(), 200);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color dColor)
        {
            Player targetPlayer = Main.player[Main.npc[(int)projectile.ai[0]].target];
            Color Alpha = dColor;
            if(timecount < 10)
            {
                Alpha.R = (byte)(0f);
                Alpha.G = (byte)(0f);
                Alpha.B = (byte)(0f);
                Alpha.A = (byte)(0f);
                projectile.rotation = Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center).ToRotation() + (Main.npc[(int)projectile.ai[0]].position.X < targetPlayer.position.X ? 0 : (float)Math.PI);
                projectile.direction = projectile.spriteDirection = Main.npc[(int)projectile.ai[0]].position.X < targetPlayer.position.X ? -1 : 1;
            }
            else if(timecount < 100)
            {
                Alpha.R = (byte)((float)(timecount * 2));
                Alpha.G = (byte)((float)(timecount * 2));
                Alpha.B = (byte)((float)(timecount * 2));
                Alpha.A = (byte)((float)(timecount * 2));
                projectile.rotation = Main.npc[(int)projectile.ai[0]].DirectionTo(targetPlayer.Center).ToRotation() + (Main.npc[(int)projectile.ai[0]].position.X < targetPlayer.position.X ? 0 : (float)Math.PI);
                projectile.direction = projectile.spriteDirection = Main.npc[(int)projectile.ai[0]].position.X < targetPlayer.position.X ? -1 : 1;
            }
            else
            {
                Alpha.R = (byte)200f;
                Alpha.G = (byte)200f;
                Alpha.B = (byte)200f;
                Alpha.A = (byte)200f;
                projectile.rotation = projectile.velocity.ToRotation() + (projectile.velocity.X > 0 ? 0 : (float)Math.PI);
                projectile.direction = projectile.velocity.X > 0 ? -1 : 1;
            }
            int red = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, 0, 2);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], red, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 1, frame, Alpha, true);
            return false;
        }

    }
}
