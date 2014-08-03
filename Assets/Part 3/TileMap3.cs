﻿using UnityEngine;
using System.Collections;

// https://www.youtube.com/watch?v=ou3uwaJGJUc
// Unity 3d: TileMaps - Part 3 - Generating a Mesh + Texturing

// NOTES
// - Monodevelop: When your cursor is on a variable, if you press F2, you can refactor variable name.
// - [ExecuteInEditMode] makes the script run while in the editor, so you can see the tilemap.

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap3 : MonoBehaviour {

	public int 
		size_x = 100,  // Number of tiles on the x axis
		size_z = 50;
	public float
		tile_size = 1.0f; // Size in unity units

	// Use this for initialization
	void Start () {
		BuildMesh();
	}
	
	public void BuildMesh(){
		/* Map
		 * 0----1----2----3
		 * |\   |\   |\   |
		 * | \  | \  | \  |
		 * |  \ |  \ |  \ |
		 * |   \|   \|   \|
		 * 4----5----6----7
		 * |\   |\   |\   |
		 * | \  | \  | \  |
		 * |  \ |  \ |  \ |
		 * |   \|   \|   \|
		 * 8----9---10---11
		 */
		int num_tiles = size_x * size_z;
		int num_triangles = num_tiles * 2;

		int vsize_x = size_x + 1; // Number of horizontal vertices. To draw one tile wide, you need two vertices - so we add one.
		int vsize_z = size_z + 1;
		int num_vertices = vsize_x * vsize_z;

		// Generate the mesh data
		Vector3[] vertices = new Vector3[num_vertices];
		Vector3[] normals = new Vector3[num_vertices];   // one per vertex
		Vector2[] uv = new Vector2[num_vertices];        // one per vertex; value from 0 to 1. Represents where in texture to apply (just one texture for entire map right now). 0 = left most, 1 = right most.

		int[] triangles = new int[num_triangles * 3];     // 2 triangles * 3		 vertices

		// Populate vertices, normals, and uv
		int x, z, index;
		for (z = 0; z < vsize_z; z++) {
			for (x = 0; x < vsize_x; x++) {
				index = z * vsize_x + x;
				vertices[index] = new Vector3( x * tile_size, 0, z * tile_size);
				normals[index] = Vector3.up;
				uv[index] = new Vector2((float)x / vsize_x, (float)z / vsize_z);
			}
		}

		// Populate triangles
		for (z = 0; z < size_z; z++) {
			for (x = 0; x < size_x; x++) {
				/* First tile
				 * -----------------------------
				 * First triangle
				 * triangles[0] = 0;
				 * triangles[1] = vsize_x + 0;
				 * triangles[2] = vsize_x + 1;
				 * 
				 * Second triangle
				 * triangles[3] = 0;
				 * triangles[4] = vsize_x + 1;
				 * triangles[5] = 1;
				 */
				int square_index = z * size_x + x;
				int triangle_offset = square_index * 6;

				// First triangle
				triangles[triangle_offset + 0] = z * vsize_x + x + 0;
				triangles[triangle_offset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triangle_offset + 2] = z * vsize_x + x + vsize_x + 1;

				// Second triangle
				triangles[triangle_offset + 3] = z * vsize_x + x + 0;
				triangles[triangle_offset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triangle_offset + 5] = z * vsize_x + x + 1;
			}
		}

		// Create a new mesh and populate with the data
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		//Assign our mess to filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter> ();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		MeshCollider mesh_collider = GetComponent<MeshCollider> ();
		
		mesh_filter.mesh = mesh;
	}
}
