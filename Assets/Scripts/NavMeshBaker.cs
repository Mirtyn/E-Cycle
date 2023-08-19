using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    private const string TILE_TAG = "Tile";
    private bool baked = false;
    private GameObject[] Tiles;

    private void Update()
    {
        if (!baked)
        {
            baked = true;
            Tiles = GameObject.FindGameObjectsWithTag(TILE_TAG);
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i].GetComponent<NavMeshSurface>().BuildNavMesh();
            }
        }
    }
}
