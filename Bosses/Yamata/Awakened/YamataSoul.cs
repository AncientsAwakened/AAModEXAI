using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class YamataSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mire Soul");
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
            npc.damage = 200;
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
        
        public override void AI()
        {
            Lighting.AddLight(npc.Center, AAColor.YamataA.R / 255, AAColor.YamataA.G / 255, AAColor.YamataA.B / 255);
            AAAI.AIShadowflameGhost(npc, ref npc.ai, false, 660f, 0.3f, 15f, 0.2f, 8f, 5f, 10f, 0.4f, 0.4f, 0.95f, 5f);
            if (!NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                npc.life = 0;
            }
            if(Main.npc[(int)internalAI[0]].active && Main.npc[(int)internalAI[0]].life > 0)
            {
                float dist = npc.Distance(Main.npc[(int)internalAI[0]].Center);
                if (dist > 2000) npc.position = Main.npc[(int)internalAI[0]].Center;
            }
            if(internalAI[3] > 0) internalAI[3] --;
            else 
            {
                npc.active = false;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int Head = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"));
                    Main.npc[Head].ai[0] = internalAI[0];
                    if(!((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head2.active)
                    {
                        ((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head2 = Main.npc[Head];
                        Main.npc[Head].ai[1] = 300 * -3f;
                        Main.npc[Head].ai[2] = -500 * .7f;
                        Main.npc[Head].ai[3] = 3f;
                    }
                    else if(!((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head3.active)
                    {
                        ((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head3 = Main.npc[Head];
                        Main.npc[Head].ai[1] = 300 * -2f;
                        Main.npc[Head].ai[2] = -500 * .8f;
                        Main.npc[Head].ai[3] = 2f;
                    }
                    else if(!((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head4.active)
                    {
                        ((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head4 = Main.npc[Head];
                        Main.npc[Head].ai[1] = 300 * -1f;
                        Main.npc[Head].ai[2] = -500 * .9f;
                        Main.npc[Head].ai[3] = 1f;
                    }
                    else if(!((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head5.active)
                    {
                        ((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head5 = Main.npc[Head];
                        Main.npc[Head].ai[1] = 300 * 1f;
                        Main.npc[Head].ai[2] = -500 * .9f;
                        Main.npc[Head].ai[3] = 1f;
                    }
                    else if(!((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head6.active)
                    {
                        ((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head6 = Main.npc[Head];
                        Main.npc[Head].ai[1] = 300 * 2f;
                        Main.npc[Head].ai[2] = -500 * .8f;
                        Main.npc[Head].ai[3] = 2f;
                    }
                    else if(!((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head7.active)
                    {
                        ((YamataA)Main.npc[(int)internalAI[0]].modNPC).Head7 = Main.npc[Head];
                        Main.npc[Head].ai[1] = 300 * 3f;
                        Main.npc[Head].ai[2] = -500 * .7f;
                        Main.npc[Head].ai[3] = 3f;
                    }
                    else
                    {
                        Main.npc[Head].active = false;
                    }
                    
                    Main.npc[Head].netUpdate = true;
                }
            }
            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<YamataAuraDust>(), 0f, 0f, 100, default, 2f);
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
            Texture2D glowTex = mod.GetTexture("Bosses/Yamata/Awakened/YamataSoul");
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, GetGlowAlpha());
            BaseDrawing.DrawAfterimage(spritebatch, glowTex, 0, npc, 0.8f, 1f, 4, false, 0f, 0f, Color.White);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<DeBuffs.HydraToxin>(), 600);
            target.AddBuff(mod.BuffType("Yanked"), 120);
        }
    }
}
