using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Anubis
{
    public class Locust : ModNPC
	{				
		public override void SetStaticDefaults()
		{
            Main.npcFrameCount[npc.type] = 4;
		}

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 38;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 2500;
            npc.defense = 130;
            npc.damage = 5;
            npc.HitSound = SoundID.NPCHit31;
            npc.DeathSound = SoundID.NPCDeath35;
            npc.knockBackResist = 0f;	
			npc.noTileCollide = true;		
			npc.defense = 40;
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;
		}

		public override void NPCLoot()
		{

		}
		
		public int body = -1;
		public float rotValue = -1f;
		public bool spawnedDust = false;

		public override void AI()
		{
			npc.noGravity = true;
			if(body == -1)
			{
				int npcID = BaseAI.GetNPC(npc.Center, mod.NPCType("Anubis"), 500f, null);	
				if(npcID >= 0) body = npcID;
			}
			if(body == -1) return;				
			NPC anubis = Main.npc[body];
			if(anubis == null || anubis.life <= 0 || !anubis.active || anubis.type != mod.NPCType("Anubis")){ BaseAI.KillNPCWithLoot(npc); return; }

			for (int m = npc.oldPos.Length - 1; m > 0; m--)
			{
				npc.oldPos[m] = npc.oldPos[m - 1];
			}
			npc.oldPos[0] = npc.position;

			int locust = ((Anubis)anubis.modNPC).LocustCount;
			if(rotValue == -1f) rotValue = (npc.ai[0] % locust) * ((float)Math.PI * 2f / locust);
			rotValue += 0.05f;
			while(rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;
			npc.Center = BaseUtility.RotateVector(anubis.Center, anubis.Center + new Vector2(160f, 0f), rotValue);

			npc.spriteDirection = (npc.position.X - npc.oldPos[1].X) < 0 ? -1 : 1;
			npc.rotation = (npc.position.X - npc.oldPos[1].X) * 0.05f;

            Player player = Main.player[anubis.target];
            BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("LocustSpit"), ref npc.ai[2], Main.expertMode ? 120 : 80, npc.damage / 2, 9, false);
		}

		public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
			Color lightColor = BaseDrawing.GetNPCColor(npc, null);
			if(Main.player[npc.target] != null && Main.player[npc.target].active && !Main.player[npc.target].dead) BaseDrawing.DrawAfterimage(sb, Main.npcTexture[npc.type], 0, npc, 2f, 0.9f, 2, true, 0f, 0f, lightColor);
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, lightColor);
			return false;
		}		
	}
}