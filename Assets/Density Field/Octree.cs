using System;
using Assets.Density_Function;
using UnityEngine;

namespace Assets.Density_Field
{
    public struct Node
    {
        public byte Children;
        public int FirstChild { get; set; }

        public bool GetChildMask(int n)
        {
            return (Children >> n & 1) > 0;
        }

        public void SetChildMask(int n, bool value)
        {
            if (value)
            {
                Children |= (byte)(1 << n);
            }
            else
            {
                Children &= (byte)(~(1 << n));
            }
        }

        public int GetChildrenCount()
        {
            var counter = 0;
            for (var i = 0; i < 8; i++)
            {
                counter += Children >> i & 1;
            }

            return counter;
        }

        public int GetNChildIndex(int n)
        {
            for (var i = 0; i < 8; i++)
            {
                if (GetChildMask(i))
                {
                    if (n == 0)
                    {
                        return i;
                    }

                    n--;
                }
            }

            return -1;
        }
    }

    public class Octree
    {
        public static Vector3[] NodeOffsets = new []
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1),
        };

        public int Size => _data.Length;

        private Node[] _data;
        private int _size = 128;
        private int _next = 1;

        public Octree()
        {
            _data = new Node[_size];
        }

        public ref Node Get(int index)
        {
            return ref _data[index];
        }

        public int FindSpace()
        {
            if (_next == _size)
            {
                Resize();
            }

            return _next;
        }

        public void Resize()
        {
            Debug.Log($"resize at {_size}");
            _size *= 2;
            var newData = new Node[_size];
            Array.Copy(_data,0,newData,0,_data.Length);

            _data = newData;
        }

        public void AddChildren(ref Node node)
        {
            var count = node.GetChildrenCount();

            if (count == 0)
            {
                return;
            }

            //Find any sufficiently sized empty spots in array or return end of array
            var index = FindSpace();

            //If it's end of the array, make sure _next is updated
            if (index == _next)
            {
                _next += count;
            }



            node.FirstChild = index;
        }

        public ref Node GetNChild(Node parent, int n)
        {
            var index = parent.FirstChild + n;

            if (index >= _data.Length)
            {
                Resize();
            }

            return ref _data[parent.FirstChild + n];
        }

        public void DepthFirst(ref Node node, Vector3 position, Vector3 size, IDensityFunction densityFunction, int maxLevel, int level = 0)
        {
            for (var i = 0; i < 8; i++)
            {
                var childSize = NodeOffsets[i];
                childSize.Scale(size/2);
                var childPosition = position + childSize;

                var overlap = CheckOverlap(childPosition, childPosition + size / 2, densityFunction.Minimum, densityFunction.Maximum);
                node.SetChildMask(i, overlap);
            }

            AddChildren(ref node);

            if (level >= maxLevel)
                return;

            var count = node.GetChildrenCount();

            for (var i = 0; i < count; i++)
            {
                ref var child = ref GetNChild(node, i);
                var childIndex = node.GetNChildIndex(i);
                var childPosition = NodeOffsets[childIndex];
                childPosition.Scale(size / 2);
                DepthFirst(ref child, position + childPosition, size / 2, densityFunction, maxLevel, level+1);
            }
        }

        public bool CheckOverlap(Vector3 min, Vector3 max, Vector3 min2, Vector3 max2)
        {
            var center = min + (max - min) / 2;
            var center2 = min2 + (max2 - min2) / 2;
            var half = max - center;
            var half2 = max2 - center2;

            if (Math.Abs(center.x - center2.x) > (half.x + half2.x)) return false;
            if (Math.Abs(center.y - center2.y) > (half.y + half2.y)) return false;
            if (Math.Abs(center.z - center2.z) > (half.z + half2.z)) return false;

            return true;
        }
    }
}
