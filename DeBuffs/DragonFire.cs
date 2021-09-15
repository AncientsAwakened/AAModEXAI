using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace AAModEXAI.DeBuffs
{
    public class DragonFire : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dragon Fire");
			Description.SetDefault("Your damage output is reduced");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetModPlayer<AAModEXPlayer>().dragonFire = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Dusts.DragonflameDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 107);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 0.7f, 0.2f, 0.1f);
            
        }


    }
    public class DragonFireDamageReduction : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<DeBuffs.DragonFire>()))
            {
                damage = (int)(damage * .8f); //this is a better way to reduce the player's damage from debuff since this will effect already summoned minions
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (player.HasBuff(ModContent.BuffType<DeBuffs.DragonFire>()))
            {
                damage = (int)(damage * .8f);
            }
        }
    }
}
