using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class Static : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Static Shock");
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.penetrate = -1;               
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = true;       
            projectile.ignoreWater = true;
            projectile.timeLeft = 900;
        }

        int a = 0;

        public override void PostAI()
        {
            if (Main.netMode != 1) a++;
            if (a == 40)
            {
                projectile.tileCollide = true;
                projectile.netUpdate = true;
            }
            if (a < 40)
            {
                projectile.tileCollide = false;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.Oblivion;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 130f) //projectile time left before disappears
            {
                projectile.Kill();
            }
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position, oldVelocity, projectile.width, projectile.height);
            Main.PlaySound(ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Glitch"), (int)projectile.Center.X, (int)projectile.Center.Y);
            return true;
        }
    }
}