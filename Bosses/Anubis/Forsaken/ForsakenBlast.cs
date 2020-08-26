
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class ForsakenBlast : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.extraUpdates = 1;
        }


        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            int dustType = ModLoader.GetMod("AAMod").DustType("ForsakenDust");
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
                Main.PlaySound(SoundID.DD2_BetsyFireballShot, projectile.Center);
            }
            if (Main.rand.Next(3) == 0)
            {
                int dustID2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID2].velocity = -projectile.velocity * 0.5f;
                Main.dust[dustID2].noLight = false;
                Main.dust[dustID2].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            int dustType = ModLoader.GetMod("AAMod").DustType("ForsakenDust");
            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 1.6f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            Main.PlaySound(SoundID.Item62, (int)projectile.position.X, (int)projectile.position.Y);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}