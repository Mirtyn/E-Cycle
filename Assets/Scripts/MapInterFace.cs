using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.Mathematics;
using UnityEngine.AI;
using System;
using Unity.AI.Navigation;

public class MapInterFace : MapSettings
{
    public GameObject Map;

    [SerializeField] private LayerMask tileLayerMask;

    private List<Tile> Tiles = new List<Tile>();
    private List<MapObject> Objects = new List<MapObject>();
    private List<MapObject> Creatures = new List<MapObject>();

    [SerializeField] private GameObject DirtTile_1;
    [SerializeField] private GameObject ForestTile_1;
    [SerializeField] private GameObject ForestTile_2;
    [SerializeField] private GameObject ForestTile_3;
    [SerializeField] private GameObject GrassTile_1;
    [SerializeField] private GameObject MountainTile_1;
    [SerializeField] private GameObject MountainTile_2;
    [SerializeField] private GameObject MountainTile_3;
    [SerializeField] private GameObject SandTile_1;
    [SerializeField] private GameObject WaterTile_1;
    [SerializeField] private GameObject Stone_1;
    [SerializeField] private GameObject Stone_2;
    [SerializeField] private GameObject Tree_1;
    [SerializeField] private GameObject Tree_2;
    [SerializeField] private GameObject Tree_3;
    [SerializeField] private GameObject Chicken;

    //private YRow[] YRows;
    private int AmountTiles = -1;
    private int AmountObjects = -1;

    private bool PlacedObj = false;

    //Random rnd;

    private void Awake()
    {
        
        LandGenerator();
    }

    private void FixedUpdate()
    {
        if (!PlacedObj)
        {
            PlacedObj = true;

            PlaceStones(MapObject._ObjectType.Stone, Stone_1, stoneMinRangeLandTilesDevider, stoneMaxRangeLandTilesDevider, distanceBetweenStone);
            PlaceStones(MapObject._ObjectType.Stone, Stone_2, stoneMinRangeLandTilesDevider, stoneMaxRangeLandTilesDevider, distanceBetweenStone);

            PlaceTrees(MapObject._ObjectType.Tree, Tree_1, treeMinRangeLandTilesDevider, treeMaxRangeLandTilesDevider, distanceBetweenTree);
            PlaceTrees(MapObject._ObjectType.Tree, Tree_2, treeMinRangeLandTilesDevider, treeMaxRangeLandTilesDevider, distanceBetweenTree);
            PlaceTrees(MapObject._ObjectType.Tree, Tree_2, treeMinRangeLandTilesDevider, treeMaxRangeLandTilesDevider, distanceBetweenTree);
            PlaceTrees(MapObject._ObjectType.Tree, Tree_3, treeMinRangeLandTilesDevider, treeMaxRangeLandTilesDevider, distanceBetweenTree);

            Map.GetComponent<NavMeshSurface>().BuildNavMesh();

            PlaceChickens(MapObject._ObjectType.Chicken, Chicken, chickenMinRangeLandTilesDevider, chickenMaxRangeLandTilesDevider, distanceBetweenChicken);
        }
    }

    private void LandGenerator()
    {
        //if (seed == 0)
        //{
        //    seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        //}

        if (seed == 0)
        {
            seed = new System.Random().Next(int.MinValue, int.MaxValue);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        Random.seed = seed;
#pragma warning restore CS0618 // Type or member is obsolete

        //rnd = new Random(seed);

        Debug.Log(seed);

        Debug.Log(Application.persistentDataPath + "/GeneratedMaps.txt");
        Debug.Log(Application.dataPath + "/.txt");
        //Debug.Log(Application. + "/test.txt");
        CreateTiles();

        GiveTilesAdjacentTiles();

        SetTileType();

        CreateDirt();

        CreateForest();

        CreateMountain();

        CreateMoreWater();

        SetSandAroundwater();

        TurnSeaWaterSurroundedSandToSeaWater();

        TurnLoseSandInSeaToSeaWater();

        WriteMapToTextFile();

        DrawMap();

        CheckIfAllTilesWereGeneratedOnMap();
    }

    private void PlaceChickens(MapObject._ObjectType objectType, GameObject g, int MinRangeLandTilesDevider, int MaxRangeLandTilesDevider, float distanceBetweenObject)
    {
        int amount = Random.Range((MapYSize * MapXSize) * 2 / MinRangeLandTilesDevider, ((MapYSize * MapXSize) * 2 / MaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < amount; i++)
        {
            float chosenXPos = Random.Range(0f, (float)MapXSize);
            float chosenYPos = Random.Range(0f, (float)MapYSize);

            //Tile chosenTile;
            foreach (Tile tile in Tiles)
            {
                if (tile.XPos == (int)chosenXPos && tile.YPos == (int)chosenYPos && (tile.TileType == Tile._TileType.Grass || tile.TileType == Tile._TileType.Dirt || tile.TileType == Tile._TileType.Sand))
                {
                    AmountObjects++;
                    MapObject mapObject = new MapObject();

                    mapObject.XPos = chosenXPos;
                    mapObject.YPos = chosenYPos;

                    mapObject.ObjectType = objectType;
                    mapObject.ObjectId = AmountObjects;

                    //chosenTile = tile;

                    float rndRotY = Random.Range(-180f, 180f);
                    mapObject.RotY = rndRotY;

                    Ray ray = new Ray();
                    RaycastHit hit;

                    ray.origin = new Vector3(chosenXPos, 20, chosenYPos);
                    ray.direction = new Vector3(chosenXPos, -20, chosenYPos) - new Vector3(chosenXPos, 0, chosenYPos);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                    {
                        mapObject.ObjectOnMap = Instantiate(g, hit.point, Quaternion.Euler(0, rndRotY, 0), Map.transform);
                        mapObject.Generated = true;
                        Objects.Add(mapObject);
                    }
                    else
                    {
                        Debug.LogError("Raycast Was created and drawn but never hit anything, " + chosenXPos + " " + chosenYPos);
                    }
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 10f);
                }

            }
        }
    }

    private void PlaceStones(MapObject._ObjectType objectType, GameObject g, int MinRangeLandTilesDevider, int MaxRangeLandTilesDevider, float distanceBetweenObject)
    {
        int amount = Random.Range((MapYSize * MapXSize) / MinRangeLandTilesDevider, ((MapYSize * MapXSize) / MaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < amount; i++)
        {
            float chosenXPos = Random.Range(0f, (float)MapXSize);
            float chosenYPos = Random.Range(0f, (float)MapYSize);

            //Tile chosenTile;
            foreach (Tile tile in Tiles)
            {
                if (tile.XPos == (int)chosenXPos && tile.YPos == (int)chosenYPos && tile.TileType != Tile._TileType.SeaWater && tile.TileType != Tile._TileType.RiverWater && tile.TileType != Tile._TileType.Mountain && !CheckThatThereIsNoObjectTooClose(chosenXPos, chosenYPos, distanceBetweenObject))
                {
                    AmountObjects++;
                    MapObject mapObject = new MapObject();

                    mapObject.XPos = chosenXPos;
                    mapObject.YPos = chosenYPos;

                    mapObject.ObjectType = objectType;
                    mapObject.ObjectId = AmountObjects;

                    //chosenTile = tile;

                    float rndRotY = Random.Range(-180f, 180f);
                    mapObject.RotY = rndRotY;

                    Ray ray = new Ray();
                    RaycastHit hit;

                    ray.origin = new Vector3(chosenXPos, 20, chosenYPos);
                    ray.direction = new Vector3(chosenXPos, -20, chosenYPos) - new Vector3(chosenXPos, 0, chosenYPos);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                    {
                        mapObject.ObjectOnMap = Instantiate(g, hit.point, Quaternion.Euler(0, rndRotY, 0), Map.transform);
                        mapObject.Generated = true;
                        Objects.Add(mapObject);
                    }
                    else
                    {
                        Debug.LogError("Raycast Was created and drawn but never hit anything, " + chosenXPos + " " + chosenYPos);
                    }
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 10f);
                }
                
                //else
                //{
                //    //List<MapObject> copyObjects = Objects;

                //    foreach (MapObject mapObject1 in Objects)
                //    {
                //        if (tile.XPos == (int)chosenXPos && tile.YPos == (int)chosenYPos && tile.TileType != Tile._TileType.SeaWater && tile.TileType != Tile._TileType.RiverWater && tile.TileType != Tile._TileType.Mountain && )
                //        {
                //            MapObject mapObject = new MapObject();

                //            mapObject.XPos = chosenXPos;
                //            mapObject.YPos = chosenYPos;

                //            mapObject.ObjectType = MapObject._ObjectType.Stone;

                //            //chosenTile = tile;

                //            float rndRotY = Random.Range(-180f, 180f);
                //            mapObject.RotY = rndRotY;

                //            Ray ray = new Ray();
                //            RaycastHit hit;

                //            ray.origin = new Vector3(chosenXPos, 20, chosenYPos);
                //            ray.direction = new Vector3(chosenXPos, -20, chosenYPos) - new Vector3(chosenXPos, 0, chosenYPos);

                //            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                //            {
                //                mapObject.ObjectOnMap = Instantiate(g, hit.point, Quaternion.Euler(0, rndRotY, 0));
                //                mapObject.Generated = true;
                //                Objects.Add(mapObject);
                //            }
                //            else
                //            {
                //                Debug.LogError("Raycast Was created and drawn but never hit anything, " + chosenXPos + " " + chosenYPos);
                //            }
                //            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 10f);
                //        }
                //    }
                //}
            }

            //MapObject[] copyObjects = Objects.ToArray();

            //foreach (MapObject mapObject1 in copyObjects)
            //{
            //    foreach (MapObject mapObject2 in copyObjects)
            //    {
            //        if (mapObject1 != mapObject2)
            //        {
            //            if (Math.CalculateDistance2D(new System.Numerics.Vector2(mapObject1.XPos, mapObject1.YPos), new System.Numerics.Vector2(mapObject2.XPos, mapObject2.YPos)) < distanceBetweenObject)
            //            {
            //                mapObject2.ObjectOnMap.SetActive(false);
            //                Objects.Remove(mapObject2);
            //            }
            //        }
            //    }
            //}
        }
    }

    private void PlaceTrees(MapObject._ObjectType objectType, GameObject g, int MinRangeLandTilesDevider, int MaxRangeLandTilesDevider, float distanceBetweenObject)
    {
        int amount = Random.Range((MapYSize * MapXSize) * 2 / MinRangeLandTilesDevider, ((MapYSize * MapXSize) * 2 / MaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < amount; i++)
        {
            float chosenXPos = Random.Range(0f, (float)MapXSize);
            float chosenYPos = Random.Range(0f, (float)MapYSize);

            //Tile chosenTile;
            foreach (Tile tile in Tiles)
            {
                if (tile.XPos == (int)chosenXPos && tile.YPos == (int)chosenYPos && tile.TileType == Tile._TileType.Forest && !CheckThatThereIsNoObjectTooClose(chosenXPos, chosenYPos, distanceBetweenObject))
                {
                    AmountObjects++;
                    MapObject mapObject = new MapObject();

                    mapObject.XPos = chosenXPos;
                    mapObject.YPos = chosenYPos;

                    mapObject.ObjectType = objectType;
                    mapObject.ObjectId = AmountObjects;

                    //chosenTile = tile;

                    float rndRotY = Random.Range(-180f, 180f);
                    mapObject.RotY = rndRotY;

                    Ray ray = new Ray();
                    RaycastHit hit;

                    ray.origin = new Vector3(chosenXPos, 20, chosenYPos);
                    ray.direction = new Vector3(chosenXPos, -20, chosenYPos) - new Vector3(chosenXPos, 0, chosenYPos);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                    {
                        mapObject.ObjectOnMap = Instantiate(g, hit.point, Quaternion.Euler(0, rndRotY, 0), Map.transform);
                        mapObject.Generated = true;
                        Objects.Add(mapObject);
                    }
                    else
                    {
                        Debug.LogError("Raycast Was created and drawn but never hit anything, " + chosenXPos + " " + chosenYPos);
                    }
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 10f);
                }

            }
        }
    }

    private void CheckIfAllTilesWereGeneratedOnMap()
    {
        foreach (Tile tile in Tiles)
        {
            if (!tile.Generated)
            {
                Debug.LogError("A Tile in the 'List<Tile> Tiles' was not drawn!\nTile: X: " + tile.XPos + ", Y: " + tile.YPos);
            }
        }
    }

    private void TurnLoseSandInSeaToSeaWater()
    {
        foreach (Tile tile in Tiles)
        {
            if (!tile.CheckSurroundingTileForMainLandNotSand())
            {
                float rndNum = Random.Range(0f, 1f);
                //float rndNum = (float)rnd.NextDouble();

                if (rndNum < procentChanceOfLoseSandInSeaBecomeSeaWater)
                {
                    tile.TileType = Tile._TileType.SeaWater;
                }
            }
        }
        if (doubleCheckTurnLoseSandInSeaSeaWater)
        {
            foreach (Tile tile in Tiles)
            {
                if (!tile.CheckSurroundingTileForMainLandNotSand())
                {
                    float rndNum = Random.Range(0f, 1f);
                    //float rndNum = (float)rnd.NextDouble();

                    if (rndNum < procentChanceOfLoseSandInSeaBecomeSeaWater)
                    {
                        tile.TileType = Tile._TileType.SeaWater;
                    }
                }
            }
        }
    }

    private void TurnSeaWaterSurroundedSandToSeaWater()
    {
        foreach (Tile tile in Tiles)
        {
            if (tile.CheckAllSurroundingTileForSameType(Tile._TileType.SeaWater))
            {
                float rndNum = Random.Range(0f, 1f);
                //float rndNum = (float)rnd.NextDouble();

                if (rndNum < procentChanceOfSandBecomeSeaWater)
                {
                    tile.TileType = Tile._TileType.SeaWater;
                }
            }
        }
    }

    private void CreateMountain()
    {
        int mRnd = Random.Range((MapYSize * MapXSize) / mountainMinRangeLandTilesDevider, ((MapYSize * MapXSize) / mountainMaxRangeLandTilesDevider) + 1);
        //int mRnd = rnd.Next((MapYSize * MapXSize) / mountainMinRangeLandTilesDevider, ((MapYSize * MapXSize) / mountainMaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < mRnd; i++)
        {
            int chosenTile = Random.Range(0, Tiles.Count);
            //int chosenTile = rnd.Next(0, Tiles.Count);
            if (Tiles[chosenTile].TileType == Tile._TileType.Grass ||
                Tiles[chosenTile].TileType == Tile._TileType.Dirt ||
                Tiles[chosenTile].TileType == Tile._TileType.Forest ||
                Tiles[chosenTile].TileType == Tile._TileType.Sand)
            {
                SetMountainOnMap(Tiles[chosenTile]);
            }
        }
    }

    private void CreateForest()
    {
        int fRnd = Random.Range((MapYSize * MapXSize) / forestMinRangeLandTilesDevider, ((MapYSize * MapXSize) / forestMaxRangeLandTilesDevider) + 1);
        //int fRnd = rnd.Next((MapYSize * MapXSize) / forestMinRangeLandTilesDevider, ((MapYSize * MapXSize) / forestMaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < fRnd; i++)
        {
            int chosenTile = Random.Range(0, Tiles.Count);
            //int chosenTile = rnd.Next(0, Tiles.Count);
            if (Tiles[chosenTile].TileType == Tile._TileType.Grass ||
                Tiles[chosenTile].TileType == Tile._TileType.Dirt)
            {
                SetForestOnMap(Tiles[chosenTile]);
            }
        }
    }

    private void CreateMoreWater()
    {
        int wRnd = Random.Range((MapYSize * MapXSize) / waterMinRangeLandTilesDevider, ((MapYSize * MapXSize) / waterMaxRangeLandTilesDevider) + 1);
        //int wRnd = rnd.Next((MapYSize * MapXSize) / waterMinRangeLandTilesDevider, ((MapYSize * MapXSize) / waterMaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < wRnd; i++)
        {
            int chosenTile = Random.Range(0, Tiles.Count);
            //int chosenTile = rnd.Next(0, Tiles.Count);
            if (Tiles[chosenTile].TileType == Tile._TileType.Grass ||
                Tiles[chosenTile].TileType == Tile._TileType.Dirt ||
                Tiles[chosenTile].TileType == Tile._TileType.Sand)
            {
                SetWaterOnMap(Tiles[chosenTile]);
            }
        }
    }

    private void CreateDirt()
    {
        int dRnd = Random.Range((MapYSize * MapXSize) / dirtMinRangeLandTilesDevider, ((MapYSize * MapXSize) / dirtMaxRangeLandTilesDevider) + 1);
        //int dRnd = rnd.Next((MapYSize * MapXSize) / dirtMinRangeLandTilesDevider, ((MapYSize * MapXSize) / dirtMaxRangeLandTilesDevider) + 1);

        for (int i = 0; i < dRnd; i++)
        {
            int chosenTile = Random.Range(0, Tiles.Count);
            //int chosenTile = rnd.Next(0, Tiles.Count);
            if (Tiles[chosenTile].TileType == Tile._TileType.Grass)
            {
                SetDirtOnMap(Tiles[chosenTile]);
            }
        }
    }

    private void GiveTilesAdjacentTiles()
    {
        foreach (Tile tile in Tiles)
        {
            // Find Above
            foreach (Tile tile2 in Tiles)
            {
                if (tile2.YPos == (tile.YPos + 1) && tile2.XPos == tile.XPos)
                {
                    tile.AboveTile = tile2;
                    break;
                }
            }

            // Find Down
            foreach (Tile tile2 in Tiles)
            {
                if (tile2.YPos == (tile.YPos - 1) && tile2.XPos == tile.XPos)
                {
                    tile.DownTile = tile2;
                    break;
                }
            }

            // Find Right
            foreach (Tile tile2 in Tiles)
            {
                if (tile2.XPos == (tile.XPos + 1) && tile2.YPos == tile.YPos)
                {
                    tile.RightTile = tile2;
                    break;
                }
            }

            // Find Left
            foreach (Tile tile2 in Tiles)
            {
                if (tile2.XPos == (tile.XPos - 1) && tile2.YPos == tile.YPos)
                {
                    tile.LeftTile = tile2;
                    break;
                }
            }

            if (tile.AboveTile == null)
            {
                tile.AboveTile = new Tile();
            }
            if (tile.DownTile == null)
            {
                tile.DownTile = new Tile();
            }
            if (tile.RightTile == null)
            {
                tile.RightTile = new Tile();
            }
            if (tile.LeftTile == null)
            {
                tile.LeftTile = new Tile();
            }
        }
    }

    private void CreateTiles()
    {
        for (int y = 0; y < MapYSize; y++)
        {
            for (int x = 0; x < MapXSize; x++)
            {
                AmountTiles += 1;
                Tile tile = new Tile();
                tile.InitializeTile();
                tile.XPos = x;
                tile.YPos = y;
                tile.TileId = AmountTiles;

                Tiles.Add(tile);
            }
        }
    }

    private void SetTileType()
    {
        foreach (Tile tile in Tiles)
        {
            //Check if edge of map
            if (tile.XPos == 0 || tile.YPos == 0)
            {
                tile.TileType = Tile._TileType.SeaWater;
            }
            else
            {
                //Tile._TileType w = Tile._TileType.Water;
                //Tile._TileType g = Tile._TileType.Grass;
                //Tile._TileType s = Tile._TileType.Sand;
                //Tile._TileType d = Tile._TileType.Dirt;

                // check for water around
                if (tile.CheckSurroundingTileForType(Tile._TileType.SeaWater))
                {

                    float rndNum = Random.Range(0f, 1f);
                    //float rndNum = (float)rnd.NextDouble();

                    if (rndNum < procentChanceOfWaterBecomeSand)
                    {
                        tile.TileType = Tile._TileType.Sand;
                    }
                    else
                    {
                        tile.TileType = Tile._TileType.SeaWater;
                    }
                }
                else
                {
                    tile.TileType = Tile._TileType.Grass;
                }
            }
        }
    }

    private void SetDirtOnMap(Tile tile)
    {
        int generation = 0;

        tile.TileType = Tile._TileType.Dirt;
        if (tile.AboveTile.TileType == Tile._TileType.Grass)
        {
            tile.AboveTile.SetAndSpreadDirt(generation);
        }
        if (tile.DownTile.TileType == Tile._TileType.Grass)
        {
            tile.DownTile.SetAndSpreadDirt(generation);
        }
        if (tile.RightTile.TileType == Tile._TileType.Grass)
        {
            tile.RightTile.SetAndSpreadDirt(generation);
        }
        if (tile.LeftTile.TileType == Tile._TileType.Grass)
        {
            tile.LeftTile.SetAndSpreadDirt(generation);
        }
    }

    private void SetForestOnMap(Tile tile)
    {
        int generation = 0;

        tile.TileType = Tile._TileType.Forest;
        if (tile.AboveTile.TileType == Tile._TileType.Grass ||
            tile.AboveTile.TileType == Tile._TileType.Dirt)
        {
            tile.AboveTile.SetAndSpreadForest(generation);
        }
        if (tile.DownTile.TileType == Tile._TileType.Grass ||
            tile.DownTile.TileType == Tile._TileType.Dirt)
        {
            tile.DownTile.SetAndSpreadForest(generation);
        }
        if (tile.RightTile.TileType == Tile._TileType.Grass ||
            tile.RightTile.TileType == Tile._TileType.Dirt)
        {
            tile.RightTile.SetAndSpreadForest(generation);
        }
        if (tile.LeftTile.TileType == Tile._TileType.Grass ||
            tile.LeftTile.TileType == Tile._TileType.Dirt)
        {
            tile.LeftTile.SetAndSpreadForest(generation);
        }
    }

    private void SetMountainOnMap(Tile tile)
    {
        int generation = 0;

        tile.TileType = Tile._TileType.Mountain;
        if (tile.AboveTile.TileType == Tile._TileType.Grass ||
            tile.AboveTile.TileType == Tile._TileType.Dirt ||
            tile.AboveTile.TileType == Tile._TileType.Forest ||
            tile.AboveTile.TileType == Tile._TileType.Sand)
        {
            tile.AboveTile.SetAndSpreadMountain(generation);
        }
        if (tile.DownTile.TileType == Tile._TileType.Grass ||
            tile.DownTile.TileType == Tile._TileType.Dirt ||
            tile.DownTile.TileType == Tile._TileType.Forest ||
            tile.DownTile.TileType == Tile._TileType.Sand)
        {
            tile.DownTile.SetAndSpreadMountain(generation);
        }
        if (tile.RightTile.TileType == Tile._TileType.Grass ||
            tile.RightTile.TileType == Tile._TileType.Dirt ||
            tile.RightTile.TileType == Tile._TileType.Forest ||
            tile.RightTile.TileType == Tile._TileType.Sand)
        {
            tile.RightTile.SetAndSpreadMountain(generation);
        }
        if (tile.LeftTile.TileType == Tile._TileType.Grass ||
            tile.LeftTile.TileType == Tile._TileType.Dirt ||
            tile.LeftTile.TileType == Tile._TileType.Forest ||
            tile.LeftTile.TileType == Tile._TileType.Sand)
        {
            tile.LeftTile.SetAndSpreadMountain(generation);
        }
    }

    private void SetWaterOnMap(Tile tile)
    {
        int generation = 0;

        tile.TileType = Tile._TileType.RiverWater;
        if (tile.AboveTile.TileType == Tile._TileType.Grass || 
            tile.AboveTile.TileType == Tile._TileType.Dirt || 
            tile.AboveTile.TileType == Tile._TileType.Sand)
        {
            tile.AboveTile.SetAndSpreadWater(generation);
        }
        if (tile.DownTile.TileType == Tile._TileType.Grass || 
            tile.AboveTile.TileType == Tile._TileType.Dirt || 
            tile.AboveTile.TileType == Tile._TileType.Sand)
        {
            tile.DownTile.SetAndSpreadWater(generation);
        }
        if (tile.RightTile.TileType == Tile._TileType.Grass || 
            tile.AboveTile.TileType == Tile._TileType.Dirt || 
            tile.AboveTile.TileType == Tile._TileType.Sand)
        {
            tile.RightTile.SetAndSpreadWater(generation);
        }
        if (tile.LeftTile.TileType == Tile._TileType.Grass || 
            tile.AboveTile.TileType == Tile._TileType.Dirt || 
            tile.AboveTile.TileType == Tile._TileType.Sand)
        {
            tile.LeftTile.SetAndSpreadWater(generation);
        }
    }

    void SetSandAroundwater()
    {
        foreach (Tile tile in Tiles)
        {
            if (tile.CheckSurroundingTileForType(Tile._TileType.SeaWater))
            {
                if (tile.TileType != Tile._TileType.SeaWater && tile.TileType != Tile._TileType.RiverWater && tile.TileType != Tile._TileType.Mountain)
                {
                    tile.TileType = Tile._TileType.Sand;
                }
            }
            else if (tile.CheckSurroundingTileForType(Tile._TileType.RiverWater))
            {
                if (tile.TileType != Tile._TileType.SeaWater && tile.TileType != Tile._TileType.RiverWater && tile.TileType != Tile._TileType.Mountain)
                {
                    tile.TileType = Tile._TileType.Sand;
                }
            }
        }
    }

    //private void WriteToTextFile()
    //{
    //    textFile.
    //}

    public void WriteMapToTextFile()    //WriteString()
    {
        string path = Application.dataPath + "/GeneratedMaps.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);

        //Debug.Log("Now: " + System.DateTime.Now);
        //Debug.Log("UtcNow: " + System.DateTime.UtcNow);
        //Debug.Log("Today: " + System.DateTime.Today);

        writer.Write("    -" + System.DateTime.Now.ToString() + "-" + "\n\n");
        writer.Write(" SETTINGS:");
        writer.Write("\n -Seed: " + seed);
        writer.Write("\n -MapXSize: " + MapXSize);
        writer.Write("\n -MapYSize: " + MapYSize);
        writer.Write("\n -AmountTiles: " + (AmountTiles + 1));
        writer.Write("\n -ProcentChanceOfWaterBecomeSand: " + procentChanceOfWaterBecomeSand);
        writer.Write("\n -ProcentChanceOfSandBecomeSeaWater: " + procentChanceOfSandBecomeSeaWater);
        writer.Write("\n -ProcentChanceOfLoseSandInSeaBecomeSeaWater: " + procentChanceOfLoseSandInSeaBecomeSeaWater);
        writer.Write("\n -DoubleCheckTurnLoseSandInSeaSeaWater: " + doubleCheckTurnLoseSandInSeaSeaWater);
        writer.Write("\n -WaterMinRangeLandTilesDevider: " + waterMinRangeLandTilesDevider);
        writer.Write("\n -WaterMaxRangeLandTilesDevider: " + waterMaxRangeLandTilesDevider);
        writer.Write("\n -DirtMinRangeLandTilesDevider: " + dirtMinRangeLandTilesDevider);
        writer.Write("\n -DirtMaxRangeLandTilesDevider: " + dirtMaxRangeLandTilesDevider);
        writer.Write("\n -ForestMinRangeLandTilesDevider: " + forestMinRangeLandTilesDevider);
        writer.Write("\n -ForestMaxRangeLandTilesDevider: " + forestMaxRangeLandTilesDevider);
        writer.Write("\n -MountainMinRangeLandTilesDevider: " + mountainMinRangeLandTilesDevider);
        writer.Write("\n -MountainMaxRangeLandTilesDevider: " + mountainMaxRangeLandTilesDevider);
        writer.Write("\n -StoneMinRangeLandTilesDevider: " + stoneMinRangeLandTilesDevider);
        writer.Write("\n -StoneMaxRangeLandTilesDevider: " + stoneMaxRangeLandTilesDevider);
        writer.Write("\n -DirtMaxGenerations: " + dirtMaxGenerations);
        writer.Write("\n -DirtMaxGenerationsResultMultiplier: " + dirtMaxGenerationsResultMultiplier);
        writer.Write("\n -WaterMaxGenerations: " + waterMaxGenerations);
        writer.Write("\n -WaterMaxGenerationsResultMultiplier: " + waterMaxGenerationsResultMultiplier);
        writer.Write("\n -ForestMaxGenerations: " + forestMaxGenerations);
        writer.Write("\n -ForestMaxGenerationsResultMultiplier: " + forestMaxGenerationsResultMultiplier);
        writer.Write("\n -MountainMaxGenerations: " + mountainMaxGenerations);
        writer.Write("\n -MountainMaxGenerationsResultMultiplier: " + mountainMaxGenerationsResultMultiplier);
        writer.Write("\n\n TILES: \n");
        writer.Write(" -None = 'o' \n");
        writer.Write(" -SeaWater = '~' \n");
        writer.Write(" -RiverWater = '-' \n");
        writer.Write(" -Grass = '=' \n");
        writer.Write(" -Dirt = '#' \n");
        writer.Write(" -Sand = '.' \n");
        writer.Write(" -Forest = '|' \n");
        writer.Write(" -Mountain = '^' \n");
        writer.Write("\n MAP: \n\n");

        writer.WriteLine("START!");

        foreach (Tile tile in Tiles)
        {
            if (tile.XPos == (MapXSize - 1))
            {
                switch (tile.TileType)
                {
                    case Tile._TileType.None:
                        writer.Write("o\n");
                        break;

                    case Tile._TileType.SeaWater:
                        writer.Write("~\n");
                        break;
                    case Tile._TileType.RiverWater:
                        writer.Write("-\n");
                        break;

                    case Tile._TileType.Grass:
                        writer.Write("=\n");
                        break;

                    case Tile._TileType.Dirt:
                        writer.Write("#\n");
                        break;

                    case Tile._TileType.Sand:
                        writer.Write(".\n");
                        break;
                    case Tile._TileType.Forest:
                        writer.Write("|\n");
                        break;
                    case Tile._TileType.Mountain:
                        writer.Write("^\n");
                        break;
                }
            }
            else
            {
                switch (tile.TileType)
                {
                    case Tile._TileType.None:
                        writer.Write("o");
                        break;

                    case Tile._TileType.SeaWater:
                        writer.Write("~");
                        break;
                    case Tile._TileType.RiverWater:
                        writer.Write("-");
                        break;

                    case Tile._TileType.Grass:
                        writer.Write("=");
                        break;

                    case Tile._TileType.Dirt:
                        writer.Write("#");
                        break;

                    case Tile._TileType.Sand:
                        writer.Write(".");
                        break;

                    case Tile._TileType.Forest:
                        writer.Write("|");
                        break;

                    case Tile._TileType.Mountain:
                        writer.Write("^");
                        break;
                }
            }
        }

        writer.WriteLine("END! \n \n");

        writer.Close();

        //StreamReader reader = new StreamReader(path);
        ////Print the text from the file
        ////Debug.Log(reader.ReadToEnd());
        //reader.Close();
    }

    private void DrawMap()
    {
        foreach (Tile tile in Tiles)
        {
            switch (tile.TileType)
            {
                case Tile._TileType.None:
                    Debug.LogError("A Tile with an unassigned TileType was tried to be rendered!\nTile: X: " + tile.XPos + ", Y: " + tile.YPos);
                    break;

                case Tile._TileType.SeaWater:
                    DrawTile(tile, Tile._TileType.SeaWater);
                    break;

                case Tile._TileType.RiverWater:
                    DrawTile(tile, Tile._TileType.RiverWater);
                    break;

                case Tile._TileType.Grass:
                    DrawTile(tile, Tile._TileType.Grass);
                    break;

                case Tile._TileType.Dirt:
                    DrawTile(tile, Tile._TileType.Dirt);
                    break;

                case Tile._TileType.Sand:
                    DrawTile(tile, Tile._TileType.Sand);
                    break;
                case Tile._TileType.Forest:
                    DrawTile(tile, Tile._TileType.Forest);
                    break;
                case Tile._TileType.Mountain:
                    DrawTile(tile, Tile._TileType.Mountain);
                    break;
            }
        }
    }

    private void DrawTile(Tile tile, Tile._TileType tileType)
    {
        int randomTileVariation = Random.Range(0, 3);
        //int randomTileVariation = rnd.Next(0, 3);
        GameObject g;
        switch (tile.TileType)
        {
            case Tile._TileType.RiverWater:
                // Tile variation

                // Tile 1
                g = Instantiate(WaterTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                tile.TileOnMap = g;
                RandomRotateTile(g);
                tile.Generated = true;

                //switch (randomTileVariation)
                //{
                //    case 0:
                //        // Tile 1
                //        g = Instantiate(WaterTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        break;
                //}
                break;
            case Tile._TileType.SeaWater:
                // Tile variation

                // Tile 1
                g = Instantiate(WaterTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                tile.TileOnMap = g;
                RandomRotateTile(g);
                tile.Generated = true;

                //switch (randomTileVariation)
                //{
                //    case 0:
                //        // Tile 1
                //        g = Instantiate(WaterTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        break;
                //}
                break;

            case Tile._TileType.Grass:
                // Tile variation

                // Tile 1
                g = Instantiate(GrassTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                tile.TileOnMap = g;
                RandomRotateTile(g);
                tile.Generated = true;

                //switch (randomTileVariation)
                //{
                //    case 0:
                //        // Tile 1
                //        g = Instantiate(GrassTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //    case 1:
                //        // Tile 2
                //        g = Instantiate(GrassTile_2, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //    case 2:
                //        // Tile 3
                //        g = Instantiate(GrassTile_3, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //}
                break;

            case Tile._TileType.Dirt:
                // Tile variation

                g = Instantiate(DirtTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                tile.TileOnMap = g;
                RandomRotateTile(g);
                tile.Generated = true;

                //switch (randomTileVariation)
                //{
                //    case 0:
                //        // Tile 1
                //        g = Instantiate(DirtTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //    case 1:
                //        // Tile 2
                //        g = Instantiate(DirtTile_2, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //    case 2:
                //        // Tile 3
                //        g = Instantiate(DirtTile_3, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //}
                break;

            case Tile._TileType.Sand:
                // Tile variation

                // Tile 1
                g = Instantiate(SandTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                tile.TileOnMap = g;
                RandomRotateTile(g);
                tile.Generated = true;

                //switch (randomTileVariation)
                //{
                //    case 0:
                //        // Tile 1
                //        g = Instantiate(SandTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //    case 1:
                //        // Tile 2
                //        g = Instantiate(SandTile_2, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //    case 2:
                //        // Tile 3
                //        g = Instantiate(SandTile_3, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity);
                //        RandomRotateTile(g);
                //        tile.Generated = true;
                //        break;
                //}
                break;
            case Tile._TileType.Forest:
                // Tile variation
                switch (randomTileVariation)
                {
                    case 0:
                        // Tile 1
                        g = Instantiate(ForestTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                        tile.TileOnMap = g;
                        RandomRotateTile(g);
                        tile.Generated = true;
                        break;
                    case 1:
                        // Tile 2
                        g = Instantiate(ForestTile_2, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                        tile.TileOnMap = g;
                        RandomRotateTile(g);
                        tile.Generated = true;
                        break;
                    case 2:
                        // Tile 3
                        g = Instantiate(ForestTile_3, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                        tile.TileOnMap = g;
                        RandomRotateTile(g);
                        tile.Generated = true;
                        break;
                }
                break;
            case Tile._TileType.Mountain:
                // Tile variation
                switch (randomTileVariation)
                {
                    case 0:
                        // Tile 1
                        g = Instantiate(MountainTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                        tile.TileOnMap = g;
                        RandomRotateTile(g);
                        tile.Generated = true;
                        break;
                    case 1:
                        // Tile 2
                        g = Instantiate(MountainTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                        tile.TileOnMap = g;
                        RandomRotateTile(g);
                        tile.Generated = true;
                        break;
                    case 2:
                        // Tile 3
                        g = Instantiate(MountainTile_1, new Vector3(tile.XPos, 0, tile.YPos), Quaternion.identity, Map.transform);
                        tile.TileOnMap = g;
                        RandomRotateTile(g);
                        tile.Generated = true;
                        break;
                }
                break;
        }
    }

    private void RandomRotateTile(GameObject g)
    {
        int randomTileRotation = Random.Range(0, 4);

        switch (randomTileRotation)
        {
            case 0:
                // 0 degrees
                g.transform.GetChild(0).eulerAngles = new Vector3(0, 0, 0);
                break;
            case 1:
                // 90 degrees
                g.transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
                break;
            case 2:
                // 180 degrees
                g.transform.GetChild(0).eulerAngles = new Vector3(0, 180, 0);
                break;
            case 3:
                // 270 degrees
                g.transform.GetChild(0).eulerAngles = new Vector3(0, 270, 0);
                break;
        }
    }

    private bool CheckThatThereIsNoObjectTooClose(float XPos1, float YPos1, float distanceBetweenObject)
    {
        foreach (MapObject obj in Objects)
        {
            if (Vector2.Distance(new Vector2(XPos1, YPos1), new Vector2(obj.XPos, obj.YPos)) < distanceBetweenObject)
            {
                Debug.Log("Stopped an object (;");
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}


//public class YRow
//{
//    public void CreateRow(int MapXSize, int AmountTiles, List<Tile> Tiles, int y)
//    {
//        for (int x = 0; x < MapXSize; x++)
//        {
//            AmountTiles += 1;
//            Tile tile = new Tile();
//            tile.XPos = x;
//            tile.YPos = y;

//            Tiles.Add(tile);
//        }
//    }
//}

public class MapObject
{
    public int ObjectId;
    public GameObject ObjectOnMap;
    public bool Generated = false;

    public float XPos;
    public float YPos;

    public float RotY;

    public enum _ObjectType { None, Stone, Tree, Chicken };
    public _ObjectType ObjectType { get; internal set; }
}

public class Tile
{
    public int TileId;
    public GameObject TileOnMap;

    public bool Generated = false;

    public float MapXSize;
    public float MapYSize;

    public float XPos;
    public float YPos;

    public Tile AboveTile = null;
    public Tile DownTile = null;
    public Tile RightTile = null;
    public Tile LeftTile = null;

    public enum _TileType { None, Grass, SeaWater, RiverWater, Sand, Dirt, Forest, Mountain };
    public _TileType TileType { get; internal set; }

    public int dirtMaxGenerations;
    public int dirtMaxGenerationsResultMultiplier;

    public int waterMaxGenerations;
    public int waterMaxGenerationsResultMultiplier;

    public int forestMaxGenerations;
    public int forestMaxGenerationsResultMultiplier;

    public int mountainMaxGenerations;
    public int mountainMaxGenerationsResultMultiplier;

    public void InitializeTile()
    {
        MapXSize = MapSettings.MapXSize;
        MapYSize = MapSettings.MapYSize;
        dirtMaxGenerations = MapSettings.dirtMaxGenerations;
        dirtMaxGenerationsResultMultiplier = MapSettings.dirtMaxGenerationsResultMultiplier;

        waterMaxGenerations = MapSettings.waterMaxGenerations;
        waterMaxGenerationsResultMultiplier = MapSettings.waterMaxGenerationsResultMultiplier;

        forestMaxGenerations = MapSettings.forestMaxGenerations;
        forestMaxGenerationsResultMultiplier = MapSettings.forestMaxGenerationsResultMultiplier;

        mountainMaxGenerations = MapSettings.mountainMaxGenerations;
        mountainMaxGenerationsResultMultiplier = MapSettings.mountainMaxGenerationsResultMultiplier;
    }

    public void SetAndSpreadMountain(int generation)
    {
        generation += 1;

        float rngNumber = Random.Range(0f, 1f);
        //float rngNumber = (float)rnd.NextDouble();
        float chance = (float)(Mathf.Abs(generation - mountainMaxGenerations) * mountainMaxGenerationsResultMultiplier) / 100;

        if (rngNumber < chance)
        {
            TileType = Tile._TileType.Mountain;

            if (YPos != MapYSize || YPos != 0)
            {
                if (AboveTile.TileType == Tile._TileType.Grass ||
                AboveTile.TileType == Tile._TileType.Dirt ||
                AboveTile.TileType == Tile._TileType.Forest ||
                AboveTile.TileType == Tile._TileType.Sand)
                {
                    AboveTile.SetAndSpreadMountain(generation);
                }
                if (DownTile.TileType == Tile._TileType.Grass ||
                    DownTile.TileType == Tile._TileType.Dirt ||
                    DownTile.TileType == Tile._TileType.Forest ||
                    DownTile.TileType == Tile._TileType.Sand)
                {
                    DownTile.SetAndSpreadMountain(generation);
                }
            }

            if (XPos != MapXSize || XPos != 0)
            {
                if (RightTile.TileType == Tile._TileType.Grass ||
                RightTile.TileType == Tile._TileType.Dirt ||
                RightTile.TileType == Tile._TileType.Forest ||
                RightTile.TileType == Tile._TileType.Sand)
                {
                    RightTile.SetAndSpreadMountain(generation);
                }
                if (LeftTile.TileType == Tile._TileType.Grass ||
                    LeftTile.TileType == Tile._TileType.Dirt ||
                    LeftTile.TileType == Tile._TileType.Forest ||
                    LeftTile.TileType == Tile._TileType.Sand)
                {
                    LeftTile.SetAndSpreadMountain(generation);
                }
            }
        }
    }

    public void SetAndSpreadForest(int generation)
    {
        generation += 1;

        float rngNumber = Random.Range(0f, 1f);
        //float rngNumber = (float)rnd.NextDouble();
        float chance = (float)(Mathf.Abs(generation - forestMaxGenerations) * forestMaxGenerationsResultMultiplier) / 100;

        if (rngNumber < chance)
        {
            TileType = Tile._TileType.Forest;

            if (YPos != MapYSize || YPos != 0)
            {
                if (AboveTile.TileType == Tile._TileType.Grass ||
                AboveTile.TileType == Tile._TileType.Dirt)
                {
                    AboveTile.SetAndSpreadForest(generation);
                }
                if (DownTile.TileType == Tile._TileType.Grass ||
                    DownTile.TileType == Tile._TileType.Dirt)
                {
                    DownTile.SetAndSpreadForest(generation);
                }
            }
            if (XPos != MapXSize || XPos != 0)
            {
                if (RightTile.TileType == Tile._TileType.Grass ||
                RightTile.TileType == Tile._TileType.Dirt)
                {
                    RightTile.SetAndSpreadForest(generation);
                }
                if (LeftTile.TileType == Tile._TileType.Grass ||
                    LeftTile.TileType == Tile._TileType.Dirt)
                {
                    LeftTile.SetAndSpreadForest(generation);
                }
            }
        }
    }

    public void SetAndSpreadDirt(int generation)
    {
        generation += 1;

        float rngNumber = Random.Range(0f, 1f);
        //float rngNumber = (float)rnd.NextDouble();
        float chance = (float)(Mathf.Abs(generation - dirtMaxGenerations) * dirtMaxGenerationsResultMultiplier) / 100;

        if (rngNumber < chance)
        {
            TileType = Tile._TileType.Dirt;

            if (YPos != MapYSize || YPos != 0)
            {
                if (AboveTile.TileType == Tile._TileType.Grass)
                {
                    AboveTile.SetAndSpreadDirt(generation);
                }
                if (DownTile.TileType == Tile._TileType.Grass)
                {
                    DownTile.SetAndSpreadDirt(generation);
                }
            }
            if (XPos != MapXSize || XPos != 0)
            {
                if (RightTile.TileType == Tile._TileType.Grass)
                {
                    RightTile.SetAndSpreadDirt(generation);
                }
                if (LeftTile.TileType == Tile._TileType.Grass)
                {
                    LeftTile.SetAndSpreadDirt(generation);
                }
            }
        }
    }

    public void SetAndSpreadWater(int generation)
    {
        generation += 1;

        float rngNumber = Random.Range(0f, 1f);
        //float rngNumber = (float)rnd.NextDouble();
        float chance = (float)(Mathf.Abs(generation - waterMaxGenerations) * waterMaxGenerationsResultMultiplier) / 100;

        if (rngNumber < chance)
        {
            TileType = Tile._TileType.RiverWater;

            if (YPos != MapYSize || YPos != 0)
            {
                if (AboveTile.TileType == Tile._TileType.Grass ||
                AboveTile.TileType == Tile._TileType.Dirt ||
                AboveTile.TileType == Tile._TileType.Sand)
                {
                    AboveTile.SetAndSpreadWater(generation);
                }
                if (DownTile.TileType == Tile._TileType.Grass ||
                    AboveTile.TileType == Tile._TileType.Dirt ||
                    AboveTile.TileType == Tile._TileType.Sand)
                {
                    DownTile.SetAndSpreadWater(generation);
                }
            }
            if (XPos != MapXSize || XPos != 0)
            {
                if (RightTile.TileType == Tile._TileType.Grass ||
                AboveTile.TileType == Tile._TileType.Dirt ||
                AboveTile.TileType == Tile._TileType.Sand)
                {
                    RightTile.SetAndSpreadWater(generation);
                }
                if (LeftTile.TileType == Tile._TileType.Grass ||
                    AboveTile.TileType == Tile._TileType.Dirt ||
                    AboveTile.TileType == Tile._TileType.Sand)
                {
                    LeftTile.SetAndSpreadWater(generation);
                }
            }
        }
    }

    public bool CheckSurroundingTileForMainLandNotSand()
    {
        if (CheckSurroundingTileForType(_TileType.Forest) || CheckSurroundingTileForType(_TileType.Dirt) || CheckSurroundingTileForType(_TileType.Grass) || CheckSurroundingTileForType(_TileType.Mountain))
        {
            return true;
        }
        else { return false; }
    }

    public bool CheckSurroundingTileForType(Tile._TileType tileType)
    {
        if (AboveTile.TileType == tileType || DownTile.TileType == tileType || RightTile.TileType == tileType || LeftTile.TileType == tileType)
        {
            return true;
        }
        else { return false; }
    }

    public bool CheckAllSurroundingTileForSameType(Tile._TileType tileType)
    {
        if (AboveTile.TileType == tileType && DownTile.TileType == tileType && RightTile.TileType == tileType && LeftTile.TileType == tileType)
        {
            return true;
        }
        else { return false; }
    }
}
