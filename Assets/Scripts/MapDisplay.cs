using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void Awake()
    {
        textureRenderer = GetComponent<Renderer>();
    }

    public void DrawNoiseMap(float[,] noiseMap)
    {
        //get width and height of noisemap
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        //create a new 2D texture
        Texture2D texture = new Texture2D(width, height);

        //calculate color for each point in noise map
        Color[] colorMap = new Color[width * height];

        //loop through color map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //color changes from black to white based on how high the value of the noise map point
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        //apply color map to texture all at once for faster performance
        texture.SetPixels(colorMap);
        texture.Apply();

        //apply texture to texture renderer, use shared materials so that we can generate it in the editor too.
        textureRenderer.sharedMaterial.mainTexture = texture;

        //set size of plane to same size of the map
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
