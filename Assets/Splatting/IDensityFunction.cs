using UnityEngine;

namespace Assets.Splatting
{
    public interface IDensityFunction
    {
        Vector3 Minimum { get; set; }
        Vector3 Maximum { get; set; }

        float Value(Vector3 position);
    }
}
