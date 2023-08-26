using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSettings : ProjectManager
{
    public static int seed = 0;
    public static int MapXSize = 64;
    public static int MapYSize = 64;

    public static float procentChanceOfWaterBecomeSand = 0.5f;
    public static float procentChanceOfSandBecomeSeaWater = 0.20f;
    public static float procentChanceOfLoseSandInSeaBecomeSeaWater = 0.98f;
    public static bool doubleCheckTurnLoseSandInSeaSeaWater = true;

    public static int waterMinRangeLandTilesDevider = 80;
    public static int waterMaxRangeLandTilesDevider = 55;
    public static int dirtMinRangeLandTilesDevider = 80;
    public static int dirtMaxRangeLandTilesDevider = 55;
    public static int forestMinRangeLandTilesDevider = 72;
    public static int forestMaxRangeLandTilesDevider = 45;
    public static int mountainMinRangeLandTilesDevider = 95;
    public static int mountainMaxRangeLandTilesDevider = 70;

    public static int stoneMinRangeLandTilesDevider = 160;
    public static int stoneMaxRangeLandTilesDevider = 145;
    public static float distanceBetweenStone = 1f;

    public static int treeMinRangeLandTilesDevider = 3;
    public static int treeMaxRangeLandTilesDevider = 1;
    public static float distanceBetweenTree = 0.3f;

    public static int chickenMinRangeLandTilesDevider = 60;
    public static int chickenMaxRangeLandTilesDevider = 55;
    public static float distanceBetweenChicken = 0.01f;

    public static int dirtMaxGenerations { get; set; } = 6;
    public static int dirtMaxGenerationsResultMultiplier { get; set; } = 12;

    public static int waterMaxGenerations { get; set; } = 7;
    public static int waterMaxGenerationsResultMultiplier { get; set; } = 10;

    public static int forestMaxGenerations { get; set; } = 8;
    public static int forestMaxGenerationsResultMultiplier { get; set; } = 11;

    public static int mountainMaxGenerations { get; set; } = 6;
    public static int mountainMaxGenerationsResultMultiplier { get; set; } = 8;

    public static List<Tile> Tiles = new List<Tile>();
    public static List<MapObject> Objects = new List<MapObject>();
    public static List<MapObject> Creatures = new List<MapObject>();
}
