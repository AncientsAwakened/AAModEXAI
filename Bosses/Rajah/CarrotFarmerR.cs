using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah
{
    public class CarrotFarmerR : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carrot Farmer");
        }

        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 156;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ownerHitCheck = true;
            projectile.ignoreWater = true;
            projectile.hostile = true;
            projectile.friendly = false;
            aiType = ProjectileID.Bullet;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            projHitbox.Width += 16;
            projHitbox.Height += 16;

            return projHitbox.Intersects(targetHitbox);
        }

        public Rajah rajah = null;

        public override void AI()
        {
            if (rajah == null)
            {
                NPC npcBody = Main.npc[(int)projectile.ai[0]];
                if (npcBody.type == mod.NPCType("Rajah"))
                {
                    rajah = (Rajah)npcBody.modNPC;
                }
                else if (npcBody.type == mod.NPCType("SupremeRajah"))
                {
                    rajah = (SupremeRajah)npcBody.modNPC;
                }
            }
            if (rajah == null)
                return;

            if (!rajah.npc.active || rajah.npc.life <= 0 || rajah.npc.ai[3] != 4)
            {
                projectile.Kill();
            }

            if (rajah.npc.direction > 0)
            {
                projectile.rotation += 0.35f;
                projectile.spriteDirection = 1;
            }
            else
            {
                projectile.rotation -= 0.35f;
                projectile.spriteDirection = -1;
            }

            projectile.position.X = rajah.WeaponPos.X - 95;
            projectile.position.Y = rajah.WeaponPos.Y - 93;

            projectile.ai[1]++;
            if (projectile.ai[1] >= 16)
            {
                for (int u = 0; u < 10; u++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<CarrotDust>(), Main.rand.Next((int)-5f, (int)5f), Main.rand.Next((int)-5f, (int)5f), 0);
                    Main.dust[dust].noGravity = true;
                }
                float spread = 12f * 0.0174f;
                double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 30f;
                double offsetAngle;
                int i;
                if (projectile.owner == Main.myPlayer)
                {
                    for (i = 0; i < 30; i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                        int carrotType = rajah.isSupreme ? mod.ProjectileType("CarrotEXR") : mod.ProjectileType("CarrotHostile");
                        if (Main.rand.Next(rajah.isSupreme ? 10 : 15) == 0)
                        {
                            int ProjID = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 6f), (float)(Math.Cos(offsetAngle) * 6f), carrotType, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                            Main.projectile[ProjID].Center = projectile.Center;
                        }
                        if (Main.rand.Next(rajah.isSupreme ? 10 : 15) == 0)
                        {
                            int ProjID = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 6f), (float)(-Math.Cos(offsetAngle) * 6f), carrotType, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                            Main.projectile[ProjID].Center = projectile.Center;
                        }
                    }
                }
                projectile.ai[1] = -0;
            }
        }
    }
}