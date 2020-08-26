using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.IO;
using AAMod;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.AH.Ashe
{
    public class AsheRune : ModNPC
    {
        public override void SetDefaults()
        {
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.aiStyle = -1;
            npc.damage = Main.expertMode ? 50 : 84;
            npc.defense = Main.expertMode ? 1 : 1;
            npc.knockBackResist = 0.2f;
            npc.width = 82;
            npc.height = 82;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.scale = .001f;
            npc.friendly = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(count);
                writer.Write(Control);
                writer.Write(spinLeft);
                writer.Write(SpinCheck);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                count = reader.ReadInt();
                Control = reader.ReadInt();
                spinLeft = reader.ReadBool();
                SpinCheck = reader.ReadBool();
            }
        }

        public int count = 0;
        public int Control = 0;

        public bool spinLeft = false;
        public bool SpinCheck = false;

        public Vector2 Runeshootspeed = new Vector2();

        public override void AI()
        {
            if (!SpinCheck && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.Next(2) == 0)
                {
                    spinLeft = true;
                }
                SpinCheck = true;
                npc.netUpdate = true;
            }
            if (Control == 1)
            {
                npc.rotation += spinLeft ? .02f : -.02f;
                if (count == 0)
                {
                    if(Main.player[Main.npc[(int)npc.ai[3]].target].position - new Vector2(npc.ai[0], npc.ai[1]) == new Vector2(0f, 0f))
                    {
                        Runeshootspeed = new Vector2(0, 0);
                    }
                    else
                    {
                        Runeshootspeed = 10f * Vector2.Normalize(Main.player[Main.npc[(int)npc.ai[3]].target].position - new Vector2(npc.ai[0], npc.ai[1]));
                    }
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int SootProj = Projectile.NewProjectile(npc.Center.X + Runeshootspeed.X, npc.Center.Y + Runeshootspeed.Y, Runeshootspeed.X, Runeshootspeed.Y, mod.ProjectileType("AsheShot"), (int)npc.ai[2]/2, 0, Main.myPlayer, npc.whoAmI, 0);
                        Main.projectile[SootProj].alpha = 0;
                    }
                    npc.netUpdate = true;
                }
                
                if(count >= 60)
                {
                    Control = 2;
                    npc.netUpdate = true;
                }
                count ++;
            }
            else if (Control == 2)
            {
                npc.rotation -= spinLeft ? .02f : -.02f;
                if (npc.alpha < 255)
                {
                    npc.alpha += 5;
                }
                else
                {
                    npc.active = false;
                }
                if (npc.scale < 1)
                {
                    npc.scale -= .04f;
                }
            }
            else
            {
                npc.rotation += spinLeft ? .04f : -.04f;
                if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                }
                else
                {
                    npc.alpha = 0;
                    Control = 1;
                    npc.netUpdate = true;
                }
                if (npc.scale < 1)
                {
                    npc.scale += .04f;
                }
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawAura(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, auraPercent, 1.4f, npc.scale, npc.rotation, npc.direction, 1, default, 0, 0, ColorUtils.COLOR_GLOWPULSE);
            int red = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], red, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, npc.frame, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}