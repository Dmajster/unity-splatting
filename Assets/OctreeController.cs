using Assets.Density_Field;
using Assets.Density_Function;
using UnityEngine;

namespace Assets
{
    public class OctreeController : MonoBehaviour
    {
        public Octree Octree;
        public int MaxLevel;

        public void Start()
        {
            Octree = new Octree();

            Octree.DepthFirst(
                ref Octree.Get(0),
                transform.position, 
                transform.localScale, 
                new DfSphere(
                    new Vector3(50,50,50),
                    25f
                ),
                MaxLevel
            );

            Debug.Log(Octree.Size);
        }

        public void OnDrawGizmosSelected()
        { 
            
            var root = Octree.Get(0);

            DrawOctree(ref root, transform.position, transform.localScale);
        }

        public void DrawOctree(ref Node node, Vector3 position, Vector3 size)
        {
            var count = node.GetChildrenCount();

            Gizmos.DrawWireCube(position + size / 2, size);

            for (var i = 0; i < count; i++)
            {
                ref var child = ref Octree.GetNChild(node, i);
                var childIndex = node.GetNChildIndex(i);
                var childPosition = Octree.NodeOffsets[childIndex];
                childPosition.Scale(size / 2);

                if (!child.Equals(node))
                {
                    DrawOctree(ref child, position + childPosition, size / 2);
                }
            }
        }
    }
}
