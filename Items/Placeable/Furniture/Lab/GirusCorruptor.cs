using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Redemption.Tiles.Furniture.Lab;

namespace Redemption.Items.Placeable.Furniture.Lab
{
    public class GirusCorruptor : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to corrupt Xenomite, AI Chips, etc."
                + "\nFound in the Abandoned Laboratory");
			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<GirusCorruptorTile>(), 0);
			Item.width = 54;
			Item.height = 64;
			Item.maxStack = 9999;
			Item.value = Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
		}
	}
}