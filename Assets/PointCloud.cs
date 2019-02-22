using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PointCloud : MonoBehaviour
    {
        private Mesh _mesh;
        private UniformPointCloud _pointCloud;

        private void Start()
        {
            _mesh = new Mesh();
            _pointCloud = new UniformPointCloud(100, 100, 100);
            GetComponent<MeshFilter>().mesh = _mesh;

            var startTime = Time.realtimeSinceStartup;
            Add(new Vector3(50, 50, 50), 10);
            Render();
            var endTime = Time.realtimeSinceStartup - startTime;
            Debug.Log($"Execution time: {endTime:f6}s");
        }

        public void Set(Vector3 position, float radius)
        {
            for (var x = 0; x < _pointCloud.Width; x++)
            {
                for (var y = 0; y < _pointCloud.Height; y++)
                {
                    for (var z = 0; z < _pointCloud.Length; z++)
                    {
                        var inside = Mathf.Pow(position.x - x, 2) + Mathf.Pow(position.y - y, 2) + Mathf.Pow(position.z - z, 2) - Mathf.Pow(radius, 2) < 0;

                        _pointCloud.Set(x, y, z, inside);
                    }
                }
            }
        }

        public void Add(Vector3 position, float radius)
        {
            for (var x = 0; x < _pointCloud.Width; x++)
            {
                for (var y = 0; y < _pointCloud.Height; y++)
                {
                    for (var z = 0; z < _pointCloud.Length; z++)
                    {
                        if (_pointCloud.Get(x, y, z))
                            continue;

                        var inside = Mathf.Pow(position.x - x, 2) + Mathf.Pow(position.y - y, 2) + Mathf.Pow(position.z - z, 2) - Mathf.Pow(radius, 2) < 0;

                        _pointCloud.Set(x, y, z, inside);
                    }
                }
            }
        }

        public void Remove(Vector3 position, float radius)
        {
            for (var x = 0; x < _pointCloud.Width; x++)
            {
                for (var y = 0; y < _pointCloud.Height; y++)
                {
                    for (var z = 0; z < _pointCloud.Length; z++)
                    {
                        if (!_pointCloud.Get(x, y, z))
                            continue;

                        var inside = Mathf.Pow(position.x - x, 2) + Mathf.Pow(position.y - y, 2) + Mathf.Pow(position.z - z, 2) - Mathf.Pow(radius, 2) < 0;

                        if (inside)
                        {
                            _pointCloud.Set(x, y, z, false);
                        }
                            
                    }
                }
            }
        }

        private void Render()
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var colors = new List<Color>();
            var i = 0;
            for (var x = 0; x < _pointCloud.Width; x++)
            {
                for (var y = 0; y < _pointCloud.Height; y++)
                {
                    for (var z = 0; z < _pointCloud.Length; z++)
                    {
                        if (_pointCloud.Get(x, y, z))
                        {
                            vertices.Add(new Vector3(x, y, z));
                            indices.Add(i++);
                            colors.Add(Color.red );
                        }
                    }
                }
            }

            _mesh.vertices = vertices.ToArray();
            _mesh.colors = colors.ToArray();
            _mesh.SetIndices(indices.ToArray(), MeshTopology.Points, 0);
        }
    }
}