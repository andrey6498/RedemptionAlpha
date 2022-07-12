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
using Terraria.ID;
using ReLogic.Content;
using Terraria.DataStructures;
using Redemption.Base;
using Redemption.Tiles.Furniture.Lab;
using Redemption.Tiles.MusicBoxes;
using System.Linq;

namespace Redemption.WorldGeneration.Space
{
    public class SpaceSub : Subworld
    {
        public override int Width => 2400;
        public override int Height => 1200;
        public override bool NormalUpdates => false;
        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => false;
        public override List<GenPass> Tasks => new()
        {
            new SpacePass1("Loading", 1),
            new SpacePass2("Smoothing Asteroids", 0.3f),
        };
        public override void OnLoad()
        {
            Main.cloudAlpha = 0;
            Main.cloudBGAlpha = 0;
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
            Main.spawnTileY = 612;
            Main.spawnTileX = 2400 / 2;
            Main.worldSurface = Main.maxTilesY - 42;
            Main.rockLayer = Main.maxTilesY + 42;
            for (int i = 0; i < 100; i++)
            {
                int X = WorldGen.genRand.Next(100, 2400 - 100);
                int Y = WorldGen.genRand.Next(100, 612);
                MakeMeteor(X, Y);
            }
            SlayerBase1();
        }
        private readonly int WIDTH = 168;
        private readonly int HEIGHT = 128;
        public void SlayerBase1()
        {
            Mod mod = Redemption.Instance;
            Dictionary<Color, int> colorToTile = new()
            {
                [new Color(255, 0, 0)] = ModContent.TileType<AsteroidTile>(),
                [new Color(0, 255, 0)] = ModContent.TileType<SlayerShipPanelTile>(),
                [new Color(255, 255, 0)] = ModContent.TileType<HalogenLampTile>(),
                [new Color(0, 255, 255)] = ModContent.TileType<ShipGlassTile>(),
                [new Color(0, 0, 255)] = ModContent.TileType<MetalSupportBeamTile>(),
                [new Color(0, 255, 150)] = TileID.TinPlating,
                [new Color(0, 150, 255)] = TileID.CopperPlating,
                [new Color(255, 0, 150)] = TileID.TeamBlockPink,
                [new Color(150, 0, 255)] = TileID.TeamBlockGreen,
                [new Color(255, 0, 255)] = TileID.TeamBlockBlue,
                [new Color(150, 150, 150)] = -2, //turn into air
                [Color.Black] = -1 //don't touch when genning
            };
            Dictionary<Color, int> colorToWall = new()
            {
                [new Color(255, 0, 0)] = ModContent.WallType<SlayerShipPanelWallTile>(),
                [new Color(0, 255, 255)] = WallID.Glass,
                [new Color(255, 255, 0)] = WallID.MartianConduit,
                [Color.Black] = -1
            };
            Texture2D tex = ModContent.Request<Texture2D>("Redemption/WorldGeneration/Space/SlayerBase1", AssetRequestMode.ImmediateLoad).Value;
            Texture2D texWalls = ModContent.Request<Texture2D>("Redemption/WorldGeneration/Space/SlayerBase1_Walls", AssetRequestMode.ImmediateLoad).Value;
            Texture2D texSlopes = ModContent.Request<Texture2D>("Redemption/WorldGeneration/Space/SlayerBase1_Slopes", AssetRequestMode.ImmediateLoad).Value;

            Point16 origin = new((2400 / 2) - 24, 510);
            bool placed = false;
            bool genned = false;
            while (!genned)
            {
                if (placed)
                    continue;

                GenUtils.InvokeOnMainThread(() =>
                {
                    TexGen gen = BaseWorldGenTex.GetTexGenerator(tex, colorToTile, texWalls, colorToWall, null, texSlopes);
                    gen.Generate(origin.X, origin.Y, true, true);
                    genned = true;
                });
                placed = true;
            }

            for (int i = origin.X + 114; i < origin.X + 127; i++)
            {
                for (int j = origin.Y + 31; j < origin.Y + 32; j++)
                {
                    if (Framing.GetTileSafely(i, j).TileType == ModContent.TileType<SlayerShipPanelTile>() && WorldGen.InWorld(i, j))
                        Wiring.ActuateForced(i, j);
                }
            }
            for (int i = origin.X + 97; i < origin.X + 103; i++)
            {
                for (int j = origin.Y + 23; j < origin.Y + 32; j++)
                {
                    if (Framing.GetTileSafely(i, j).TileType == ModContent.TileType<SlayerShipPanelTile>() && WorldGen.InWorld(i, j))
                        Wiring.ActuateForced(i, j);
                }
            }

            GenUtils.ObjectPlace(origin.X + 24, origin.Y + 103, TileID.Teleporter);
            GenUtils.ObjectPlace(origin.X + 20, origin.Y + 90, ModContent.TileType<LabWorkbenchTile>());
            GenUtils.ObjectPlace(origin.X + 20, origin.Y + 89, ModContent.TileType<KSBoxTile>());
            GenUtils.ObjectPlace(origin.X + 24, origin.Y + 79, ModContent.TileType<LabCeilingLampTile>());
            GenUtils.ObjectPlace(origin.X + 106, origin.Y + 32, ModContent.TileType<LabTableTile>());
            GenUtils.ObjectPlace(origin.X + 106, origin.Y + 30, ModContent.TileType<LabComputerTile>());
            GenUtils.ObjectPlace(origin.X + 108, origin.Y + 32, ModContent.TileType<LabChairTile>());
            GenUtils.ObjectPlace(origin.X + 113, origin.Y + 26, ModContent.TileType<LabCeilingMonitorTile>());
            GenUtils.ObjectPlace(origin.X + 127, origin.Y + 26, ModContent.TileType<LabCeilingMonitorTile>(), 0, 1);
            GenUtils.ObjectPlace(origin.X + 134, origin.Y + 16, ModContent.TileType<LabCeilingLampTile>());
            GenUtils.ObjectPlace(origin.X + 132, origin.Y + 32, ModContent.TileType<LabWorkbenchTile>());
            GenUtils.ObjectPlace(origin.X + 132, origin.Y + 31, ModContent.TileType<LabComputerTile>());
            GenUtils.ObjectPlace(origin.X + 131, origin.Y + 32, ModContent.TileType<LabChairTile>(), 0, 1);
            GenUtils.ObjectPlace(origin.X + 89, origin.Y + 32, ModContent.TileType<ServerCabinetTile>(), 0, 1);
            GenUtils.ObjectPlace(origin.X + 91, origin.Y + 32, ModContent.TileType<ServerCabinetTile>());
            GenUtils.ObjectPlace(origin.X + 83, origin.Y + 32, ModContent.TileType<ServerCabinetTile>(), 0, 1);
            GenUtils.ObjectPlace(origin.X + 81, origin.Y + 32, ModContent.TileType<ServerCabinetTile>());
            GenUtils.ObjectPlace(origin.X + 92, origin.Y + 26, ModContent.TileType<LabChairTile>());
            GenUtils.ObjectPlace(origin.X + 92, origin.Y + 26, ModContent.TileType<LabChairTile>());
            GenUtils.ObjectPlace(origin.X + 90, origin.Y + 26, ModContent.TileType<LabTableTile>());
            GenUtils.ObjectPlace(origin.X + 90, origin.Y + 24, ModContent.TileType<LabComputerTile>());
            GenUtils.ObjectPlace(origin.X + 88, origin.Y + 26, ModContent.TileType<LabChairTile>(), 0, 1);
            GenUtils.ObjectPlace(origin.X + 85, origin.Y + 26, ModContent.TileType<LabChairTile>());
            GenUtils.ObjectPlace(origin.X + 83, origin.Y + 26, ModContent.TileType<LabTableTile>());
            GenUtils.ObjectPlace(origin.X + 83, origin.Y + 24, ModContent.TileType<LabComputerTile>());
            GenUtils.ObjectPlace(origin.X + 81, origin.Y + 26, ModContent.TileType<LabChairTile>(), 0, 1);
            GenUtils.ObjectPlace(origin.X + 61, origin.Y + 32, ModContent.TileType<LabRailTile_L>());
            for (int i = 62; i < 77; i++)
                GenUtils.ObjectPlace(origin.X + i, origin.Y + 32, ModContent.TileType<LabRailTile_Mid>());
            GenUtils.ObjectPlace(origin.X + 77, origin.Y + 32, ModContent.TileType<LabRailTile_R>(), 0, 1);

            for (int i = origin.X; i < origin.X + WIDTH; i++)
            {
                for (int j = origin.Y; j < origin.Y + HEIGHT; j++)
                {
                    switch (Framing.GetTileSafely(i, j).TileType)
                    {
                        case TileID.TeamBlockPink:
                            Framing.GetTileSafely(i, j).ClearTile();
                            WorldGen.PlaceTile(i, j, ModContent.TileType<LabPlatformTile>(), true);
                            WorldGen.SlopeTile(i, j, 1);
                            break;
                        case TileID.TeamBlockGreen:
                            Framing.GetTileSafely(i, j).ClearTile();
                            WorldGen.PlaceTile(i, j, ModContent.TileType<LabPlatformTile>(), true);
                            WorldGen.SlopeTile(i, j, 2);
                            break;
                    }
                    if (Framing.GetTileSafely(i, j).TileType == TileID.TeamBlockBlue)
                    {
                        Framing.GetTileSafely(i, j).ClearTile();
                        WorldGen.PlaceTile(i, j, ModContent.TileType<LabPlatformTile>(), true);
                    }
                    if ((Framing.GetTileSafely(i, j).TileType == TileID.TinPlating || Framing.GetTileSafely(i, j).TileType == TileID.CopperPlating) && WorldGen.InWorld(i, j))
                        Framing.GetTileSafely(i, j).TileColor = PaintID.BlackPaint;
                }
            }
            for (int i = origin.X + 98; i < origin.X + 99; i++)
            {
                for (int j = origin.Y + 33; j < origin.Y + 98; j++)
                {
                    if (Framing.GetTileSafely(i, j).WallType == WallID.MartianConduit && WorldGen.InWorld(i, j))
                        Framing.GetTileSafely(i, j).WallColor = PaintID.BlackPaint;
                }
            }
            for (int i = origin.X + 101; i < origin.X + 102; i++)
            {
                for (int j = origin.Y + 33; j < origin.Y + 98; j++)
                {
                    if (Framing.GetTileSafely(i, j).WallType == WallID.MartianConduit && WorldGen.InWorld(i, j))
                        Framing.GetTileSafely(i, j).WallColor = PaintID.BlackPaint;
                }
            }
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
    public class SpacePass2 : GenPass
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Smoothing Tiles";
            int[] TileArray = { ModContent.TileType<AsteroidTile>() };
            for (int i = 0; i < 2400; i++)
            {
                for (int j = 0; j < 1200; j++)
                {
                    if (TileArray.Contains(Framing.GetTileSafely(i, j).TileType) && WorldGen.InWorld(i, j))
                        BaseWorldGen.SmoothTiles(i, j, i, j);
                }
            }
        }
        public SpacePass2(string name, float loadWeight) : base(name, loadWeight)
        {
        }
    }
}
