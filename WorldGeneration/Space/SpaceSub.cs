using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;
using Redemption.Tiles.Tiles;
using Redemption.Tiles.Ores;
using Redemption.Walls;
using Terraria.WorldBuilding;
using Terraria.IO;
using ReLogic.Content;
using Terraria.DataStructures;
using Redemption.Base;

namespace Redemption.WorldGeneration.Space
{
    public class SpaceSub : Subworld
    {
        public override int Width => Main.maxTilesX;
        public override int Height => 1200;
        public override bool NormalUpdates => false;
        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => false;
        public override List<GenPass> Tasks => new()
        {
            new SpacePass1("Loading", 1),
        };
        public override void OnLoad()
        {
            Main.cloudAlpha = 0;
            Main.numClouds = 0;
            Main.rainTime = 0;
            Main.raining = false;
            Main.maxRaining = 0f;
            Main.slimeRain = false;
        }
        public override void OnUnload()
        {
        }
    }
    public class SpacePass1 : GenPass
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Loading";
            WorldGen.noTileActions = true;
            Main.spawnTileY = 612;
            Main.spawnTileX = Main.maxTilesX / 2;
            Main.worldSurface = Main.maxTilesY - 42;
            Main.rockLayer = Main.maxTilesY + 42;
            for (int i = 0; i < 100; i++)
            {
                int X = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int Y = WorldGen.genRand.Next(100, 612);
                MakeMeteor(X, Y);
            }
            Mod mod = Redemption.Instance;
            Dictionary<Color, int> colorToTile = new()
            {
                [new Color(255, 0, 0)] = ModContent.TileType<AsteroidTile>(),
                [new Color(0, 255, 0)] = ModContent.TileType<SlayerShipPanelTile>(),
                [new Color(150, 150, 150)] = -2, //turn into air
                [Color.Black] = -1 //don't touch when genning
            };
            Texture2D tex = ModContent.Request<Texture2D>("Redemption/WorldGeneration/Space/AstGen1", AssetRequestMode.ImmediateLoad).Value;
            Point16 origin = new((Main.maxTilesX / 2) - 14, 612);
            GenUtils.InvokeOnMainThread(() =>
            {
                TexGen gen = BaseWorldGenTex.GetTexGenerator(tex, colorToTile);
                gen.Generate(origin.X, origin.Y, true, true);
            });
        }
        public static void MakeMeteor(int X, int Y)
        {
            Mod mod = Redemption.Instance;
            Dictionary<Color, int> colorToTile = new()
            {
                [new Color(255, 0, 0)] = ModContent.TileType<AsteroidTile>(),
                [new Color(150, 150, 150)] = -2, //turn into air
                [Color.Black] = -1 //don't touch when genning
            };
            Texture2D tex = ModContent.Request<Texture2D>("Redemption/WorldGeneration/Space/AstGen" + (WorldGen.genRand.Next(24) + 2), AssetRequestMode.ImmediateLoad).Value;
            Point16 origin = new(X, Y);
            GenUtils.InvokeOnMainThread(() =>
            {
                TexGen gen = BaseWorldGenTex.GetTexGenerator(tex, colorToTile);
                gen.Generate(origin.X, origin.Y, true, true);
            });
        }
        public SpacePass1(string name, float loadWeight) : base(name, loadWeight)
        {
        }
    }
}
