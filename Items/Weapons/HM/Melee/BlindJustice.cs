using Microsoft.Xna.Framework;
using Redemption.Items.Materials.PreHM;
using Redemption.Projectiles.Melee;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Redemption.Items.Weapons.HM.Melee
{
    public class BlindJustice : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blind Justice, Demon's Terror");
            Tooltip.SetDefault("Hold left-click to charge a Radiance Spin Slash\n" +
                "Right-click to shoot a spectral scythe, dealing Arcane damage\n" +
                "Deals double damage against demonic enemies");

            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            // Common Properties
            Item.width = 72;
            Item.height = 72;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 3, 50);

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = false;

            // Weapon Properties
            Item.damage = 108;
            Item.knockBack = 6;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.channel = true;

            // Projectile Properties
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<BlindJustice_Proj>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noMelee = false;
                Item.noUseGraphic = false;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 20)
                .AddIngredient(ModContent.ItemType<LostSoul>(), 10)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                SoundEngine.PlaySound(SoundID.Item71, player.position);
                damage = (int)(damage * 0.75f);
                type = ModContent.ProjectileType<SpectralScythe_Proj>();
            }
            else
                type = ModContent.ProjectileType<BlindJustice_Proj>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                TooltipLine line = new(Mod, "Lore",
                    "'Elegant and holy, this weapon was owned by Eymen - a well-renowned member of the High Templars.\n" +
                    "He imbued the blade with part of his permanent soul to keep it holy far past his own death.\n" +
                    "Eymen treated his weapon like his own flesh, and used it to smite numerous demons of high calibre.'")
                {
                    OverrideColor = Color.LightGray
                };
                tooltips.Add(line);
            }
            else
            {
                TooltipLine line = new(Mod, "HoldShift", "Hold [Shift] to view lore")
                {
                    OverrideColor = Color.Gray,
                };
                tooltips.Add(line);
            }
        }
    }
}
