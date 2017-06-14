using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;


class MeshComposer //: MonoBehaviour
{
    byte[] data;
    List<Vector3[]> vertexList      = new List<Vector3[]>();
    List<Vector3[]> normalsList     = new List<Vector3[]>();
    List<Vector3[]> uvsList         = new List<Vector3[]>();
    List<int[]>     trianglesList   = new List<int[]>();
    public List<int> materialsNumberList = new List<int>();
    List<List<int>> positionsInChunk = new List<List<int>>();
    public struct MeshComponents
    {
        public List<Vector3> vertex;
        public List<Vector3> normals;
        public List<Vector2> uvs;
        public List<List<int>> triangles;
        //public uint        materialNumber;
    }
    public List<int>[] MaterialAndPosition;
    public MeshComponents generatedMeshComponents;
    public MeshComposer(byte [] data)
    {
        this.data = data;
        
        MaterialAndPosition = new List<int>[255];
        List<int> Positions = new List<int>();
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] != 0 && data[i] != 4)
            {
                if (MaterialAndPosition[data[i]] == null)
                {
                    MaterialAndPosition[data[i]] = new List<int>();
                    MaterialAndPosition[data[i]].Add(i);
                    materialsNumberList.Add(data[i]);
                    Positions.Add(data[i]);
                }
                else
                {
                    MaterialAndPosition[data[i]].Add(i);
                }
            }
                
        }
        generatedMeshComponents = new MeshComponents();
        generatedMeshComponents.triangles = new List<List<int>>();
        generatedMeshComponents.vertex = new List<Vector3>();
        generatedMeshComponents.normals = new List<Vector3>();
        generatedMeshComponents.uvs = new List<Vector2>();
        for (int i = 0; i < Positions.Count; i++)
        {
            //if(Positions[i] !=5)
            generateMesh(MaterialAndPosition[Positions[i]]);
            //print(Positions[i]);
        }
    }
    private void generateMeshes(int i)
    {
        //We are doing the Merge per Material
        generateMesh(positionsInChunk[i]);
    }
    private MeshComponents generateMesh(List<int> listOfPositions)
    {
        //Generate cube
        /*
        List<Vector3> vertex = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        */
        List<int> triangles = new List<int>();
        Vector2 cubePos;

        for (int i = 0; i < listOfPositions.Count; i++)
        {
            float row = Mathf.Ceil(listOfPositions[i] / TerrainSystemNew.collSize);
            cubePos = new Vector2((listOfPositions[i] - (row * TerrainSystemNew.collSize)) * TerrainSystemNew.cubeSizeMultiplier,row * TerrainSystemNew.cubeSizeMultiplier);
            generatedMeshComponents.vertex.AddRange(getCubeVertex(cubePos));
            generatedMeshComponents.normals.AddRange(getCubeNormals());
            generatedMeshComponents.uvs.AddRange(getUvs());
            //triangles.AddRange(getTriangles((generatedMeshComponents.vertex.Count/24)-1));
            triangles.AddRange(getTriangles((generatedMeshComponents.vertex.Count/20)-1));
        }
        //generatedMeshComponents.vertex.AddRange(vertex);
        //generatedMeshComponents.normals.AddRange(normals);
        //generatedMeshComponents.uvs.AddRange(uvs);
        generatedMeshComponents.triangles.Add(triangles);
        return generatedMeshComponents;
    }
    private Vector3[] getCubeVertex(Vector2 cubePos)
    {
        #region Vertices
        float length = TerrainSystemNew.cubeSizeMultiplier*.5f;
        float width = TerrainSystemNew.cubeSizeMultiplier * .5f;
        float height = TerrainSystemNew.cubeSizeMultiplier * .5f;
        Vector3 tempVector;
        List<Vector3> vertexList = new List<Vector3>();

        tempVector = new Vector3(-length, -width, height);
        vertexList.Add(tempVector);
        tempVector = new Vector3(length, -width, height);
        vertexList.Add(tempVector);
        tempVector = new Vector3(length, -width, -height);
        vertexList.Add(tempVector);
        tempVector = new Vector3(-length, -width, -height);
        vertexList.Add(tempVector);

        tempVector = new Vector3(-length, width, height);
        vertexList.Add(tempVector);
        tempVector = new Vector3(length, width, height);
        vertexList.Add(tempVector);
        tempVector = new Vector3(length, width, -height);
        vertexList.Add(tempVector);
        tempVector = new Vector3(-length, width, -height);
        vertexList.Add(tempVector);

        for (int i = 0; i < vertexList.Count; i++)
        {
            vertexList[i] = new Vector3(vertexList[i].x + cubePos.x + length, vertexList[i].y + width + cubePos.y, vertexList[i].z);
            //print(vertexList[i]);
        }


        Vector3[] vertices = new Vector3[]
        {
	// Bottom
    vertexList[0],vertexList[1],vertexList[2],vertexList[3],

	// Left
    vertexList[7],vertexList[4],vertexList[0],vertexList[3],
 
	// Front
    //vertexList[4],vertexList[5],vertexList[1],vertexList[0],
 
	// Back
    vertexList[6],vertexList[7],vertexList[3],vertexList[2],
 
	// Right
    vertexList[5],vertexList[6],vertexList[2],vertexList[1],
 
	// Top
    vertexList[7],vertexList[6],vertexList[5],vertexList[4]

        };
        #endregion
        return vertices;

    }
    private Vector3[] getCubeNormals()
    {
        #region Normales
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	// Bottom
	down, down, down, down,
 
	// Left
	left, left, left, left,

    // Front
    //front, front, front, front,
 
	// Back
	back, back, back, back,
 
	// Right
	right, right, right, right,
 
	// Top
	up, up, up, up
        };
        #endregion
        return normales;
    }
    private Vector2[] getUvs()
    {
        #region UVs
        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);
      
        Vector2[] uvs = new Vector2[]
        {
	    // Bottom
	    _11, _01, _00, _10,
 
	    // Left
	    _11, _01, _00, _10,
 
	    // Front
	    //_11, _01, _00, _10,

	    // Back
	    _11, _01, _00, _10,

	    // Right
	    _11, _01, _00, _10,
 
	    // Top
	    _11, _01, _00, _10,
        };
        #endregion
        return uvs;
    }
    private int[] getTriangles(int cubeCount)
    {
        #region Triangles
        //cubeCount +=1;
        //cubeCount = (cubeCount * 24);
        cubeCount = (cubeCount * 20);

        int[] triangles = new int[]
        {
        
	// Bottom
	3+cubeCount, 1+cubeCount, 0+cubeCount,
    3+cubeCount, 2+cubeCount, 1+cubeCount,			
 
	// Left
	(7)+cubeCount, (5)+cubeCount, (4)+cubeCount,
    (7)+cubeCount, (6)+cubeCount, (5)+cubeCount,
 
	//// Front
	//(3 + 4 * 2)+cubeCount, (1 + 4 * 2)+cubeCount, (0 + 4 * 2)+cubeCount,
 //   (3 + 4 * 2)+cubeCount, (2 + 4 * 2)+cubeCount, (1 + 4 * 2)+cubeCount,
 /*
	// Back
	(15)+cubeCount, (13)+cubeCount, (12)+cubeCount,
    (15)+cubeCount, (14)+cubeCount, (13)+cubeCount,

	// Right
	(19)+cubeCount, (17)+cubeCount, (16)+cubeCount,
    (19)+cubeCount, (18)+cubeCount, (17)+cubeCount,
 
	// Top
	(23)+cubeCount, (21)+cubeCount, (20)+cubeCount,
    (23)+cubeCount, (22)+cubeCount, (21)+cubeCount,
    */
    (15-4)+cubeCount, (13-4)+cubeCount, (12-4)+cubeCount,
    (15-4)+cubeCount, (14-4)+cubeCount, (13-4)+cubeCount,

	// Right
	(19-4)+cubeCount, (17-4)+cubeCount, (16-4)+cubeCount,
    (19-4)+cubeCount, (18-4)+cubeCount, (17-4)+cubeCount,
 
	// Top
	(23-4)+cubeCount, (21-4)+cubeCount, (20-4)+cubeCount,
    (23-4)+cubeCount, (22-4)+cubeCount, (21-4)+cubeCount,
        };
        #endregion
        return triangles;
    }
    /*
    private int[] getTriangles(int cubeCount)
    {
        #region Triangles
        //cubeCount +=1;
        cubeCount = (cubeCount * 24);

        int[] triangles = new int[]
        {
        
	// Bottom
	3+cubeCount, 1+cubeCount, 0+cubeCount,
    3+cubeCount, 2+cubeCount, 1+cubeCount,			
 
	// Left
	(3 + 4 * 1)+cubeCount, (1 + 4 * 1)+cubeCount, (0 + 4 * 1)+cubeCount,
    (3 + 4 * 1)+cubeCount, (2 + 4 * 1)+cubeCount, (1 + 4 * 1)+cubeCount,
 
	//// Front
	//(3 + 4 * 2)+cubeCount, (1 + 4 * 2)+cubeCount, (0 + 4 * 2)+cubeCount,
 //   (3 + 4 * 2)+cubeCount, (2 + 4 * 2)+cubeCount, (1 + 4 * 2)+cubeCount,

	// Back
	(3 + 4 * 3)+cubeCount, (1 + 4 * 3)+cubeCount, (0 + 4 * 3)+cubeCount,
    (3 + 4 * 3)+cubeCount, (2 + 4 * 3)+cubeCount, (1 + 4 * 3)+cubeCount,

	// Right
	(3 + 4 * 4)+cubeCount, (1 + 4 * 4)+cubeCount, (0 + 4 * 4)+cubeCount,
    (3 + 4 * 4)+cubeCount, (2 + 4 * 4)+cubeCount, (1 + 4 * 4)+cubeCount,
 
	// Top
	(3 + 4 * 5)+cubeCount, (1 + 4 * 5)+cubeCount, (0 + 4 * 5)+cubeCount,
    (3 + 4 * 5)+cubeCount, (2 + 4 * 5)+cubeCount, (1 + 4 * 5)+cubeCount,
        };
        #endregion
        return triangles;
    }*/
}
