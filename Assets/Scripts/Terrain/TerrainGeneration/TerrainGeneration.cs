using SimplexNoise;
using System.Collections.Generic;


class TerrainGeneration
{
    public List<ChunkData> GenerateTerrain(uint sizeX, uint sizeY, uint totalCubesPerChunk, float sizeCubeX, float sizeCubeY)
    {
        uint totalCubes = sizeX * sizeY;
        uint totalChunks = totalCubes / totalCubesPerChunk;
        List<ChunkData> listChunksData = new List<ChunkData>();
        for (ushort i = 0; i < sizeX / totalCubesPerChunk; i++)
        {
            for (ushort j = 0; j < sizeY / totalCubesPerChunk; j++)
            {
                listChunksData.Add(new ChunkData()
                {
                    localChunkId = (ushort)(i * totalCubesPerChunk + j),
                    active = false,
                    worldPos = new UnityEngine.Vector2(i * totalCubesPerChunk * sizeCubeX, j * totalCubesPerChunk * sizeCubeY),
                    cubes = new List<cube>(),
                    materials = new List<UnityEngine.Material>()
                });
            }
        }
        for (int px = 0; px < sizeX; px++)
        {
            for (int py = 0; py < sizeY; py++)
            {
                float randomValue = Noise.Generate(px, py);
                TerrainMaterial.TypeTerrainUnit typeTerrain;
                if (randomValue >= 0)
                {
                    typeTerrain = TerrainMaterial.TypeTerrainUnit.sand;
                }
                else
                {
                    typeTerrain = TerrainMaterial.TypeTerrainUnit.rock;
                }
                cube tempCube = new cube();
                tempCube.id = (uint)(px * sizeY + py);
                tempCube.inChunkPosition = (uint)((px / totalCubesPerChunk) * (sizeX / totalCubesPerChunk) + py / totalCubesPerChunk);
                tempCube.material = TerrainMaterial.Instance.GetMaterialFromTypeTerrain(typeTerrain);

                listChunksData[(int)tempCube.inChunkPosition].cubes.Add(tempCube);
                if (!listChunksData[(int)tempCube.inChunkPosition].materials.Exists(x => x == tempCube.material))
                {
                    listChunksData[(int)tempCube.inChunkPosition].materials.Add(tempCube.material);
                }
            }
        }
        return listChunksData;
    }
}

