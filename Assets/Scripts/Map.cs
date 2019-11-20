using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int[,] map;
    [SerializeField] private int Nsize = 100;
    [SerializeField] private int Msize = 100;
    [SerializeField] private int tileSize = 1;
    void Start()
    {
        map = new int[Nsize, Msize];
        
        for(int y = 0; y < Msize; y++)
        {
            for(int step = 0; step < Nsize/3; step++)
            {
                int x = Random.Range(0, Nsize);
            }
            
         
        }
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        GameObject waterTile = (GameObject)Instantiate(Resources.Load("WaterTile"));
        GameObject lotusTile = (GameObject)Instantiate(Resources.Load("LotusTile"));
        for (int y = 0; y < Msize; y++)
        {
            for (int x = 0; x < Nsize; x++)
            {
                GameObject tile = Instantiate(waterTile, transform);
                float posX = x * tileSize;
                float posY = y * -tileSize;
                tile.transform.position = new Vector2(posX, posY);
            }
            if (eventChance(70))
            {
                int x = 0;
                int offSetStep = Nsize / 4;
                while (x < Nsize)
                {
                    x = Random.Range(x+2, x + offSetStep);
                    if (x >= Nsize) break;
                    GameObject tile = Instantiate(lotusTile, transform);
                    float posX = x *  tileSize;
                    float posY = y * -tileSize;
                    tile.transform.position = new Vector2(posX, posY);
                    tile.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 359));
                }
            }

        }

        float gridW = Nsize * tileSize;
        float gridH = Msize * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
        Destroy(waterTile);
        Destroy(lotusTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool eventChance(float percent)
    {
        if (percent >= 100f) return true;
        float chance = Random.Range(0f, 100f);
        return (chance <= percent) ? true : false;
    }
}
