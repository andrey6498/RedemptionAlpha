using Terraria.ModLoader;
using Terraria.ID;
using Redemption.Tiles.Furniture.SlayerShip;
using Terraria.GameContent.Creative;
using Redemption.Items.Placeable.Tiles;
using Redemption.Items.Materials.HM;

namespace Redemption.Items.Placeable.Furniture.SlayerShip
{
    public class CyberTeleporter : ModItem
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
		{
            Item.DefaultToPlaceableTile(ModContent.TileType<CyberTeleporterTile>(), 0);
            Item.width = 36;
            Item.height = 14;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.LightPurple;
		}
    }
}