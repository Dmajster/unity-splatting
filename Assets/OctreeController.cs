using UnityEngine;

namespace Assets
{
    public class OctreeController : MonoBehaviour
    {
        public Octree Octree;

        public void Start()
        {
            Octree = new Octree();
            ref var root = ref Octree.Get(0);
            root.SetChildMask(0, true);
            root.SetChildMask(4, true);
            root.SetChildMask(7, true);
            Octree.AddChildren(ref root);

            ref var child1 = ref Octree.GetNChild(root, 1);
            child1.SetChildMask(1, true);
            child1.SetChildMask(3, true);
            child1.SetChildMask(5, true);
            Octree.AddChildren(ref child1);
        }

        public void OnDrawGizmosSelected()
        {
            
            var root = Octree.Get(0);

            DrawOctree(root, transform.position, transform.localScale);
        }

        public void DrawOctree(Node node, Vector3 position, Vector3 size)
        {
            var count = node.GetChildrenCount();

            Gizmos.DrawWireCube(position + size / 2, size);

            for (var i = 0; i < count; i++)
            {
                var child = Octree.GetNChild(node, i);
                var childIndex = node.GetNChildIndex(i);
                var childPosition = Octree.NodeOffsets[childIndex];
                childPosition.Scale(size / 2);
                DrawOctree(child, position + childPosition, size/2);

            }
        }
    }
}
