using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RoyalRabbitLancerSpear : ModProjectile
    {

        public short customGlowMask = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Rabbit Partisan");
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.scale = 1.1f;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
        }

        

        public float MovementFactor // Change this value to alter how fast the spear moves
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("InfinityOverload"), 600);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, new Rectangle(0, 0, Tex.Width, Tex.Height), dColor, true);
            return false;
        }

        public override void AI()
        {
            //dust!
            int dustId = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width / 2, projectile.height + 5, ModContent.DustType<CarrotDust>(), projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width / 2, projectile.height + 5, ModContent.DustType<CarrotDust>(), projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[dustId3].noGravity = true;

            NPC projOwner = Main.npc[(int)projectile.ai[1]];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile directio and position based on the player
            Vector2 ownerMountedCenter = projOwner.Center;
            projectile.direction = projOwner.direction;
            projectile.position.X = ownerMountedCenter.X - projectile.width / 2;
            projectile.position.Y = ownerMountedCenter.Y - projectile.height / 2;

            if (MovementFactor == 0f) // When intially thrown out, the ai0 will be 0f
            {
                MovementFactor = 3f; // Make sure the spear moves forward when initially thrown out
                projectile.netUpdate = true; // Make sure to netUpdate this spear
            }

            if (projOwner.ai[0] > 23 * .66f) // Somewhere along the item animation, make sure the spear moves back
                MovementFactor -= .8f;
            else // Otherwise, increase the movement factor
                MovementFactor += .6f;

            // Change the spear position based off of the velocity and the movementFactor
            projectile.position += projectile.velocity * MovementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.ai[0] == 22) projectile.Kill();
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (projectile.spriteDirection == -1) projectile.rotation -= MathHelper.ToRadians(90f);
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust1;
                Dust dust2;
                Vector2 position = projectile.position;
                dust1 = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<Dusts.CarrotDust>(), 4.736842f, 0f, 46, default, 1f)];
                dust2 = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<Dusts.CarrotDust>(), 4.736842f, 0f, 46, default, 1f)];
                dust1.noGravity = true;
                dust2.noGravity = true;
            }
			if (projectile.timeLeft == 80)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, ModContent.ProjectileType<BlazingTerra>(), projectile.damage, projectile.knockBack, projectile.owner);
			}
        }

    }
}
