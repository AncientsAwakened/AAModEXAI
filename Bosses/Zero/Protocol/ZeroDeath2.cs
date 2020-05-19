using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class ZeroDeath2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero");
            Main.projFrames[projectile.type] = 30;
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
            projectile.timeLeft = 1000;
        }
        public override void AI()
        {
            AAWorld.downedZero = true;
            if (++projectile.frameCounter >= 10)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 30)
                {
                    projectile.Kill();
                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y += 0.00f;
            if (projectile.timeLeft == 913)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroDeath4"), Color.Red.R, Color.Red.G, Color.Red.B);
            }
        }
    }
}