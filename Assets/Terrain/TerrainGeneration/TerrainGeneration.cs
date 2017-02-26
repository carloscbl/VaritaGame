using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


class TerrainGeneration : MonoBehaviour 
{
    private void Start()
    {

    }
    //[Range(2, 512)]
    public int resolution = (int)TerrainSystemNew.widthCubes;
    public int resolutionY = (int)TerrainSystemNew.heightCubes;

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
    //public byte [,] GenTerrainNew()
    //{
    //    byte[,] data = new byte[resolution, resolution];

    //    NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
    //    //float stepSize = 1f / resolution;

    //    for (int y = 0; y < resolution; y++)
    //    {
    //        //Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
    //        //Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
    //        for (int x = 0; x < resolution; x++)
    //        {
    //            //Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
    //            float sample = Noise.Sum(method, new Vector3(x,y,0), frequency, octaves, lacunarity, persistence);

    //            if (type != NoiseMethodType.Value)
    //            {
    //                sample = sample * 0.5f + 0.5f;
    //            }
    //            if (sample >= 0.3f && sample <= 0.4f)
    //            {
    //                data[y, x] = 0;
    //            }else
    //            {
    //                data[y, x] = 1;
    //            }
    //            //data[x,y] =
    //            //texture.SetPixel(x, y, coloring.Evaluate(sample));
    //        }
    //    }
    //    return data;
    //}

    public byte[,] GenTerrainNew()
    {
        byte[,] data = new byte[2730, 2730];

        

        //Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        //Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        //Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        //Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
        float stepSize = 1f / resolution;
        for (int y = 0; y < resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (int x = 0; x < resolution; x++)
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
                else
                {
                    data[y, x] = 1;
                }
                //texture.SetPixel(x, y, coloring.Evaluate(sample));
            }
        }
        //texture.Apply();
        //int count = 0;
        //for (int i = 0; i < 2730; i++)
        //{
        //    for (int e = 0; e < 500; e++)
        //    {
        //        if (count == 521 || count == 20 || count == 54600 || count == 108990 || count == 165360)
        //        {
        //            data[i, e] = 2;
        //        }else
        //        {
        //            data[i, e] = 1;
        //        }
        //        count++;
        //    }
        //}
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
        float stepSize = 1f / resolution;
        for (int y = 0; y < resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (int x = 0; x < resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);

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

