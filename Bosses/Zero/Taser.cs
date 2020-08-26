using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Zero
{
    [AutoloadBossHead]
    public class Taser : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gigataser");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 70;
            npc.damage = 40;
            npc.defense = 90;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCHit4;
            npc.lifeMax = 30000;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0.0f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
            npc.lavaImmune = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.knockBackResist = 0;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * .7f);
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType("Zero")))
            {
                return false;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            bool flag = npc.life <= 0 || (!npc.active && NPC.AnyNPCs(mod.NPCType("Zero")));
            if (flag && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int ind = NPC.NewNPC((int)(npc.position.X + (double)(npc.width / 2)), (int)npc.position.Y + (npc.height / 2), mod.NPCType("TeslaHand"), npc.whoAmI, npc.ai[0], npc.ai[1], npc.ai[2], npc.ai[3], npc.target);
                Main.npc[ind].Center = npc.Center;
                Main.npc[ind].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                Main.npc[ind].velocity *= 8f;
                Main.npc[ind].netUpdate2 = true; Main.npc[ind].netUpdate = true;
            }
        }


        public int body = -1;
        public float rotValue = -1f;
        public Vector2 pos;

        public override void AI()
        {
            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(npc.Center, mod.NPCType("Zero"), 1000, null);
                if (npcID >= 0) body = npcID;
            }

            if (body == -1) return;

            NPC zero = Main.npc[body];
            if (zero == null || zero.life <= 0 || !zero.active || zero.type != mod.NPCType("Zero")) { npc.active = false; return; }

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            int probeNumber = ((Zero)zero.modNPC).WeaponCount;
            if (rotValue == -1f) rotValue = npc.ai[0] % probeNumber * ((float)Math.PI * 2f / probeNumber);
            rotValue += Main.expertMode ? .05f : 0f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;
            npc.Center = BaseUtility.RotateVector(zero.Center, zero.Center + new Vector2(((Zero)zero.modNPC).Distance, 0f), rotValue);

            if (Main.netMode != NetmodeID.MultiplayerClient) { npc.ai[2]++; }

            Player player = Main.player[zero.target];

            int aiTimerFire = Main.expertMode ? 120 : 180;

            if (npc.ai[2] == aiTimerFire)
            {
                npc.ai[2] = 0;
                if (Collision.CanHit(npc.position, npc.width, npc.height, player.Center, player.width, player.height))
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
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 vector83 = Vector2.Normalize(vector82.RotatedByRandom(0.78539818525314331)) * 20f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector83.X , vector83.Y, mod.ProjectileType("ZeroShock"), npc.damage / 2, 0f, Main.myPlayer, vector82.ToRotation(), ai);
                        }
                    }
                }
            }

            Vector2 vector2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
            float num1 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector2.X;
            float num2 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector2.Y;
            float NewRotation = (float)Math.Atan2(num2, num1) - 1.57f;
            npc.rotation = MathHelper.Lerp(npc.rotation, NewRotation, 1f / 30f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/TaserZ");
            BaseDrawing.DrawAfterimage(spriteBatch, tex, 0, npc, 1, 1, 6, true, 0, 0, Color.DarkRed, npc.frame);
            BaseDrawing.DrawTexture(spriteBatch, tex, 0, npc, drawColor);
            BaseDrawing.DrawTexture(spriteBatch, glowTex, 0, npc, AAColor.COLOR_WHITEFADE1);
            return false;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
    }
}