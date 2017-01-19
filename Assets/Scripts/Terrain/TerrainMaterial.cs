using System.Collections.Generic;
using UnityEngine;

public class TerrainMaterial
{
    public enum TypeTerrainUnit : byte
    {
        air, rock, sand, grass, water, bedrock
    }

    public struct TerrainUnitMaterial
    {
        public TypeTerrainUnit typeTerrain;
        public Material materialTerrain;

        public TerrainUnitMaterial(TypeTerrainUnit type, Material mat)
        {
            typeTerrain = type;
            materialTerrain = mat;
        }
    }

    private List<TerrainUnitMaterial> listTerrainUnitMaterials;
    private static TerrainMaterial instance;

    public static TerrainMaterial Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TerrainMaterial();
                instance.LoadTerrainMaterials();
            }
            return instance;
        }
    }

    public Material GetMaterialFromTypeTerrain(TypeTerrainUnit value)
    {
        return listTerrainUnitMaterials[(byte)value].materialTerrain;
    }

    public Material GetMaterial(int value)
    {
        return listTerrainUnitMaterials[value].materialTerrain;
    }

    public int GetMaterialPosition(Material mat)
    {
        foreach (TerrainUnitMaterial t in listTerrainUnitMaterials)
        {
            if (t.materialTerrain == mat)
            {
                return (int)t.typeTerrain;
            }
        }
        return -1;
    }

    public int TotalMaterials()
    {
        return listTerrainUnitMaterials.Count;
    }

    private void LoadTerrainMaterials()
    {
        listTerrainUnitMaterials = new List<TerrainUnitMaterial>();
        listTerrainUnitMaterials.Add(new TerrainUnitMaterial(TypeTerrainUnit.air, Resources.Load<Material>("air")));
        listTerrainUnitMaterials.Add(new TerrainUnitMaterial(TypeTerrainUnit.rock, Resources.Load<Material>("rock")));
        listTerrainUnitMaterials.Add(new TerrainUnitMaterial(TypeTerrainUnit.sand, Resources.Load<Material>("sand")));
        listTerrainUnitMaterials.Add(new TerrainUnitMaterial(TypeTerrainUnit.grass, Resources.Load<Material>("grass")));
        listTerrainUnitMaterials.Add(new TerrainUnitMaterial(TypeTerrainUnit.water, Resources.Load<Material>("water")));
        listTerrainUnitMaterials.Add(new TerrainUnitMaterial(TypeTerrainUnit.bedrock, Resources.Load<Material>("bedrock")));
    }
}
