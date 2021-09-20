
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class ForsakenStaff : ModProjectile
	{
        public override void SetDefaults()
        {
            projectile.width = 144;
            projectile.height = 144;
            projectile.aiStyle = -1;
            projectile.timeLeft = 3600;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public int master = -1;

		public override void AI()
		{
            if (master >= 0 && (Main.npc[master] == null || !Main.npc[master].active || Main.npc[master].type != ModContent.NPCType<ForsakenAnubis>())) master = -1;
            if (master == -1)
            {
                master = BaseAI.GetNPC(projectile.Center, ModContent.NPCType<ForsakenAnubis>(), -1, null);
                if (master == -1) master = -2;
            }
            if (master == -1) { return; }
			if (master < 0 || !Main.npc[master].active || Main.npc[master].life <= 0) { projectile.Kill(); return; }

            if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<ForsakenDust>(), 0f, 0f, 200, default, 0.5f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;

            //AIBoomerang(Projectile p, ref float[] ai, Vector2 position = default, int width = -1, int height = -1, bool playSound = true, float maxDistance = 9f, int returnDelay = 35, float speedInterval = 0.4f, float rotationInterval = 0.4f, bool direct = false)
            if(Main.npc[master].ai[0] == 1)
            {
                BaseAI.AIBoomerang(projectile, ref projectile.ai, Main.npc[master].position, Main.npc[master].width, Main.npc[master].height, true, 40, 45, 10f, 1f, true);
            }
            else
            {
                BaseAI.AIBoomerang(projectile, ref projectile.ai, Main.npc[master].position, Main.npc[master].width, Main.npc[master].height, true, 120, 60, 10f, 1f, true);
            }
            

            ReflectProjectiles(projectile.Hitbox);
        }

        public void ReflectProjectiles(Rectangle myRect)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].hostile)
                {
                    Rectangle hitbox = Main.projectile[i].Hitbox;
                    if (Main.projectile[i].Colliding(hitbox, myRect))
                    {
                        Main.PlaySound(SoundID.NPCHit4, Main.projectile[i].position);
                        for (int j = 0; j < 3; j++)
                        {
                            int num = Dust.NewDust(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, ModContent.DustType<ForsakenDust>(), 0f, 0f, 0, default, 1f);
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
                        reflectvelocity *= 25f;
                        Main.projectile[i].penetrate = 1;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, 0, 0);

            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile, 2f, 1f, 5, true, 0f, 0f, dColor);

            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, dColor, true);
            return false;
        }
    }
}