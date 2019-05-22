using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public bool autoUpdate;

    private MapDisplay display;

    public void Awake()
    {
        display = GetComponent<MapDisplay>();
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        display = GetComponent<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
