using Microsoft.Xna.Framework;
using Redemption.WorldGeneration.Space;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace Redemption.Biomes
{
    public class SpaceBiome : ModBiome
    {
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("Redemption/SpaceBgStyle");
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.OtherworldlySpace;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Upper Atmosphere");
        }
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SpaceSub>();
        }
    }
}