
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis
{
    public class Scepter : ModProjectile
	{
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = -1;
            projectile.timeLeft = 3600;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.melee = true;
        }

        public int master = -1;

		public override void AI()
		{
            if (master >= 0 && (Main.npc[master] == null || !Main.npc[master].active || Main.npc[master].type != mod.NPCType("Anubis"))) master = -1;
            if (master == -1)
            {
                master = BaseAI.GetNPC(projectile.Center, mod.NPCType("Anubis"), -1, null);
                if (master == -1) master = -2;
            }
            if (master == -1) { return; }
			if (master < 0 || !Main.npc[master].active || Main.npc[master].life <= 0) { projectile.Kill(); return; }

            if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 200, default, 0.5f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }

            BaseAI.AIBoomerang(projectile, ref projectile.ai, Main.npc[master].position, Main.npc[master].width, Main.npc[master].height, true, 40, 45, 10f, 1f, true);

            ReflectProjectiles(projectile.Hitbox);
        }

        public void ReflectProjectiles(Rectangle myRect)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].hostile)
                {
                    Rectangle hitbox = Main.projectile[i].Hitbox;
                    if (myRect.Intersects(hitbox))
                    {
                        Main.PlaySound(SoundID.NPCHit4, Main.projectile[i].position);
                        for (int j = 0; j < 3; j++)
                        {
                            int num = Dust.NewDust(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, DustID.Gold, 0f, 0f, 0, default, 1f);
                            Main.dust[num].velocity *= 0.3f;
                        }
                        Main.projectile[i].hostile = true;
                        Main.projectile[i].friendly = false;
                        Vector2 vector = Main.player[Main.projectile[i].owner].Center - Main.projectile[i].Center;
                        vector.Normalize();
                        vector *= Main.projectile[i].oldVelocity.Length();
                        Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        reflectvelocity.Normalize();
                        reflectvelocity *= vector.Length();
                        reflectvelocity += vector * 20f;
                        reflectvelocity.Normalize();
                        reflectvelocity *= vector.Length();
                        reflectvelocity *= 25f;
                        Main.projectile[i].penetrate = 1;

                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                    }
                }
            }
        }
    }
}