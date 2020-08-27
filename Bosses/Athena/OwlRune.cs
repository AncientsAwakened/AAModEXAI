using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using AAMod.NPCs.Enemies.Sky;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Athena
{
    public class OwlRune : ModNPC
    {
        public override void SetDefaults()
        {
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.aiStyle = 0;
            npc.damage = Main.expertMode ? 50 : 84;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.width = 82;
            npc.height = 82;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.scale = .001f;
            npc.friendly = false;
            npc.damage = 50;
        }

        public override void AI()
        {
            if (npc.ai[1] == 0)
            {
                npc.alpha -= 5;
                npc.scale += .019f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[0]++;
                    if (npc.ai[0] >= 51)
                    {
                        npc.alpha = 0;
                        npc.scale = 1;
                        npc.ai[0] = 0;
                        npc.ai[1] = 1;
                        npc.netUpdate = true;
                    }
                }
            }
            else if (npc.ai[1] == 1)
            {
                if (npc.alpha <= 0)
                {
                    npc.alpha = 0;
                }
                if (npc.scale > 1)
                {
                    npc.scale = 1;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[0]++;
                    if (npc.ai[0] >= 300)
                    {
                        npc.ai[0] = 0;
                        npc.ai[1] = 2;
                        npc.netUpdate = true;
                    }
                }
                npc.TargetClosest();
                if (npc.ai[2]++ == 15)
                {
                    Projectile.NewProjectile(npc.position, new Vector2(8f, 8f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    Projectile.NewProjectile(npc.position, new Vector2(-8f, 8f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    Projectile.NewProjectile(npc.position, new Vector2(-8f, -8f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    Projectile.NewProjectile(npc.position, new Vector2(8f, -8f), mod.ProjectileType("SeraphFeather"), 0, 0);
                }
                else if (npc.ai[2] >= 30)
                {
                    Projectile.NewProjectile(npc.position, new Vector2(0f, 8f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    Projectile.NewProjectile(npc.position, new Vector2(-8f, 0f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    Projectile.NewProjectile(npc.position, new Vector2(0f, -8f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    Projectile.NewProjectile(npc.position, new Vector2(8f, 0f), mod.ProjectileType("SeraphFeather"), 0, 0);
                    npc.ai[2] = 0;
                }
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[0]++;
                    if (npc.ai[0] >= 51)
                    {
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
                npc.alpha += 5;
                npc.scale -= .019f;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, npc.frame, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}