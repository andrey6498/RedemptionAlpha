using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Redemption.Items.Placeable.Tiles;
using Redemption.Dusts.Tiles;
using Redemption.Items.Tools.PostML;
using Redemption.Globals;

namespace Redemption.Tiles.Tiles
{
    public class SlayerShipPanelTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = ModContent.DustType<LabPlatingDust>();
            ItemDrop = ModContent.ItemType<SlayerShipPanel>();
            MinPick = 500;
            MineResist = 7f;
            HitSound = SoundID.Tink;
            AddMapEntry(new Color(72, 70, 79));
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<NanoAxe2>())
                return true;
            return WorldGen.gen || RedeBossDowned.downedOmega3 || RedeBossDowned.downedNebuleus;
        }
        public override bool Slope(int i, int j) => true;
        public override bool CanExplode(int i, int j) => false;
    }
}