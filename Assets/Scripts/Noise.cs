using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    /*
     *      Perlin Noise Generation
     *      
     *      Perlin Noise Generates a 2D height map, these height map can be thought of as a wave form.
     *      For more complex height map, we will multiply multiple height map together.
     *      These height map are referred as octaves.
     *      We want each octaves to increase in details, so we introduce a variable called Lacunarity.
     *      Lacunarity controls the increase in frequency of octaves.
     *      The frequency of each octaves will increase in power as more octaves are introduced, with input more than 0.
     *      Using this formular here.
     *      Octave's Frequency = Lacunarity ^ (Octave Number - 1)
     *      Each increasing octave should have lesser effect on the overall generated perlin noise.
     *      So we use persistance to control amplitude of octaves, with input from 0 to 1.
     *      Using this formular here.
     *      Octave's amplitude = persistance ^ (Octave Number - 1)
     *      lacunarity affects the number of small details on the map
     *      persistance affect how much those small details affect the surrounding parts of the map
     */

    /*
     *      Input: 
     *      map width = width of map, 
     *      map height = height of map, 
     *      scale = interger sampling point will give same result, so sampling point is divided using scale
     *      
     *      Output: height map array of value between 0 and 1.
     *      
     */
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //handle divide by zero error
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        //loop through noise map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //sampling points for perlin noise
                //avoid interger value so divide it using scale
                float sampleX = x / scale;
                float sampleY = y / scale;

                //calculate the perlin noise for current point
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                //apply calculated perlin noise value to noise map
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
