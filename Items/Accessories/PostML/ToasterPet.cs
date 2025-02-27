using Microsoft.Xna.Framework;
using Redemption.Buffs.Pets;
using Redemption.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Redemption.Items.Accessories.PostML
{
    public class ToasterPet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Household Heatray");
            Tooltip.SetDefault("Summons a toaster to follow you" +
                "\n'Remember, all toasters toast toast!'");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<ToasterPet_Proj>(), ModContent.BuffType<ToasterPetBuff>());
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.sellPrice(0, 5);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            return false;
        }
    }
}