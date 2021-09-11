using Terraria.ModLoader;
using Terraria;
using AAModEXAI.Buffs;
using AAModEXAI.Bosses.Akuma;
using AAModEXAI.Bosses.Akuma.Awakened;
using AAModEXAI.Bosses.Athena;
using AAModEXAI.Bosses.Athena.Olympian;
using AAModEXAI.Bosses.Shen;
using AAModEXAI.Bosses.Yamata;
using AAModEXAI.Bosses.Yamata.Awakened;
using AAModEXAI.Bosses.Zero;
using AAModEXAI.Bosses.Zero.Protocol;
using AAModEXAI.Bosses.Anubis.Forsaken;

using System;
using Microsoft.Xna.Framework;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI
{
	public class AAModEXPlayer : ModPlayer
	{
        #region Buff bools
        public bool AkumaPain = false;
        public bool YamataGravity = false;
        public bool YamataAGravity = false;
        public bool Yanked = false;
        public bool Unstable = false;

        public float DistancetoYamata = 0f;

        #endregion

        #region Accessory bools
        public bool DragonSerpentNecklace = false;
        public bool ForbiddenTele = false;
        public float ForbiddenCharge = 0;

        #endregion

        public override void OnEnterWorld(Player player)
		{
			Main.NewText("WARNING: SUPER AAMOD AI Detected. It will influence your playthrough seriously. If you are a new AA player, it is not recommended to experience it. ", 67, 110, 238, false);
            Main.NewText("GLHF.", 67, 110, 238, false);
		}

        public override void ResetEffects()
        {
            AkumaPain = false;
            YamataGravity = false;
            YamataAGravity = false;
            Yanked = false;
            Unstable = false;

            DragonSerpentNecklace = false;
            ForbiddenTele = false;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (DragonSerpentNecklace)
            {
                player.ApplyDamageToNPC(target, 30, 0, 0, false);
            }
        }

        public override void UpdateBadLifeRegen()
        {
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();
            if (AkumaPain)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;

                if ((player.onFire || player.frostBurn || player.onFire2 || modPlayer.dragonFire || modPlayer.discordInferno) && player.lifeRegen < 0)
                {
                    player.lifeRegen *= 2;
                }
            }
        }

        public override void PostUpdateRunSpeeds()
		{
            if (YamataGravity || YamataAGravity)
            {
                if (player.mount.CanFly)
                {
                    player.mount.Dismount(player);
                }

                if (player.wingTimeMax > 200) player.wingTimeMax = 200;
                player.wingTimeMax /= 2;
                if (player.wingTime > player.wingTimeMax)
                    player.wingTime = player.wingTimeMax;


                if (YamataAGravity)
                {
                    player.moveSpeed *= .58f;
                    player.maxRunSpeed *= .58f;
                    player.accRunSpeed *= .58f;
                }
            }

            if (Unstable)
            {
                bool flag = player.controlLeft;
                bool flag1 = player.controlUp || player.controlJump;
                player.controlLeft = player.controlRight;
                player.controlRight = flag;
                player.controlUp = player.controlDown;
                player.controlJump = player.controlDown;
                player.controlDown = flag1;
                
                player.moveSpeed *= Utils.NextFloat(Main.rand, 0.25f, 2f);
            }

            if (ForbiddenTele && (player.inventory[player.selectedItem].type == ItemID.RodofDiscord || player.HeldItem.type == ItemID.RodofDiscord))
            {
                player.moveSpeed *=  1.1f;
                player.maxRunSpeed *= 1.1f;
                player.accRunSpeed *= 1.1f;
            }
		}

        public override void PostUpdateMiscEffects()
		{
            if (player.endurance > .5f)
			{
                float enduranceboost = player.endurance / .5f;
                player.endurance = 1f - (float)Math.Pow(.5f, enduranceboost);
			}
        }

        public void AccessoryEffects()
        {
            if(ForbiddenTele)
            {

                if(ForbiddenCharge < 100)
                {
                    ForbiddenCharge ++;
                }
                else
                {
                    if(AAMod.AAMod.AccessoryAbilityKey.JustPressed)
                    {
                        Vector2 vector32;
                        vector32.X = (float)Main.mouseX + Main.screenPosition.X;
                        if (player.gravDir == 1f)
                        {
                            vector32.Y = (float)Main.mouseY + Main.screenPosition.Y;
                        }
                        else
                        {
                            vector32.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                        }
                        vector32.X -= (float)(player.width / 2);
                        if (vector32.X > 50f && vector32.X < (float)(Main.maxTilesX * 16 - 50) && vector32.Y > 50f && vector32.Y < (float)(Main.maxTilesY * 16 - 50))
                        {
                            int num276 = (int)(vector32.X / 16f);
                            int num277 = (int)(vector32.Y / 16f);
                            if (!Collision.SolidCollision(vector32, player.width, player.height))
                            {
                                for (int index = 0; index < 70; ++index)
                Main.dust[Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, ModContent.DustType<ForsakenDust>(), Color.Cyan, 1.2f)].velocity *= 0.5f;
                                player.Teleport(vector32, 5, 0);
                                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, vector32.X, vector32.Y, 1, 0, 0);
                            }
                        }
                        ForbiddenCharge = 0;
                    }
                }
            }
        }

        public override void PostUpdate()
        {
            AccessoryEffects();

            if(player.endurance >= 1f)
            {
                player.endurance = .8f;
            }

            for(int i = 0; i < 200; i++)
            {
                if(!Main.npc[i].boss || !Main.npc[i].active) continue;
                if(Main.npc[i].type == ModContent.NPCType<Bosses.Akuma.AkumaTransition>())
                {
                    NPC akuma = Main.npc[i];

                    if (akuma.ai[0] >= 660)
                    {
                        player.AddBuff(ModContent.BuffType<Buffs.BlazingPain>(), 2);
                    }
                }
                else if(Main.npc[i].type == ModContent.NPCType<Bosses.Akuma.Awakened.AkumaA>())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.BlazingPain>(), 2);
                }
                else if(Main.npc[i].type == ModContent.NPCType<Bosses.Yamata.Yamata>())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.YamataGravity>(), 10);
                }
                else if(Main.npc[i].type == ModContent.NPCType<Bosses.Yamata.Awakened.YamataA>())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.YamataAGravity>(), 10);
                    DistancetoYamata = (player.Center - Main.npc[i].Center).Length();
                    if(/* player.velocity + player.Center - Main.npc[i].Center).Length() > DistancetoYamata */ DistancetoYamata > 0 && YamataAGravity)
                    {
                        Vector2 dir = Vector2.Normalize(player.Center - Main.npc[i].Center);
                        player.velocity -= dir * (DistancetoYamata / 3200f);
                    }
                }
                else if(Main.npc[i].type == ModContent.NPCType<Bosses.Shen.ShenA>())
                {
                    NPC shen = Main.npc[i];

                    if (((ShenA)shen.modNPC).halfLifeAIChange)
                    {
                        player.AddBuff(ModContent.BuffType<Buffs.YamataAGravity>(), 10, true);
                        player.AddBuff(ModContent.BuffType<Buffs.BlazingPain>(), 10, true);
                    }
                }

                if (ModSupport.GetMod("CalamityMod") != null)
                {
                    bool revenge = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "revenge", false, true);
                    bool Death = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "death", false, true);

                    if (revenge && (Main.npc[i].type == ModContent.NPCType<Bosses.Shen.Shen>() || Main.npc[i].type == ModContent.NPCType<Bosses.Shen.ShenA>()))
                    {
                        player.AddBuff(ModContent.BuffType<Buffs.YamataAGravity>(), 10, true);
                        player.AddBuff(ModContent.BuffType<Buffs.BlazingPain>(), 10, true);
                    }
                }
            }
            /*
            if (NPC.AnyNPCs(mod.NPCType("AkumaTransition")))
            {
                int n = BaseAI.GetNPC(player.Center, mod.NPCType("AkumaTransition"), -1);
                NPC akuma = Main.npc[n];

                if (akuma.ai[0] >= 660)
                {
                    player.AddBuff(mod.BuffType("BlazingPain"), 2);
                }
            }
            else if (NPC.AnyNPCs(mod.NPCType("AkumaA")))
            {
                player.AddBuff(mod.BuffType("BlazingPain"), 2);
            }

            if (NPC.AnyNPCs(mod.NPCType("Yamata")))
            {
                player.AddBuff(mod.BuffType("YamataGravity"), 10);
            }

            if (NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                player.AddBuff(mod.BuffType("YamataAGravity"), 10);
            }

            if (NPC.AnyNPCs(mod.NPCType("ShenA")))
            {
                int n = BaseAI.GetNPC(player.Center, mod.NPCType("ShenA"), -1);
                NPC shen = Main.npc[n];

                if (((ShenA)shen.modNPC).halfLifeAIChange)
                {
                    player.AddBuff(mod.BuffType("YamataAGravity"), 10);
                    player.AddBuff(mod.BuffType("BlazingPain"), 10);
                }
            }

            
            if (ModSupport.GetMod("CalamityMod") != null)
            {
                bool revenge = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "revenge", false, true);
                bool Death = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "death", false, true);

                if (revenge && (NPC.AnyNPCs(ModContent.NPCType<Shen>()) || NPC.AnyNPCs(ModContent.NPCType<ShenA>())))
                {
                    player.AddBuff(mod.BuffType("YamataAGravity"), 10);
                    player.AddBuff(mod.BuffType("BlazingPain"), 10);
                }
            }
            */
        }
	}
}