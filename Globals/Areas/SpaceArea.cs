using Microsoft.Xna.Framework;
using Redemption.NPCs.Space;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Redemption.Globals
{
    public class SpaceArea : ModSystem
    {
        public static bool Active;
        public override void PreUpdateEntities()
        {
            Active = false;
        }
        public override void PreUpdateWorld()
        {
            if (!Active || Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 LiftPos = new(((2400 / 2) + 76) * 16, 583 * 16);
            if (!Terraria.NPC.AnyNPCs(ModContent.NPCType<SlayerBaseLift>()))
                Terraria.NPC.NewNPC(new EntitySource_SpawnNPC(), (int)LiftPos.X, (int)LiftPos.Y, ModContent.NPCType<SlayerBaseLift>(), 0, 0, 0, 581, 543);
        }
    }
}