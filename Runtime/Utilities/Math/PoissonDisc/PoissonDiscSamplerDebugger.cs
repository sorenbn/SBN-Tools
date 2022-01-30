using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSamplerDebugger : MonoBehaviour
{
    public Vector2 RegionSize = Vector2.one * 10;
    public float Radius = 0.5f;
    public int RejectionSamples = 30;

    [Header("Display")]
    public bool displayRadius = true;

    public float DisplayRadius = 1.0f;

    private List<Vector2> points = new List<Vector2>();

    private void OnValidate()
    {
        points = PoissonDiscSampler.GeneratePoints(Radius, RegionSize, RejectionSamples);
    }

    private void OnDrawGizmos()
    {
        if (points == null || RegionSize.x == 0.0f || RegionSize.y == 0.0f)
        {
            return;
        }

        Gizmos.DrawWireCube((Vector2)transform.position, RegionSize);

        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere((Vector2)transform.position - RegionSize / 2 + points[i], DisplayRadius);

            if (displayRadius)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere((Vector2)transform.position - RegionSize / 2 + points[i], Radius);
            }
        }
    }
}