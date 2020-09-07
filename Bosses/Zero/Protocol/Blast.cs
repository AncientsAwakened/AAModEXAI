using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class Blast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4; 
        }
        public override void SetDefaults()
        {
            projectile.damage = 1;
            projectile.width = 38;
            projectile.height = 38;
            projectile.hostile = true;
            projectile.aiStyle = -1;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return AAColor.Oblivion;
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.ai[1]++ == 120 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                switch (projectile.ai[0])
                {
                    case 0f:
                        Projectile.NewProjectile(projectile.Center, new Vector2(10, 0), mod.ProjectileType("EchoRay"), 70, 3f, Main.myPlayer, 0, projectile.whoAmI);
                        break;
                    case 1f:
                        Projectile.NewProjectile(projectile.Center, new Vector2(-10, 0), mod.ProjectileType("EchoRay"), 70, 3f, Main.myPlayer, 0, projectile.whoAmI);
                        break;
                    case 2f:
                        Projectile.NewProjectile(projectile.Center, new Vector2(0, 10), mod.ProjectileType("EchoRay"), 70, 3f, Main.myPlayer, 0, projectile.whoAmI);
                        break;
                    default:
                        Projectile.NewProjectile(projectile.Center, new Vector2(0, -10), mod.ProjectileType("EchoRay"), 70, 3f, Main.myPlayer, 0, projectile.whoAmI);
                        break;
                }
            }
            if (projectile.ai[1] > 240)
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Vector2 position = projectile.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 226, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num86].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (Main.rand.Next(7) == 0)
			{
				target.AddBuff(mod.BuffType("Unstable"), 180, true);
			}
		}
    }
}
