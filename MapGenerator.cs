using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    [Header("Dimensions")]
    public int width;
    public int heigth;
    [Header("Seeds")]
    public string Seed;
    public bool useRandomSeed;

    [Header("Cave Settings")]
    public int borderSize = 5;

    //Slider in unity
    [Range(0, 100)]
    // Make it public to Unity
    public int RandomFillPercent;

    int[,] map;

    private void Start()
    {
        GenerateMap();
    }
    private float lastGenerateTime;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }
    void GenerateMap()
    {
        map = new int[width, heigth];
        RandomFillMap();

        for(int i = 0; i < 5; i++)
        {
            SmoothMap();
        }


        int[,] borderedMap = new int[width + borderSize * 2, heigth + borderSize * 2];

        for(int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for(int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if(x >= borderSize && x < width + borderSize && y >= borderSize && y < heigth + borderSize)
                {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                }
                else
                {
                    borderedMap[x, y] = 1;
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);
    }

    void RandomFillMap()
    {

        if (useRandomSeed)
        {
            Seed = Time.time.ToString();
        }
        System.Random rand = new System.Random(Seed.GetHashCode());
        
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < heigth; y++)
            {
                if(x == 0 || x == width-1 || y == 0 || y == heigth - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (rand.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
                
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if(neighbourWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if(neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }


            }
        }

    }
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++ )
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if(neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < heigth)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
                
            }
        }
        return wallCount;
    }
    void OnDrawGizmos()
    {/*
        if(map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -heigth / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
        */
    }


}
