﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Redemption.Projectiles.Magic;

namespace Redemption.Items.Weapons.PostML.Magic
{
    public class Petridish : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mitosis");
            Tooltip.SetDefault("Throw a Petridish filled with bacteria");
        }

        public override void SetDefaults()
		{
            Item.damage = 92;
            Item.width = 24;
            Item.height = 20;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Petridish_Proj>();
            Item.shootSpeed = 14f;
        }
    }
}
