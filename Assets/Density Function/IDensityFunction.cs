using UnityEngine;

namespace Assets.Density_Function
{
    public interface IDensityFunction
    {
        Vector3 Minimum { get; set; }
        Vector3 Maximum { get; set; }

        float Value(Vector3 position);
    }
}
