using UnityEngine;
using System.Collections.Generic;

public class ColliderCreator : MonoBehaviour
{
    private int currentPathIndex = 0;
    private PolygonCollider2D polygonCollider;
    private List<Edge> edges = new List<Edge>();
    private List<Vector2> points = new List<Vector2>();
    private Vector3[] vertices;
    private List<Vector3> VertexFinal = new List<Vector3>();

    void Start()
    {
        // Get the polygon collider (create one if necessary)
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        }

        // Get the mesh's vertices for use later
        vertices = GetComponent<MeshFilter>().mesh.vertices;
        int e = 0;
        while (e < vertices.Length)
        {
            foreach (Vector3 vertex in vertices){
                if (vertices[e].z != vertex.z)
                {
                    VertexFinal.Add(vertices[e]);
                }
            }
                e++; 
        }

        // Get all edges from triangles
        int[] triangles = GetComponent<MeshFilter>().mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            edges.Add(new Edge(triangles[i], triangles[i + 1]));
            edges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
            edges.Add(new Edge(triangles[i + 2], triangles[i]));
        }

        // Find duplicate edges
        List<Edge> edgesToRemove = new List<Edge>();
        foreach (Edge edge1 in edges)
        {
            foreach (Edge edge2 in edges)
            {
                if (edge1 != edge2)
                {
                    if (edge1.vert1 == edge2.vert1 && edge1.vert2 == edge2.vert2 || edge1.vert1 == edge2.vert2 && edge1.vert2 == edge2.vert1)
                    {
                        edgesToRemove.Add(edge1);
                    }
                }
            }
        }

        // Remove duplicate edges (leaving only perimeter edges)
        foreach (Edge edge in edgesToRemove)
        {
            edges.Remove(edge);
        }

        // Start edge trace
        edgeTrace(edges[0]);
    }
   // void create
    void edgeTrace(Edge edge)
    {
        // Add this edge's vert1 coords to the point list
        Vector3[] vertices2 = VertexFinal.ToArray();
        points.Add(vertices2[edge.vert1]);

        // Store this edge's vert2
        int vert2 = edge.vert2;

        // Remove this edge
        edges.Remove(edge);

        // Find next edge that contains vert2
        foreach (Edge nextEdge in edges)
        {
            if (nextEdge.vert1 == vert2)
            {
                edgeTrace(nextEdge);
                return;
            }
        }

        // No next edge found, create a path based on these points
        //points.
        polygonCollider.pathCount = currentPathIndex + 1;
        polygonCollider.SetPath(currentPathIndex, points.ToArray());

        // Empty path
        points.Clear();

        // Increment path index
        currentPathIndex++;

        // Start next edge trace if there are edges left
        if (edges.Count > 0)
        {
            edgeTrace(edges[0]);
        }
    }
}

class Edge
{
    public int vert1;
    public int vert2;

    public Edge(int Vert1, int Vert2)
    {
        vert1 = Vert1;
        vert2 = Vert2;
    }
}