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
     *      
     *      Map Width
     *      It's the width of the map.
     *      
     *      Map Height
     *      It's the height of the map;
     *      
     *      Scale
     *      Having the scale in interger would cause the terrain to be the same, so only floats are allowed
     *      To achieve that we divide it by scale.
     *      
     *      Octaves.
     *      They are essentially height maps or waveforms. 
     *      Multiple height maps are stack together for a more complex terrain. 
     *      The more the octaves the more stages of increasingly finer details there are in a terrain.
     *      Eg. Mountain, Boulders, Rocks, Stones, Pebbles, etc.
     *      
     *      Lacunarity
     *      It controls the increase in frequency of octaves. 
     *      As lacunarity increases, the frequency of each octaves gets affected in increasing intensity 
     *      and thus having increasing details
     *      [Range (1, n)]
     *      
     *      Persistance
     *      It controls the decrease in amplitude of octaves
     *      As Persistance increases, the amplitude of each octaves get affected in diminishing intensity.
     *      The smaller the rock the less effect it has on the outline of the mountain
     *      [Range (0, 1)]
     *      
     *      Output: 
     *      height map array of value between 0 and 1.
     *      
     */
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random pseudoRandomNumberGenerator = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];

        //calculating octave offsets using pseudo random number generation to get the same thing each time the same seed is used
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = pseudoRandomNumberGenerator.Next(-100000, 100000) + offset.x;
            float offsetY = pseudoRandomNumberGenerator.Next(-100000, 100000) + offset.y ;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //keep track of minimum and maximum noise height
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfwidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //loop through noise map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //amplitude, frequency and noise height of the current point of the noise map
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //calculate the noise height for each octaves
                for (int i = 0; i < octaves; i++)
                {
                    //sampling points for perlin noise
                    //avoid interger value so divide it using scale
                    //frequency affects the closeness of each point on the noise map
                    //adding offset to keep the value the same when the same seed is used
                    float sampleX = (x - halfwidth) / (scale * frequency) + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / (scale * frequency) + octaveOffsets[i].y;

                    //calculate the perlin noise for current point
                    //perlin noise returns a value from 0 to 1, 
                    //so multiplying by 2 and minusing 1 gives it a range of -1 to 1
                    float perlinValue = (Mathf.PerlinNoise(sampleX, sampleY) * 2) - 1;

                    //perlin value is affected by amplitude, 
                    //result is added to the sum of all previous noise height value
                    noiseHeight += perlinValue * amplitude;

                    //with each increasing octaves, the amplitude is affected by the persistance of the noise map
                    amplitude *= persistance;
                    //same with lacunarity and frequency
                    frequency *= lacunarity;
                }

                //keep track of minimum and maximum noise height
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                //apply calculated noise height value to noise map
                noiseMap[x, y] = noiseHeight;
            }
        }

        //normalize noise map to return its range back to 0 to 1
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
