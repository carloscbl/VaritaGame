using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


class TerrainGeneration : MonoBehaviour 
{
    private void Awake()
    {
        //halfRes = (int)resolution / 2;
        //octaveResPlusRes = halfRes + (resolution/8);
    }
    private void Start()
    {

    }
    //[Range(2, 512)]
    int resolution = 1024* incrementFactor;//(int)TerrainSystemNew.widthCubes;
    int halfRes = 512* incrementFactor;
    int octaveResPlusRes = (512* incrementFactor + 128* incrementFactor);
    static int incrementFactor = 2;
    public float frequency = 23.4f;

    [Range(1, 8)]
    public int octaves = 3;

    [Range(1f, 4f)]
    public float lacunarity = 1.65f;

    [Range(0f, 1f)]
    public float persistence = 0f;

    [Range(1, 3)]
    public int dimensions = 3;

    public NoiseMethodType type;// = NoiseMethodType.Perlin;

    public Gradient coloring;

    private Texture2D texture;
    private void elevateGround()
    {

    }
    float getMidPoint(int min, int max, float sample)
    {
        return ((max - min) * sample) + min;
    }

    public byte[,] GenTerrainNew()
    {

        float time = Time.realtimeSinceStartup;
        byte[,] data = new byte[2730, 2730];//2730

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
        NoiseMethod method2 = Noise.methods[(int)NoiseMethodType.Perlin][1-1];
        float stepSize = 1f / resolution;
        Hashtable hT = new Hashtable();
        float stepSizeX = 1f / 500;
        for (int y = 0; y < resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (int x = 0; x < resolution; x++)
            {
                if (y > halfRes)
                {
                    if (y > halfRes && y < octaveResPlusRes)
                    {
                        Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                        float sample = Noise.Sum(method2, point, 4, octaves, lacunarity, persistence);
                        //float sample = Noise.Sum(method2, point, 50, 0, 0, 0);
                        float maxAltitude = getMidPoint(halfRes+ (64 * incrementFactor), octaveResPlusRes, sample);
                        //Determinar la altura maxima y solo poner 1 si esta por debajo
                        /////Interpolar el resultado de la altura maxima con el sample
                        if (y < maxAltitude)
                        {
                            data[y, x] = 1;
                        }
                        else
                        {
                            data[y, x] = 0;
                        }
                    }else
                    {
                        data[y, x] = 0;
                    }

                }else
                {
                    Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                    float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);

                    if (type != NoiseMethodType.Value)
                    {
                        sample = sample * 0.5f + 0.5f;
                    }
                    if (/*sample >= 0.3f &&*/ sample <= 0.43f)
                    {
                        data[y, x] = 0;
                    }
                    else if (sample > 0.43f && sample < 0.48f)
                    {
                        data[y, x] = 2;
                    }
                    else
                    {
                        data[y, x] = 1;
                    }
                }

            }
        }
        print(Time.realtimeSinceStartup - time);
        return data;
    }

    private void OnEnable()
    {
        if (texture == null)
        {
            texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
        FillTexture();
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            FillTexture();
        }
    }

    public void FillTexture()
    {
        if (texture.width != resolution)
        {
            texture.Resize(resolution, resolution);
        }

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];

        NoiseMethod method2 = Noise.methods[(int)NoiseMethodType.Perlin][1 - 1];
        float stepSize = 1f / resolution;
        for (int y = 0; y < resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (int x = 0; x < resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = Noise.Sum(method2, point, frequency, octaves, lacunarity, persistence);
                //Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                //float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);

                if (type != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }
                texture.SetPixel(x, y, coloring.Evaluate(sample));
            }
        }
        texture.Apply();
    }
}

