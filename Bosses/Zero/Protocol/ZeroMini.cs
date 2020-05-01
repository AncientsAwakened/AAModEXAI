using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.Graphics.Shaders;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class ZeroMini : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ZER0 SELF 0RGANIZATI0N");
            Main.npcFrameCount[npc.type] = 12; 
        }
        public override void SetDefaults()
        {
            npc.lifeMax = 8000;
            npc.damage = 100;
            npc.defense = 80;
            npc.knockBackResist = 0f;
            npc.width = 52;
            npc.height = 52;
            npc.friendly = false;
            npc.aiStyle = -1;
            npc.value = Item.sellPrice(0, 0, 0, 0);
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCHit, "Sounds/Sounds/Zerohit");
            npc.DeathSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/ZeroDeath");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return AAColor.Oblivion;
        }

        public Vector2 point = new Vector2(0f,0f);

        int body = -1;
        public override void AI()
        {
            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(npc.Center, mod.NPCType("ZeroProtocol"), -1, null);
                if (npcID >= 0) body = npcID;
            }

            if (body == -1) return;

            NPC zero = Main.npc[body];

            npc.TargetClosest(true);

            Player player = Main.player[npc.target];

            if(npc.ai[0] == 0)
            {
                npc.velocity *= 0;
            }
            else if(npc.ai[0] == 1)
            {
                npc.ai[1] ++;
                if(npc.ai[1] % 180 == 60)
                {
                    if(Main.netMode != 1)
                    {
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, -14f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, 14f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(14f, 0f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-14f, 0f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                    }
                }
                if(npc.ai[1] % 180 == 120)
                {
                    if(Main.netMode != 1)
                    {
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(10f, -10f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-10f, -10f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-10f, 10f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(10f, 10f), mod.ProjectileType("ProtoStar"), npc.damage/2, 3);
                    }
                }
            }
            else if(npc.ai[0] == 2)
            {
                npc.velocity *= 0;
                if(Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.Center + new Vector2(30, 30), new Vector2(10, 10), mod.ProjectileType("EchoRay"), npc.damage / 3, 0f, Main.myPlayer, 6.2831855f / 750f, npc.whoAmI);
                    Projectile.NewProjectile(npc.Center + new Vector2(-30, 30), new Vector2(-10, 10), mod.ProjectileType("EchoRay"), npc.damage / 3, 0f, Main.myPlayer, 6.2831855f / 750f, npc.whoAmI);
                    Projectile.NewProjectile(npc.Center + new Vector2(30, -30), new Vector2(10, -10), mod.ProjectileType("EchoRay"), npc.damage / 3, 0f, Main.myPlayer, 6.2831855f / 750f, npc.whoAmI);
                    Projectile.NewProjectile(npc.Center + new Vector2(-30, -30), new Vector2(-10, -10), mod.ProjectileType("EchoRay"), npc.damage / 3, 0f, Main.myPlayer, 6.2831855f / 750f, npc.whoAmI);
                }
                npc.ai[0] = 3;
                npc.ai[1] = 0;
                npc.netUpdate = true;
            }
            else if(npc.ai[0] == 3)
            {
                npc.velocity *= 0;
                npc.ai[1]++;
                if(npc.ai[1] >= 90)
                {
                    npc.ai[1] = 0;
                    npc.ai[0] = 1;
                    npc.netUpdate = true;
                }
                return;
            }
            else
            {
                if (Main.rand.Next(2) == 0)
                {
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("GlitchBomb"), ref npc.ai[3], 50, npc.damage / 3, 10, true);
                }
                else
                {
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("GlitchRocket"), ref npc.ai[3], 50, npc.damage / 3, 10, true);
                }
            }

            npc.ai[2] ++;

            if(npc.ai[2] > 360 && npc.ai[0] != 2)
            {
                npc.ai[0] = Main.rand.Next(2) == 0? 1:3;
                npc.netUpdate = true;
            }

            if(zero.ai[0] == 5 && zero.ai[3] == 1f)
            {
                npc.ai[0] = 2;
                npc.netUpdate = true;
            }
            else if((npc.ai[2] > 360 && npc.ai[0] != 2) || npc.ai[0] == 2)
            {
                npc.ai[0] = Main.rand.Next(2) == 0? 1:3;
                npc.ai[1] = 0;
                npc.netUpdate = true;
                npc.ai[2] = 0;
            }

            if(npc.ai[0] != 3 && npc.ai[0] != 2 && npc.ai[0] != 0)
            {
                if((npc.Center - player.Center).Length() > 400f)
                {
                    MoveToPoint(player.Center);
                }
                else
                {
                    BaseAI.AISkull(npc, ref Move, true, 14, 350, .04f, .05f);
                }
            }
        }

        public float[] Move = new float[4];

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 12f;

            if (Vector2.Distance(npc.Center, point) > 500)
            {
                moveSpeed = 18f;
            }

            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 10)
            {
                npc.frameCounter = 0;
                Frame += 1;
            }

            if (Frame < 0)
            {
                Frame = 0;
            }
            else if (Frame < 6)
            {
                npc.ai[0] = 0;
            }
            else if (Frame > 11)
            {
                Frame = 6;
            }

            npc.frame.Y = frameHeight * Frame;
        }

        public override void NPCLoot()
        {
            DeathDust();
        }

        public void DeathDust()
        {
            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 226, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 5; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num90].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 15; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num92].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }

    }
}
