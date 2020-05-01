using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI
{
    public class CalamityGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public float CalamityDR = 1f;

        public override void SetDefaults(NPC npc)
        {
            if (ModSupport.GetMod("CalamityMod") != null)
			{
                if(npc.type == mod.NPCType("Athena")) CalamityDR = 0.8f;
                if(npc.type == mod.NPCType("OlympianDragon")) CalamityDR = 0.8f;

                if(npc.type == mod.NPCType("Greed")) CalamityDR = 0.8f;

                if(npc.type == mod.NPCType("ForsakenAnubis")) CalamityDR = 0.65f;
                if(npc.type == mod.NPCType("CurseCircle")) CalamityDR = 0.65f;
                if(npc.type == mod.NPCType("CursedScarab")) CalamityDR = 0.65f;
                if(npc.type == mod.NPCType("CursedLocust")) CalamityDR = 0.65f;
                if(npc.type == mod.NPCType("Naddaha")) CalamityDR = 0.65f;
                if(npc.type == mod.NPCType("HorusSentry")) CalamityDR = 0.65f;

                if(npc.type == mod.NPCType("Ashe")) CalamityDR = 0.6f;
                if(npc.type == mod.NPCType("AsheDragon")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("AsheOrbiter")) CalamityDR = 0.7f;

                if(npc.type == mod.NPCType("Haruka")) CalamityDR = 0.6f;

                if(npc.type == mod.NPCType("AkumaA")) CalamityDR = 0.4f;
                if(npc.type == mod.NPCType("Akuma")) CalamityDR = 0.4f;
                if(npc.type == mod.NPCType("AwakenedLung")) CalamityDR = 0.4f;
                if(npc.type == mod.NPCType("AncientLung")) CalamityDR = 0.4f;

                if(npc.type == mod.NPCType("AthenaA")) CalamityDR = 0.7f;
                if(npc.type == mod.NPCType("Seraph")) CalamityDR = 0.7f;
                if(npc.type == mod.NPCType("SeraphA")) CalamityDR = 0.7f;

                if(npc.type == mod.NPCType("DaybringerHead")) CalamityDR = 0.4f;
                if(npc.type == mod.NPCType("NightcrawlerHead")) CalamityDR = 0.4f;
                if(npc.type == mod.NPCType("NCCloud")) CalamityDR = 0.6f;
                
                if(npc.type == mod.NPCType("GreedA")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("GreedMinion")) CalamityDR = 0.7f;

                if(npc.type == mod.NPCType("SupremeRajah")) CalamityDR = 0.6f;

                if(npc.type == mod.NPCType("AbyssGrip")) CalamityDR = 0.1f;
                if(npc.type == mod.NPCType("BlazeGrip")) CalamityDR = 0.1f;
                if(npc.type == mod.NPCType("FuryAshe")) CalamityDR = 0.07f;
                if(npc.type == mod.NPCType("WrathHaruka")) CalamityDR = 0.07f;
                if(npc.type == mod.NPCType("Shen")) CalamityDR = 0.1f;
                if(npc.type == mod.NPCType("ShenA")) CalamityDR = 0.05f;
                if(npc.type == mod.NPCType("FuryAsheOrbiter")) CalamityDR = 0.1f;
                if(npc.type == mod.NPCType("Shenling")) CalamityDR = 0.05f;

                if(npc.type == mod.NPCType("YamataA")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataAHead")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataAHeadF")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataAHeadF1")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataAHeadF2")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("Yamata")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataHead")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataHeadF")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataHeadF1")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("YamataHeadF2")) CalamityDR = 0.5f;

                if(npc.type == mod.NPCType("ZeroEcho")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("ZeroMini")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("ZeroProtocol")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("GenocideCannon")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("Neutralizer")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("NovaFocus")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("OmegaVolley")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("RealityCannon")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("RiftShredder")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("SearcherZero")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("Taser")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("TeslaHand")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("VoidStar")) CalamityDR = 0.5f;
                if(npc.type == mod.NPCType("Zero")) CalamityDR = 0.5f;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (ModSupport.GetMod("CalamityMod") != null)
			{
                if (npc.realLife > 0 && Main.npc[npc.realLife].GetGlobalNPC<CalamityGlobalNPC>().CalamityDR < 1f) CalamityDR = Main.npc[npc.realLife].GetGlobalNPC<CalamityGlobalNPC>().CalamityDR;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
		{
            if (ModSupport.GetMod("CalamityMod") != null)
			{
                if (npc.type > 580 && npc.modNPC.mod == AAModEXAI.instance && npc.boss)
                {
                    bool revenge = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "revenge", false, true);
                    bool Death = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "death", false, true);
                    if(!NPC.downedMoonlord)
                    {
                        damage = (int)(damage * (1.1f + (revenge? 0.2f:0f) + (Death? 0.3f:0f)));
                    }
                    else
                    {
                        damage = (int)(damage * (1.2f + (revenge? 0.4f:0f) + (Death? 0.6f:0f)));
                    }
                }
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (ModSupport.GetMod("CalamityMod") != null)
			{
                if (npc.type > 580 && npc.boss && npc.modNPC.mod == AAModEXAI.instance)
                {
                    if (item.type > 3930 && item.modItem.mod == ModLoader.GetMod("CalamityMod"))
                    {
                        damage = (int)(damage * CalamityDR * (NPC.downedPlantBoss? 0.8f : 1f) * (NPC.downedMoonlord? 0.7f : 1f));
                    }
                }
                if (npc.type > 580 && npc.boss && npc.modNPC.mod == ModLoader.GetMod("CalamityMod"))
                {
                    if (item.type > 3930 && item.modItem.mod == AAModEXAI.instance)
                    {
                        damage = (int)(damage * (NPC.downedPlantBoss? 1.25f : 1f) * (NPC.downedMoonlord? 1.42f : 1f));
                    }
                }
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
            if (ModSupport.GetMod("CalamityMod") != null)
			{
                if (npc.type > 580 && npc.boss && npc.modNPC.mod == AAModEXAI.instance)
                {
                    if (projectile.type > 714 && projectile.modProjectile.mod == ModLoader.GetMod("CalamityMod"))
                    {
                        damage = (int)(damage * CalamityDR * (NPC.downedPlantBoss? 0.8f : 1f) * (NPC.downedMoonlord? 0.7f : 1f));
                    }
                }
                if (npc.type > 580 && npc.boss && npc.modNPC.mod == ModLoader.GetMod("CalamityMod"))
                {
                    if (projectile.type > 714 && projectile.modProjectile.mod == AAModEXAI.instance)
                    {
                        damage = (int)(damage * (NPC.downedPlantBoss? 1.25f : 1f) * (NPC.downedMoonlord? 1.42f : 1f));
                    }
                }
            }
		}
    }

    public class CalamityGlobalProjectile : GlobalProjectile
    {
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
		{
            if (ModSupport.GetMod("CalamityMod") != null)
			{
                if (projectile.hostile && !projectile.friendly && projectile.type > 714 && projectile.modProjectile.mod == AAModEXAI.instance)
                {
                    bool revenge = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "revenge", false, true);
                    bool Death = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "death", false, true);
                    if(!NPC.downedMoonlord)
                    {
                        damage = (int)(damage * (1.1f + (revenge? 0.2f:0f) + (Death? 0.3f:0f)));
                    }
                    else
                    {
                        damage = (int)(damage * (1.2f + (revenge? 0.4f:0f) + (Death? 0.6f:0f)));
                    }
                }
            }
		}
    }
}
