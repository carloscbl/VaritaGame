using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;


class MeshComposer
{
    byte[] data;
    List<Vector3[]> vertexList      = new List<Vector3[]>();
    List<Vector3[]> normalsList     = new List<Vector3[]>();
    List<Vector3[]> uvsList         = new List<Vector3[]>();
    List<int[]>     trianglesList   = new List<int[]>();
    Hashtable hashTable = new Hashtable(255);
    List<List<int>> positionsInChunk = new List<List<int>>();
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
        }
    }


}
