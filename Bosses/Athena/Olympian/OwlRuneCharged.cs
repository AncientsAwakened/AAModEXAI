using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Athena.Olympian
{
	public class OwlRuneCharged : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charged Owl Rune");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.aiStyle = 0;
            npc.damage = Main.expertMode ? 50 : 84;
            npc.defense = Main.expertMode ? 1 : 1;
            npc.knockBackResist = 0.2f;
            npc.width = 82;
            npc.height = 82;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.damage = 90;
        }

        public override void AI()
        {
            if (npc.localAI[1] == 0f)
            {
                Main.PlaySound(SoundID.Item121, npc.position);
                npc.localAI[1] = 1f;
            }
            if (npc.ai[0] < 180f)
            {
                npc.alpha -= 5;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
            else
            {
                npc.alpha += 5;
                if (npc.alpha > 255)
                {
                    npc.alpha = 255;
                    npc.active = false;
                    return;
                }
            }
            npc.ai[0] += 1f;
            if (npc.ai[0] % 30f == 0f && npc.ai[0] < 180f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int[] array4 = new int[5];
                Vector2[] array5 = new Vector2[5];
                int num838 = 0;
                float num839 = 2000f;
                for (int num840 = 0; num840 < 255; num840++)
                {
                    if (Main.player[num840].active && !Main.player[num840].dead)
                    {
                        Vector2 center9 = Main.player[num840].Center;
                        float num841 = Vector2.Distance(center9, npc.Center);
                        if (num841 < num839 && Collision.CanHit(npc.Center, 1, 1, center9, 1, 1))
                        {
                            array4[num838] = num840;
                            array5[num838] = center9;
                            if (++num838 >= array5.Length)
                            {
                                break;
                            }
                        }
                    }
                }
                for (int num842 = 0; num842 < num838; num842++)
                {
                    Vector2 vector82 = array5[num842] - npc.Center;
                    float ai = Main.rand.Next(100);
                    Vector2 vector83 = Vector2.Normalize(vector82.RotatedByRandom(0.78539818525314331)) * 14f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector83.X, vector83.Y, mod.ProjectileType("AthenaShock"), npc.damage, 0f, Main.myPlayer, vector82.ToRotation(), ai);
                }
            }
            Lighting.AddLight(npc.Center, 0f, 0.85f, 0.9f);
            if (npc.alpha < 150 && npc.ai[0] < 180f)
            {
                for (int num843 = 0; num843 < 1; num843++)
                {
                    float num844 = (float)Main.rand.NextDouble() * 1f - 0.5f;
                    if (num844 < -0.5f)
                    {
                        num844 = -0.5f;
                    }
                    if (num844 > 0.5f)
                    {
                        num844 = 0.5f;
                    }
                    Vector2 value47 = new Vector2(-npc.width * 0.2f * npc.scale, 0f).RotatedBy(num844 * 6.28318548f, default).RotatedBy(npc.velocity.ToRotation(), default);
                    int num845 = Dust.NewDust(npc.Center - Vector2.One * 5f, 10, 10, 226, -npc.velocity.X / 3f, -npc.velocity.Y / 3f, 150, Color.Transparent, 0.7f);
                    Main.dust[num845].position = npc.Center + value47;
                    Main.dust[num845].velocity = Vector2.Normalize(Main.dust[num845].position - npc.Center) * 2f;
                    Main.dust[num845].noGravity = true;
                }
                for (int num846 = 0; num846 < 1; num846++)
                {
                    float num847 = (float)Main.rand.NextDouble() * 1f - 0.5f;
                    if (num847 < -0.5f)
                    {
                        num847 = -0.5f;
                    }
                    if (num847 > 0.5f)
                    {
                        num847 = 0.5f;
                    }
                    Vector2 value48 = new Vector2(-npc.width * 0.6f * npc.scale, 0f).RotatedBy(num847 * 6.28318548f, default).RotatedBy(npc.velocity.ToRotation(), default);
                    int num848 = Dust.NewDust(npc.Center - Vector2.One * 5f, 10, 10, 226, -npc.velocity.X / 3f, -npc.velocity.Y / 3f, 150, Color.Transparent, 0.7f);
                    Main.dust[num848].velocity = Vector2.Zero;
                    Main.dust[num848].position = npc.Center + value48;
                    Main.dust[num848].noGravity = true;
                }
                return;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter >= 4)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
                if (npc.frame.Y >= frameHeight * 3)
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}