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
    public class NovaFocus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova Focus");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.width = 62;
            npc.height = 42;
            npc.damage = 57;
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
            npc.knockBackResist = 0;
            npc.noGravity = true;
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
                Main.npc[ind].netUpdate2 = true; Main.npc[ind].netUpdate = true;
            }
        }

        public int body = -1;
        public float rotValue = -1f;
        public Vector2 pos;
        Projectile laser;

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

            int aiTimerFire = Main.expertMode ? 230 : 280;

            Vector2 vector2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
            float num1 = Main.player[npc.target].position.X + (player.width / 2) - vector2.X;
            float num2 = Main.player[npc.target].position.Y + (player.height / 2) - vector2.Y;
            float NewRotation = (float)Math.Atan2(num2, num1);
            npc.rotation = MathHelper.Lerp(npc.rotation, NewRotation, 1f / 25f);

            if (npc.ai[2] >= aiTimerFire)
            {
                npc.ai[3]++;
                if (npc.ai[3] >= 210)
                {
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    if (Main.netMode != NetmodeID.MultiplayerClient) laser.Kill();
                }
                else if (!AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("NovaRay")) && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("NovaRay"), (int)(npc.damage * 0.75f), 3f, Main.myPlayer, npc.whoAmI, 420)];
                    laser.velocity = BaseUtility.RotateVector(default, new Vector2(14f, 0f), laser.rotation);
                }
            }
            npc.direction = 1;
            npc.spriteDirection = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/NovaFocus_Glow");
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
