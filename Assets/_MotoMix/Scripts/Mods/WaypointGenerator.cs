using UnityEngine;
using System;
using System.Collections.Generic;

namespace ChemKart
{
    public class WaypointGenerator : MonoBehaviour
    {
        public GameObject tracks; // Parent object containing all track pieces
        public GameObject waypointPrefab; // Assign a prefab for waypoints

        [SerializeField] private int waypointSpacing = 5; // Adjust based on track size

        public static List<Waypoint> waypoints = new();

        public void GenerateWaypoints()
        {
            if (tracks == null)
            {
                Debug.LogError("No track assigned!");
                return;
            }

            foreach (Transform trackPiece in tracks.transform)
            {
                MeshFilter meshFilter = trackPiece.GetComponent<MeshFilter>();
                if (meshFilter == null) continue;

                Mesh mesh = meshFilter.sharedMesh;
                Vector3[] vertices = mesh.vertices;
                Vector3[] normals = mesh.normals; // Get mesh normals for correct up direction

                Vector3 worldVertex = trackPiece.TransformPoint(vertices[0]);
                int numWaypoints = vertices.Length / waypointSpacing;
                Vector3 leftEdge = worldVertex, rightEdge = worldVertex, backEdge = worldVertex, forwardEdge = worldVertex, upEdge = worldVertex, downEdge = worldVertex;

                // go throughs each vertex, grabs the left, right, forward, and backwards most vertexes to know the full dimensions of the model
                for (int i = 0; i < vertices.Length; i++)
                {
                    worldVertex = trackPiece.TransformPoint(vertices[i]); // Convert to world space

                    if (worldVertex.x < leftEdge.x)
                    {
                        leftEdge = worldVertex;
                    }
                    if (worldVertex.x > rightEdge.x)
                    {
                        rightEdge = worldVertex;
                    }
                    if (worldVertex.z > forwardEdge.z)
                    {
                        forwardEdge = worldVertex;
                    }
                    if (worldVertex.z < backEdge.z)
                    {
                        backEdge = worldVertex;
                    }
                    if(worldVertex.y > upEdge.y)
                    {
                        upEdge = worldVertex;
                    }
                    if(worldVertex.y < downEdge.y)
                    {
                        downEdge = worldVertex;
                    }
                }
                backEdge.x = leftEdge.x + rightEdge.x / 2;
                forwardEdge.x = leftEdge.x + rightEdge.x / 2;
                // now that we know the dimensions of the track piece, make waypoints progressing along the track piece
                for(int i = 0; i < numWaypoints; i++)
                {
                    float t = (float)i / (numWaypoints - 1);
                    Vector3 waypointPosition = Vector3.Lerp(backEdge, forwardEdge, t);
                    waypointPosition.y = (upEdge.y + downEdge.y) / 2;
                    waypointPosition.x = (leftEdge.x + rightEdge.x) / 2;

                    GameObject waypointObj = Instantiate(waypointPrefab, waypointPosition, Quaternion.identity);
                    Vector3 scale = waypointObj.transform.localScale;
                    scale.x = Mathf.Abs(leftEdge.x - rightEdge.x);
                    scale.y = Mathf.Abs(upEdge.y - downEdge.y);
                    waypointObj.transform.localScale = scale;
                    waypointObj.transform.SetParent(trackPiece);

                    waypoints.Add(waypointObj.GetComponent<Waypoint>());
                }
            }
            
            LinkWaypoints();
            waypoints[waypoints.Count / 2].isRequiredWaypoint = true;
            waypoints[0].gameObject.tag = "FinishLineWaypoint";
        }

        void LinkWaypoints()
        {
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Waypoint wp = waypoints[i];
                wp.nextWaypoint = waypoints[i + 1];
                wp.waypointIndex = i;
            }

            // Make it loop
            if(waypoints.Count > 0)
            {
                waypoints[waypoints.Count - 1].nextWaypoint = waypoints[0];
            }
        }
    }
}