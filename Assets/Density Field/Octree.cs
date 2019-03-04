using System;
using UnityEngine;

namespace Assets
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
        private readonly Node[] _data = new Node[100];
        private int _next = 1;

        public ref Node Get(int index)
        {
            return ref _data[index];
        }

        public int FindSpace()
        {
            return _next;
        }

        public void AddChildren(ref Node node)
        {
            var count = node.GetChildrenCount();

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
            return ref _data[parent.FirstChild + n];
        }
    }
}
