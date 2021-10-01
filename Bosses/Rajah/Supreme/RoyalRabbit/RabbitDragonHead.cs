using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class RabbitDragonHead : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;

            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.netImportant = true;
            projectile.tileCollide = false;

            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.timeLeft *= 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rabbit Lung");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num214 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            if(flaming) projectile.frame = 1;
            else projectile.frame = 0;
            int y6 = num214 * projectile.frame;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle(0, y6, texture2D13.Width, num214),
                projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), projectile.scale,
                projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }

        public bool flaming = false;
        public int master = -1;

        public override void AI()
        {
            if ((int)projectile.ai[0] >= 200f || (int)projectile.ai[0] < 0f || Main.npc[(int)projectile.ai[0]] == null || !Main.npc[(int)projectile.ai[0]].active || Main.npc[(int)projectile.ai[0]].type != ModContent.NPCType<RoyalRabbitSummoner>()) master = -1;
            if (!NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitSummoner>()))
			{
                projectile.active = false;
                return;
            }
            
            NPC summonernpc = Main.npc[(int)projectile.ai[0]];

            if ((int)Main.time % 120 == 0) projectile.netUpdate = true;
            if (!summonernpc.active)
            {
                projectile.active = false;
                return;
            }

            int num1038 = 10;
            num1038 = 30;

            Vector2 center = summonernpc.Center;
            float num1040 = 700f;
            float num1041 = 1000f;
            int num1042 = -1;
            if (projectile.Distance(center) > 2000f)
            {
                projectile.Center = center;
                projectile.netUpdate = true;
            }

            Player ownerMinionAttackTargetPlayer = Main.player[summonernpc.target];
            if (ownerMinionAttackTargetPlayer != null && ownerMinionAttackTargetPlayer.active && ownerMinionAttackTargetPlayer.statLife > 0)
            {
                float num1043 = projectile.Distance(ownerMinionAttackTargetPlayer.Center);
                if (num1043 < num1040 * 2f)
                {
                    num1042 = ownerMinionAttackTargetPlayer.whoAmI;
                }
            }

            if (num1042 < 0 || !ownerMinionAttackTargetPlayer.active || ownerMinionAttackTargetPlayer.statLife <= 0)
            {
                for (int num1044 = 0; num1044 < 200; num1044++)
                {
                    Player player13 = Main.player[num1044];
                    if (ownerMinionAttackTargetPlayer.active && ownerMinionAttackTargetPlayer.statLife > 0 && summonernpc.Distance(player13.Center) < num1041)
                    {
                        float num1045 = projectile.Distance(player13.Center);
                        if (num1045 < num1040)
                        {
                            num1042 = num1044;
                        }
                    }
                }
            }

            if (num1042 != -1)
            {
                ownerMinionAttackTargetPlayer = Main.player[num1042];
                Vector2 vector132 = ownerMinionAttackTargetPlayer.Center - projectile.Center;
                (vector132.X > 0f).ToDirectionInt();
                (vector132.Y > 0f).ToDirectionInt();
                float scaleFactor15 = 0.4f;
                if (vector132.Length() < 300f) scaleFactor15 = 0.8f;
                if (vector132.Length() > (ownerMinionAttackTargetPlayer.Size.Length() * 0.75f + 100f))
                {
                    projectile.velocity += Vector2.Normalize(vector132) * scaleFactor15 * 1.5f;
                    if (Vector2.Dot(projectile.velocity, vector132) < 0.25f) projectile.velocity *= 0.8f;
                }

                float num1046 = 30f;
                if (projectile.velocity.Length() > num1046) projectile.velocity = Vector2.Normalize(projectile.velocity) * num1046;
            }
            else
            {
                float num1047 = 0.2f;
                Vector2 vector133 = center - projectile.Center;
                if (vector133.Length() < 200f) num1047 = 0.12f;
                if (vector133.Length() < 140f) num1047 = 0.06f;
                if (vector133.Length() > 100f)
                {
                    if (Math.Abs(center.X - projectile.Center.X) > 20f) projectile.velocity.X = projectile.velocity.X + num1047 * Math.Sign(center.X - projectile.Center.X);
                    if (Math.Abs(center.Y - projectile.Center.Y) > 10f) projectile.velocity.Y = projectile.velocity.Y + num1047 * Math.Sign(center.Y - projectile.Center.Y);
                }
                else if (projectile.velocity.Length() > 2f)
                {
                    projectile.velocity *= 0.96f;
                }

                if (Math.Abs(projectile.velocity.Y) < 1f) projectile.velocity.Y = projectile.velocity.Y - 0.1f;
                float num1048 = 15f;
                if (projectile.velocity.Length() > num1048) projectile.velocity = Vector2.Normalize(projectile.velocity) * num1048;
            }

            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            int direction = projectile.direction;
            projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
            if (direction != projectile.direction) projectile.netUpdate = true;
            float num1049 = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
            projectile.position = projectile.Center;
            projectile.scale = 1f + num1049 * 0.01f;
            projectile.width = projectile.height = (int)(num1038 * projectile.scale);
            projectile.Center = projectile.position;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 42;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
        }
    }

    public class RabbitDragonBody : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;

            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.netImportant = true;
            projectile.tileCollide = false;

            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            projectile.timeLeft *= 5;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rabbit Lung");
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles,
            List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num214 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int y6 = num214 * projectile.frame;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle(0, y6, texture2D13.Width, num214),
                projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), projectile.scale,
                projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }

        public override void AI()
        {

            if ((int)Main.time % 120 == 0) projectile.netUpdate = true;

            if (!NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitSummoner>()))
			{
                projectile.active = false;
                return;
            }

            int num1038 = 10;
            num1038 = 30;

            //D U S T
            /*if (Main.rand.Next(30) == 0)
            {
                int num1039 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 0, default, 2f);
                Main.dust[num1039].noGravity = true;
                Main.dust[num1039].fadeIn = 2f;
                Point point4 = Main.dust[num1039].position.ToTileCoordinates();
                if (WorldGen.InWorld(point4.X, point4.Y, 5) && WorldGen.SolidTile(point4.X, point4.Y))
                {
                    Main.dust[num1039].noLight = true;
                }
            }*/

            bool flag67 = false;
            Vector2 value67 = Vector2.Zero;
            Vector2 arg_2D865_0 = Vector2.Zero;
            float num1052 = 0f;
            float scaleFactor16 = 0f;
            float scaleFactor17 = 1f;
            if (projectile.ai[1] == 1f)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }

            int byUUID = Projectile.GetByUUID(projectile.owner, (int)projectile.ai[0]);
            if (byUUID >= 0 && Main.projectile[byUUID].active)
            {
                flag67 = true;
                value67 = Main.projectile[byUUID].Center;
                Vector2 arg_2D957_0 = Main.projectile[byUUID].velocity;
                num1052 = Main.projectile[byUUID].rotation;
                float num1053 = MathHelper.Clamp(Main.projectile[byUUID].scale, 0f, 50f);
                scaleFactor17 = num1053;
                scaleFactor16 = 16f;
                int arg_2D9AD_0 = Main.projectile[byUUID].alpha;
                Main.projectile[byUUID].localAI[0] = projectile.localAI[0] + 1f;
                if (Main.projectile[byUUID].type != ModContent.ProjectileType<RabbitDragonHead>()) Main.projectile[byUUID].localAI[1] = projectile.whoAmI;
                if (Main.projectile[byUUID].type != ModContent.ProjectileType<RabbitDragonHead>() && Main.projectile[byUUID].type != ModContent.ProjectileType<RabbitDragonBody>())
                {
                    projectile.active = false;
                    return;
                }
            }

            if (!flag67) return;
            if (projectile.alpha > 0)
                for (int num1054 = 0; num1054 < 2; num1054++)
                {
                    int num1055 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 100, default, 2f);
                    Main.dust[num1055].noGravity = true;
                    Main.dust[num1055].noLight = true;
                }

            projectile.alpha -= 42;
            if (projectile.alpha < 0) projectile.alpha = 0;
            projectile.velocity = Vector2.Zero;
            Vector2 vector134 = value67 - projectile.Center;
            if (num1052 != projectile.rotation)
            {
                float num1056 = MathHelper.WrapAngle(num1052 - projectile.rotation);
                vector134 = vector134.RotatedBy(num1056 * 0.1f, default);
            }

            projectile.rotation = vector134.ToRotation() + 1.57079637f;
            projectile.position = projectile.Center;
            projectile.scale = scaleFactor17;
            projectile.width = projectile.height = (int)(num1038 * projectile.scale);
            projectile.Center = projectile.position;
            if (vector134 != Vector2.Zero) projectile.Center = value67 - Vector2.Normalize(vector134) * scaleFactor16 * scaleFactor17;
            projectile.spriteDirection = vector134.X > 0f ? 1 : -1;

            projectile.damage = Main.projectile[byUUID].damage;
        }
    }

    public class RabbitDragonTail : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;

            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.netImportant = true;
            projectile.tileCollide = false;

            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            projectile.timeLeft *= 5;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rabbit Lung");
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles,
           List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num214 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int y6 = num214 * projectile.frame;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle(0, y6, texture2D13.Width, num214),
                projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), projectile.scale,
                projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }

        public override void AI()
        {

            if ((int)Main.time % 120 == 0) projectile.netUpdate = true;

            if (!NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitSummoner>()))
			{
                projectile.active = false;
                return;
            }


            int num1038 = 10;
            num1038 = 30;

            //D U S T
            /*if (Main.rand.Next(30) == 0)
            {
                int num1039 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 0, default, 2f);
                Main.dust[num1039].noGravity = true;
                Main.dust[num1039].fadeIn = 2f;
                Point point4 = Main.dust[num1039].position.ToTileCoordinates();
                if (WorldGen.InWorld(point4.X, point4.Y, 5) && WorldGen.SolidTile(point4.X, point4.Y))
                {
                    Main.dust[num1039].noLight = true;
                }
            }*/

            bool flag67 = false;
            Vector2 value67 = Vector2.Zero;
            Vector2 arg_2D865_0 = Vector2.Zero;
            float num1052 = 0f;
            float scaleFactor16 = 0f;
            float scaleFactor17 = 1f;
            if (projectile.ai[1] == 1f)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }

            int byUUID = Projectile.GetByUUID(projectile.owner, (int)projectile.ai[0]);
            if (byUUID >= 0 && Main.projectile[byUUID].active)
            {
                flag67 = true;
                value67 = Main.projectile[byUUID].Center;
                Vector2 arg_2D957_0 = Main.projectile[byUUID].velocity;
                num1052 = Main.projectile[byUUID].rotation;
                float num1053 = MathHelper.Clamp(Main.projectile[byUUID].scale, 0f, 50f);
                scaleFactor17 = num1053;
                scaleFactor16 = 16f;
                int arg_2D9AD_0 = Main.projectile[byUUID].alpha;
                Main.projectile[byUUID].localAI[0] = projectile.localAI[0] + 1f;
                if (Main.projectile[byUUID].type != ModContent.ProjectileType<RabbitDragonHead>()) Main.projectile[byUUID].localAI[1] = projectile.whoAmI;
                if (Main.projectile[byUUID].type == ModContent.ProjectileType<RabbitDragonHead>())
                {
                    Main.projectile[byUUID].Kill();
                    projectile.Kill();
                    return;
                }
                if (Main.projectile[byUUID].type != ModContent.ProjectileType<RabbitDragonHead>() && Main.projectile[byUUID].type != ModContent.ProjectileType<RabbitDragonBody>())
                {
                    projectile.active = false;
                    return;
                }
            }

            if (!flag67) return;
            if (projectile.alpha > 0)
                for (int num1054 = 0; num1054 < 2; num1054++)
                {
                    int num1055 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 100, default, 2f);
                    Main.dust[num1055].noGravity = true;
                    Main.dust[num1055].noLight = true;
                }

            projectile.alpha -= 42;
            if (projectile.alpha < 0) projectile.alpha = 0;
            projectile.velocity = Vector2.Zero;
            Vector2 vector134 = value67 - projectile.Center;
            if (num1052 != projectile.rotation)
            {
                float num1056 = MathHelper.WrapAngle(num1052 - projectile.rotation);
                vector134 = vector134.RotatedBy(num1056 * 0.1f, default);
            }

            projectile.rotation = vector134.ToRotation() + 1.57079637f;
            projectile.position = projectile.Center;
            projectile.scale = scaleFactor17;
            projectile.width = projectile.height = (int)(num1038 * projectile.scale);
            projectile.Center = projectile.position;
            if (vector134 != Vector2.Zero) projectile.Center = value67 - Vector2.Normalize(vector134) * scaleFactor16 * scaleFactor17;
            projectile.spriteDirection = vector134.X > 0f ? 1 : -1;

            projectile.damage = Main.projectile[byUUID].damage;
        }
    }
}