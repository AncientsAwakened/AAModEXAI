using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using AAMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using AAMod;

namespace AAModEXAI.Bosses.Greed.OreBomb
{
    public class OreChunkM : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
			projectile.aiStyle = -1;
            projectile.friendly = false;
			projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.aiStyle = 14;
            projectile.timeLeft = 1800;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ore");
            Main.projFrames[projectile.type] = 28;
        }

        public float rotationspeed = 0.2f;

        public override void AI()
        {
            OreEffect();
            if (projectile.velocity.X > 0)
            {
                projectile.direction = 1;
            }
            else
            {
                projectile.direction = -1;
            }

            bool flag = false;
            Vector2 velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, true, true, 1);;
            if (velocity != projectile.velocity)
            {
                flag = true;
            }
            if (flag && ProjectileLoader.OnTileCollide(projectile, projectile.velocity))
            {
                rotationspeed -= .021f;
            }

            if (rotationspeed <= 0 || (projectile.velocity.X == 0))
            {
                projectile.Kill();
                rotationspeed = 0f;
            }

            projectile.rotation += rotationspeed * projectile.direction;

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }

        public override void PostAI()
        {
            projectile.frame = (int)projectile.ai[1];
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 28, 0, 0);
            if (projectile.ai[1] == 9 || projectile.ai[1] == 11 || projectile.ai[1] == 22 || projectile.ai[1] == 26)
            {
                BaseDrawing.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.oldPos, 1, projectile.rotation, projectile.direction, 28, frame, .8f, 1, 4, true, 0, 0, lightColor);
            }
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 28, frame, lightColor, true);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            int DustType = DType();
            if (projectile.ai[1] == 8)
            {
                for (int num291 = 0; num291 < 5; num291++)
                {
                    int num292 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default, 2.1f);
                    Main.dust[num292].velocity *= 2f;
                    Main.dust[num292].noGravity = true;
                };
            }
            if (projectile.ai[1] == 21)
            {
                for (int s = 0; s < 3; s++)
                {
                    int a = Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<OreSpores>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0, s);
                }
            }
            if (projectile.ai[1] == 22)
            {
                int a = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<LuminiteBlast>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0, 0);
            }
            if (projectile.ai[1] == 25)
            {
                int a = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<DaybreakBlast>(), projectile.damage, projectile.knockBack * 3, Main.myPlayer, 0, 0);
            }
            if (projectile.ai[1] == 27)
            {
                for (int v = 0; v < 4; v++)
                {
                    int x = Main.rand.Next(-6, 6);
                    int y = -Main.rand.Next(3, 5);
                    int p = Projectile.NewProjectile(projectile.position, new Vector2(x, y), ModContent.ProjectileType<AFrag>(), projectile.damage, 0, Main.myPlayer, 0, Main.rand.Next(23));
                    Main.projectile[p].Center = projectile.Center;
                }
            }
            for (int num468 = 0; num468 < 5; num468++)
            {
                float VelX = -projectile.velocity.X * 0.2f;
                float VelY = -projectile.velocity.Y * 0.2f;
                num468 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustType, VelX, VelY);
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            switch ((int)projectile.ai[1])
            {
                case 6:
                case 7: target.AddBuff(BuffID.Midas, 180); break;

                case 12:
                case 13: target.AddBuff(BuffID.OnFire, 180); break;

                case 23: target.AddBuff(ModContent.BuffType<AAMod.Buffs.Electrified>(), 180); break;
                case 25: target.AddBuff(BuffID.Daybreak, 180); break;
                case 26: target.AddBuff(ModContent.BuffType<AAMod.Buffs.Moonraze>(), 180); break;
            }
        }

        public void OreEffect()
        {
            switch ((int)projectile.ai[1])
            {
                case 9:
                case 11:
                case 24:
                    projectile.extraUpdates = 1;
                    break;
                case 13:
                case 14:
                    for (int num291 = 0; num291 < 5; num291++)
                    {
                        int num292 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    };
                    break;
                case 22:
                case 26:
                    projectile.extraUpdates = 2;
                    break;
            }
        }

        public int Damage()
        {
            switch ((int)projectile.ai[1])
            {
                case 0:
                    return 8;
                case 1:
                    return 9;
                case 2:
                    return 10;
                case 3:
                case 4:
                    return 11;
                case 5:
                    return 12;
                case 6:
                    return 13;
                case 7:
                    return 15;
                case 8:
                    return 21;
                case 9:
                    return 19;
                case 10:
                    return 22;
                case 11:
                    return 14;
                case 12:
                    return 26;
                case 13:
                    return 36;
                case 14:
                    return 39;
                case 15:
                    return 41;
                case 16:
                    return 44;
                case 17:
                    return 47;
                case 18:
                    return 50;
                case 19:
                    return 52;
                case 20:
                    return 57;
                case 21:
                    return 75;
                case 22:
                    return 110;
                case 23:
                    return 130;
                case 24:
                    return 170;
                case 25:
                    return 160;
                case 26:
                    return 130;
                case 27:
                    return 150;
                default:
                    goto case 0;
            }
        }

        public int DType()
        {
            switch ((int)projectile.ai[1])
            {
                case 0:
                    return DustID.Copper;
                case 1:
                    return DustID.Tin;
                case 2:
                    return DustID.Iron;
                case 3:
                    return DustID.Lead;
                case 4:
                    return DustID.Silver;
                case 5:
                    return DustID.Tungsten;
                case 6:
                    return DustID.Gold;
                case 7:
                    return DustID.Platinum;
                case 8:
                    return DustID.t_Meteor;
                case 9:
                    return 14;
                case 10:
                    return 117;
                case 11:
                    return ModLoader.GetMod("AAMod").DustType("IncineriteDust");
                case 12:
                    return ModLoader.GetMod("AAMod").DustType("AbyssiumDust");
                case 13:
                    return DustID.Fire;
                case 14:
                    return 48;
                case 15:
                    return 144;
                case 16:
                    return 49;
                case 17:
                    return 145;
                case 18:
                    return 50;
                case 19:
                    return 146;
                case 20:
                    return DustID.Gold;
                case 21:
                    return 128;
                case 22:
                    return ModLoader.GetMod("AAMod").DustType("LuminiteDust");
                case 23:
                    return ModLoader.GetMod("AAMod").DustType("DarkmatterDust");
                case 24:
                    return ModLoader.GetMod("AAMod").DustType("RadiumDust");
                case 25:
                    return ModLoader.GetMod("AAMod").DustType("DaybreakIncineriteDust");
                case 26:
                    return ModLoader.GetMod("AAMod").DustType("YamataDust");
                case 27:
                    return ModLoader.GetMod("AAMod").DustType("VoidDust");
                default:
                    goto case 0;
            }

        }
    }
}
