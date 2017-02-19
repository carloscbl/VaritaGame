using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;


class MeshComposer : MonoBehaviour
{
    byte[] data;
    List<Vector3[]> vertexList      = new List<Vector3[]>();
    List<Vector3[]> normalsList     = new List<Vector3[]>();
    List<Vector3[]> uvsList         = new List<Vector3[]>();
    List<int[]>     trianglesList   = new List<int[]>();
    Hashtable hashTable = new Hashtable(255);
    List<List<int>> positionsInChunk = new List<List<int>>();
    public struct MeshComponents
    {
        public Vector3[]   vertex;
        public Vector3[]   normals;
        public Vector2[]   uvs;
        public int[]       triangles;
    }
    public MeshComponents generatedMeshComponents;
    public MeshComposer(byte [] data)
    {
        this.data = data;

        for (int i = 0; i < data.Length; i++)
        {
            if (hashTable.ContainsKey(data[i]))
            {
               List<int> temp = hashTable[data[i]] as List<int>;
                temp.Add(i);
            }else
            {
                List<int> temp = new List<int>();
                temp.Add(i);
                positionsInChunk.Add(temp);
                hashTable.Add(data[i], temp);
                
            }
            //Probar que todo funciona correcto :) <-----
        }/*
        print(positionsInChunk.Count);
        print(positionsInChunk[0].Count);
        print(positionsInChunk[1].Count);
        print(positionsInChunk[2].Count);
        */
        generateMeshes();
    }
    private void generateMeshes()
    {
        //We are doing the Merge per Material
        for (int i = 0; i < positionsInChunk.Count; i++)
        {
            generateMesh(positionsInChunk[i]);
        }
    }
    private void generateMesh(List<int> listOfPositions)
    {
        //Generate cube
        List<Vector3> vertex = new List<Vector3>();
        //print("--->"+listOfPositions.Count);
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        Vector2 cubePos;
        //print(listOfPositions.Count);
        
        for (int i = 0; i < listOfPositions.Count; i++)
        {
            float row = Mathf.Ceil(listOfPositions[i] / TerrainSystemNew.collSize);
            cubePos = new Vector2((listOfPositions[i] - (row * TerrainSystemNew.collSize)) * TerrainSystemNew.cubeSizeMultiplier,row * TerrainSystemNew.cubeSizeMultiplier);
            //print(row);
            vertex.AddRange(getCubeVertex(cubePos));
            normals.AddRange(getCubeNormals());
            uvs.AddRange(getUvs());
            triangles.AddRange(getTriangles(i));
            //print(i);
            //print("--->Lenght" + vertex.Count);
            
        }
        generatedMeshComponents = new MeshComponents();
        generatedMeshComponents.vertex = vertex.ToArray();
        generatedMeshComponents.normals = normals.ToArray();
        generatedMeshComponents.uvs = uvs.ToArray();
        generatedMeshComponents.triangles = triangles.ToArray();

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
            vertexList[i] = new Vector3(vertexList[i].x + cubePos.x, vertexList[i].y +cubePos.y, vertexList[i].z);
            //print(vertexList[i]);
        }


        Vector3[] vertices = new Vector3[]
        {
	// Bottom
    vertexList[0],vertexList[1],vertexList[2],vertexList[3],

	// Left
    vertexList[7],vertexList[4],vertexList[0],vertexList[3],
 
	// Front
    vertexList[4],vertexList[5],vertexList[1],vertexList[0],
 
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
	front, front, front, front,
 
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
	    _11, _01, _00, _10,
 
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
        cubeCount = cubeCount * 24;
        int[] triangles = new int[]
        {
        
	// Bottom
	3+cubeCount, 1+cubeCount, 0+cubeCount,
    3+cubeCount, 2+cubeCount, 1+cubeCount,			
 
	// Left
	(3 + 4 * 1)+cubeCount, (1 + 4 * 1)+cubeCount, (0 + 4 * 1)+cubeCount,
    (3 + 4 * 1)+cubeCount, (2 + 4 * 1)+cubeCount, (1 + 4 * 1)+cubeCount,
 
	// Front
	(3 + 4 * 2)+cubeCount, (1 + 4 * 2)+cubeCount, (0 + 4 * 2)+cubeCount,
    (3 + 4 * 2)+cubeCount, (2 + 4 * 2)+cubeCount, (1 + 4 * 2)+cubeCount,
 
	// Back
	(3 + 4 * 3)+cubeCount, (1 + 4 * 3)+cubeCount, (0 + 4 * 3)+cubeCount,
    (3 + 4 * 3)+cubeCount, (2 + 4 * 3)+cubeCount, (1 + 4 * 3)+cubeCount,
 
	// Right
	(3 + 4 * 4)+cubeCount, (1 + 4 * 4)+cubeCount, (0 + 4 * 4)+cubeCount,
    (3 + 4 * 4)+cubeCount, (2 + 4 * 4)+cubeCount, (1 + 4 * 4)+cubeCount,
 
	// Top
	(3 + 4 * 5)+cubeCount, (1 + 4 * 5)+cubeCount, (0 + 4 * 5)+cubeCount,
    (3 + 4 * 5)+cubeCount, (2 + 4 * 5)+cubeCount, (1 + 4 * 5)+cubeCount,

    /*
    // Bottom
	((3+1)*cubeCount)-1,(( 1+1)*cubeCount)-1,(( 0+1)*cubeCount)-1,
    ((3+1)*cubeCount)-1,(( 2+1)*cubeCount)-1,(( 1+1)*cubeCount)-1,		
 
	// Left
	((3 + 4 * 1+1)*cubeCount)-1,(( 1 + 4 * 1+1)*cubeCount)-1,(( 0 + 4 * 1+1)*cubeCount)-1,
    ((3 + 4 * 1+1)*cubeCount)-1,(( 2 + 4 * 1+1)*cubeCount)-1,(( 1 + 4 * 1+1)*cubeCount)-1,
 
	// Front
	((3 + 4 * 2+1)*cubeCount)-1,(( 1 + 4 * 2+1)*cubeCount)-1,(( 0 + 4 * 2+1)*cubeCount)-1,
    ((3 + 4 * 2+1)*cubeCount)-1,(( 2 + 4 * 2+1)*cubeCount)-1,(( 1 + 4 * 2+1)*cubeCount)-1,
 
	// Back
	((3 + 4 * 3+1)*cubeCount)-1,(( 1 + 4 * 3+1)*cubeCount)-1,(( 0 + 4 * 3+1)*cubeCount)-1,
    ((3 + 4 * 3+1)*cubeCount)-1,(( 2 + 4 * 3+1)*cubeCount)-1,(( 1 + 4 * 3+1)*cubeCount)-1,
 
	// Right
	((3 + 4 * 4+1)*cubeCount)-1,(( 1 + 4 * 4+1)*cubeCount)-1,(( 0 + 4 * 4+1)*cubeCount)-1,
    ((3 + 4 * 4+1)*cubeCount)-1,(( 2 + 4 * 4+1)*cubeCount)-1,(( 1 + 4 * 4+1)*cubeCount)-1,
 
	// Top
	((3 + 4 * 5+1)*cubeCount)-1,(( 1 + 4 * 5+1)*cubeCount)-1,(( 0 + 4 * 5+1)*cubeCount)-1,
    ((3 + 4 * 5+1)*cubeCount)-1,(( 2 + 4 * 5+1)*cubeCount)-1,(( 1 + 4 * 5+1)*cubeCount)-1,

    /*
    // Bottom
	3, 1, 0,
    3, 2, 1,			
 
	// Left
	3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
    3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
	// Front
	3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
    3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
 
	// Back
	3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
    3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
	// Right
	3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
    3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
 
	// Top
	3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
    3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
    */
        };
        #endregion
        return triangles;
    }
}
