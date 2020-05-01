using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class ZeroDeath1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero");
            Main.projFrames[projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }
        public bool linesaid = false;
        public override void AI()
        {
            if (Main.expertMode && !AAWorld.downedZero && !linesaid)
            {
                if (Main.netMode != 1)
                {
                    AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroDeath1"), Color.Red.R, Color.Red.G, Color.Red.B);
                    linesaid = true;
                }
            }
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 7)
                {
                    projectile.Kill();
                   
                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y += 0.00f;
           
        }
        public override void Kill(int timeLeft)
        {
            if (!AAWorld.downedZero && Main.expertMode)
            {
                if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroDeath3"), Color.Red.R, Color.Red.G, Color.Red.B);
            }
            int p = Projectile.NewProjectile(projectile.Center, new Vector2(0f, 0f), mod.ProjectileType("ZeroDeath2"), 0, 0);
            Main.projectile[p].Center = projectile.Center;
        }
    }
}