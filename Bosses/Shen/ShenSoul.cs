using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using AAMod;


namespace AAModEXAI.Bosses.Shen
{
    public class ShenSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Soul");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.ShadowFlameApparition);
            aiType = NPCID.ShadowFlameApparition;
            npc.aiStyle = NPCID.ShadowFlameApparition;
            animationType = NPCID.ShadowFlameApparition;
            npc.npcSlots = 0;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.aiStyle = 86;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.damage = 250;
            npc.alpha = 255;

        }

        public float[] internalAI = new float[4];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }
        public override bool PreAI()
        {
            if (!npc.HasPlayerTarget) npc.TargetClosest();
            if (internalAI[3] > 0)
            {
                npc.rotation = 0;
                NPC shen = Main.npc[(int)internalAI[0]];
                npc.direction = shen.direction;
                if(!AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("ShenWaveDeathraySmall")) && !AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("ShenWaveDeathray")))
                {
                    npc.Center = new Vector2(shen.Center.X + internalAI[1] * shen.direction, shen.Center.Y + internalAI[2]);
                }
                if(shen.ai[0] == 0 && shen.ai[2] == 240 && Main.netMode != 1)
                {
                    float ai0 = (float)Math.PI * 2 / 300 * (internalAI[3] == 2 ? 1 : -1) * Math.Sign(internalAI[2]) * shen.direction;
                            Projectile.NewProjectile(npc.Center, Vector2.UnitX, mod.ProjectileType("ShenWaveDeathraySmall"), npc.damage, 0f, Main.myPlayer, ai0, npc.whoAmI);
                }
                if(shen.ai[0] == 0 && shen.ai[1] == npc.ai[3] * 60 - 30)
                {
                    if (Main.netMode != 1)
                        Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[shen.target].Center), mod.ProjectileType("ShenDeathray"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                }
                if((shen.ai[0] == 1 && shen.ai[1] > 110) || (shen.ai[0] == 13 && shen.ai[1] > 350))
                {
                    internalAI[3] = 0;
                }
                return false;
            }
            return base.PreAI();
        }
        public override void AI()
        {
            Lighting.AddLight(npc.Center, AAColor.Shen3.R / 255, AAColor.Shen3.G / 255, AAColor.Shen3.B / 255);
            AAAI.AIShadowflameGhost(npc, ref npc.ai, false, 660f, 0.3f, 15f, 0.2f, 8f, 5f, 10f, 0.4f, 0.4f, 0.95f, 5f);
            if (!NPC.AnyNPCs(ModContent.NPCType<ShenA>()))
            {
                npc.life = 0;
            }
            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("Discord"), 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            npc.alpha -= 12;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
        }

        public Color GetGlowAlpha()
        {
            return new Color(200, 0, 50) * (Main.mouseTextColor / 255f);
        }
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = mod.GetTexture("Bosses/Shen/ShenSoul");
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, GetGlowAlpha());
            BaseDrawing.DrawAfterimage(spritebatch, glowTex, 0, npc, 0.8f, 1f, 4, false, 0f, 0f, Color.White);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("DiscordInferno"), 600);
            target.AddBuff(mod.BuffType("Yanked"), 120);
        }
    }
}
