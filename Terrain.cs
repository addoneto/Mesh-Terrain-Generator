using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
class Terrain : MonoBehaviour{
    
    public int TerrainSizeX = 50;
    public int TerrainSizeZ = 50;
    public int VertexPerUnit = 1;

    [Space(10)]

    public float PerlinNoiseAmplitude = 2f;
    public float PerlinNoiseFrequency = 0.3f;

    private Vector3[] vertices;
    private int[] triangles;

    private Mesh terrainMesh;

    private void OnValidate(){
        GenerateMesh();
        ApplyMesh();
    }

    private void GenerateMesh(){

        // Generate Vertices

        vertices = new Vector3[(TerrainSizeX + 1) * (TerrainSizeZ + 1)];

        for(int z = 0; z <= TerrainSizeZ; z++){
            for(int x = 0; x <= TerrainSizeX; x++){

                int index = z * (TerrainSizeX + 1) + x;

                float y = Mathf.PerlinNoise(x * PerlinNoiseFrequency, z * PerlinNoiseFrequency) * PerlinNoiseAmplitude;

                vertices[index] = new Vector3((float)x / (float)VertexPerUnit, y, (float)z / (float)VertexPerUnit);
            }
        }

        // Generate Triangles

        //Square base mesh
        // triangles = new int[6];
        // triangles[0] = 0;
        // triangles[1] = TerrainSizeX + 1;
        // triangles[2] = 1;
        // triangles[3] = 1;
        // triangles[4] = TerrainSizeX + 1;
        // triangles[5] = TerrainSizeX + 2;

        // Each square has 2 triangles, each triangle 3 vertices indexes
        triangles = new int[TerrainSizeX * TerrainSizeZ * 6];

        int iOffset = 0, vertOffset = 0;

        for(int z = 0; z < TerrainSizeZ; z++){
            for(int x = 0; x < TerrainSizeX; x++){

                triangles[iOffset + 0] = vertOffset + 0;
                triangles[iOffset + 1] = vertOffset + TerrainSizeX + 1;
                triangles[iOffset + 2] = vertOffset + 1;

                triangles[iOffset + 3] = vertOffset + 1;
                triangles[iOffset + 4] = vertOffset + TerrainSizeX + 1;
                triangles[iOffset + 5] = vertOffset + TerrainSizeX + 2;

                vertOffset++;
                iOffset += 6;
            }
            vertOffset++;
        }

    }

    private void ApplyMesh(){
        if(terrainMesh == null){
            terrainMesh = new Mesh(); 
            this.gameObject.GetComponent<MeshFilter>().mesh = terrainMesh;
        }

        terrainMesh.Clear();

        terrainMesh.vertices = vertices;
        terrainMesh.triangles = triangles;

        terrainMesh.RecalculateNormals();
    }

    private void OnDrawGizmos(){
        for(int i = 0; i < vertices.Length; i++){
            Gizmos.DrawSphere(vertices[i], 0.05f);
        }
    }

}
