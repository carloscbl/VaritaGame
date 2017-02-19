using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


class TerrainGeneration : MonoBehaviour 
{
    static int Noise(int x, int y, float scale, float mag, float exp)
    {

        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));

    }
   
    static public byte [,] TrueGenTerrain()
    {
        byte[,] blocks = new byte[5460, 1000];
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            int stone = Noise(px, 0, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 280 * 1000 / 400;

            int dirt = Noise(px, 0, 100f, 35, 1);
            dirt += Noise(px, 100, 50, 30, 1);
            dirt += 300*1000/400;

            

            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py < stone)
                {
                    blocks[px, py] = 1;

                    //The next three lines make dirt spots in random places
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 2;

                    }
                    //The next three lines remove dirt and rock to make caves in certain places
                    if (Noise(px, py * 2, 16, 14, 1) > 10)
                    { //Caves
                        blocks[px, py] = 0;

                    }


                }
                else if (py < dirt)
                {
                    blocks[px, py] = 2;
                }
                blocks[px, py] = 2;
            }
        }
        /*for(int i = 0;i< blocks.GetLength(0);i++)
        {
            for (int e = 0; e < blocks.GetLength(1); e++)
            {
                if (blocks[i, e] != 1 || blocks[i, e] != 2 || blocks[i, e] != 3)
                {
                    blocks[i, e] = 0;
                }
            }
        }*/
        return blocks;
    }
}

