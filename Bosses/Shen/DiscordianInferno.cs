
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Shen
{
    public class DiscordianInferno : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Inferno");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
			projectile.extraUpdates = 1;
        }


        public override void AI()
        {
            int dustType = projectile.ai[0] == 1 ? ModLoader.GetMod("AAMod").DustType("AkumaADust") : ModLoader.GetMod("AAMod").DustType("YamataADust");
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
                Main.PlaySound(SoundID.DD2_BetsyFireballShot, projectile.Center);
            }
            if (InternalAI[0] >= 2f)
            {
                projectile.alpha -= 30;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
			if(Main.rand.Next(3) == 0)
			{
				for(int m = 0; m < 3; m++)
				{
					int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 1.6f);
					Main.dust[dustID].velocity = -projectile.velocity * 0.5f;
					Main.dust[dustID].noLight = false;
					Main.dust[dustID].noGravity = true;
				}
				int dustID2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.Purple, 2f);
				Main.dust[dustID2].velocity = -projectile.velocity * 0.5f;
				Main.dust[dustID2].noLight = false;
				Main.dust[dustID2].noGravity = true;
			}
        }

        public override void Kill(int timeLeft)
        {
            int dustType = projectile.ai[0] == 1 ? ModLoader.GetMod("AAMod").DustType("AkumaADust") : ModLoader.GetMod("AAMod").DustType("YamataADust");
            int pieCut = 20;
			for(int m = 0; m < pieCut; m++)
			{
				int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 1.6f);
				Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
				Main.dust[dustID].noLight = false;
				Main.dust[dustID].noGravity = true;
			}
			for(int m = 0; m < pieCut; m++)
			{
				int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 2f);
				Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
				Main.dust[dustID].noLight = false;
				Main.dust[dustID].noGravity = true;
			}
			for(int m = 0; m < 15; m++)
			{
				int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, Color.White, 1.2f);
				Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(8f + Main.rand.Next(6), 0f), MathHelper.Lerp((float)Main.rand.NextDouble(), 0f, 6.28f));
				Main.dust[dustID].noLight = false;
				Main.dust[dustID].noGravity = true;
			}
            Main.PlaySound(SoundID.Item62, (int)projectile.position.X, (int)projectile.position.Y);
        }


        public override Color? GetAlpha(Color lightColor)
        {
            Color color = projectile.ai[0] == 1 ? AAColor.AkumaA : AAColor.YamataA ;
            return new Color(color.R, color.G, color.B, 200);
        }

        public float[] InternalAI = new float[1];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(InternalAI[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                InternalAI[0] = reader.ReadFloat();
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(projectile.ai[0] == 1 ? ModLoader.GetMod("AAMod").BuffType("DragonFire") : ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 200);
        }
    }
}