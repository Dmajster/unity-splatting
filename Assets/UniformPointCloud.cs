namespace Assets
{
    public class UniformPointCloud
    {
        private readonly bool[] _data;
        public int Width;
        public int Height;
        public int Length;

        public UniformPointCloud(int width, int height, int length)
        {
            _data = new bool[width * height * length];
            Width = width;
            Height = height;
            Length = length;
        }

        public bool Get(int x, int y, int z)
        {
            return _data[y * Height * Length + z * Width + x];
        }

        public void Set(int x, int y, int z, bool value)
        {
            _data[y * Height * Length + z * Width + x] = value;
        }
    }
}
