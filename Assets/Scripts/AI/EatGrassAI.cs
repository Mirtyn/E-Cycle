using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Linq;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

internal class EatGrassAI : MapSettings
{
    // -1 = every layer
    [SerializeField] int LayerMaskInteracteable = -1;

    private float radius;
    [SerializeField] private float minRadius = 1;
    [SerializeField] private float maxRadius = 10;

    private int walkAttempt = 0;

    NavMeshAgent agent;

    bool goingForFood = false;

    Tile grassTile = new Tile();

    // Use this for initialization
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public bool Eat()
    {
        Debug.DrawLine(gameObject.transform.position, agent.destination, Color.green);

        //IEnumerable<Tile> grassTiles = Tiles.Where(t => t.TileType == Tile._TileType.Grass);
        //Tile closest = grassTiles.OrderBy(item => new Vector2(this.transform.localPosition.x, this.transform.localPosition.z) - new Vector2(item.XPos, item.YPos)).First();

        //NavMeshHit navHit;

        //NavMesh.SamplePosition(new Vector3(closest.XPos, 0, closest.YPos), out navHit, float.MaxValue, LayerMaskInteracteable);

        //Vector3 newPos = navHit.position;

        //agent.SetDestination(newPos);

        IEnumerable<Tile> grassTiles = Tiles.Where(t => t.TileType == Tile._TileType.Grass);
        //Tile closest = grassTiles.OrderBy(item => Math.Abs((this.transform.localPosition.x - item.XPos) + (this.transform.localPosition.z - item.YPos))).First();

        if (Eat2(grassTiles))
        {
            return true;
        }

        return false;


        //Tile desiredTile = Tiles
        //    .Select(t => new
        //    {
        //        Rating =
        //            Convert.ToInt32(t.XPos > this.transform.localPosition.x - 5 && t.XPos < this.transform.localPosition.x + 5) +       // Here you check first criteria
        //            Convert.ToInt32(t.YPos > this.transform.localPosition.z - 5 && t.YPos < this.transform.localPosition.z + 5) +     // Check second
        //            Convert.ToInt32(t.TileType == Tile._TileType.Grass),   // And the third one
        //        t
        //    })
        //    .OrderByDescending(obj => obj.Rating) // Here you order them by number of matching criteria
        //    .Select(obj => obj.t) // Then you select only cats from your custom object
        //    .First(); // And get the first of them



        //if (timer >= wanderTimer)
        //{
        //    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, LayerMaskInteracteable);
        //    agent.SetDestination(newPos);
        //    timer = 0;
        //    wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
        //    wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);
        //}
    }

    private bool Eat2(IEnumerable<Tile> grassTiles)
    {
        if (!goingForFood)
        {
            radius = Random.Range(minRadius, maxRadius);

            Vector3 newPos = RandomNavSphere(transform.position, radius, LayerMaskInteracteable);
            walkAttempt++;

            if (walkAttempt > 10)
            {
                maxRadius++;
                walkAttempt = 0;
            }

            try
            {
                Tile _grassTile = grassTiles.Single(t => (int)t.XPos == (int)newPos.x && (int)t.YPos == (int)newPos.z);
                grassTile = _grassTile;
            }
            catch (System.InvalidOperationException)
            {
                Eat2(grassTiles);
                grassTile = new Tile();
                return false;
            }



            goingForFood = true;
            maxRadius = 10;

            agent.SetDestination(newPos);
            return false;
        }
        else
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
            {
                walkAttempt = 0;
                goingForFood = false;

                grassTile.ChangeTileType(Tile._TileType.Dirt);

                return true;
            }
        }
        return false;
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector2 randDirection = Random.insideUnitCircle * dist;

        Vector3 r = origin + new Vector3(randDirection.x, 0, randDirection.y);

        NavMeshHit navHit;

        NavMesh.SamplePosition(r, out navHit, dist, layermask);

        return navHit.position;
    }
}
