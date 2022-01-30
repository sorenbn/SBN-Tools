using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampler
{
    /// <summary>
    /// Generates a list of Poisson Disc Sampled points inside a rectangular sample region 
    /// </summary>
    /// <param name="cellRadius">The radius between each cell. Tweak this to change to distance between each point</param>
    /// <param name="sampleRegionSize">A rectangle defining the region size of the sampling</param>
    /// <param name="numSamplesBeforeRejection">Determines how accurate the algorithm is. Higher = more precision but also more expensive</param>
    /// <returns></returns>
    public static List<Vector2> GeneratePoints(float cellRadius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        float cellSize = cellRadius / Mathf.Sqrt(2);
        int[,] grid = new int[Mathf.CeilToInt((sampleRegionSize.x / cellSize)), Mathf.CeilToInt((sampleRegionSize.y / cellSize))];

        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnpoints = new List<Vector2>();

        spawnpoints.Add(sampleRegionSize / 2);

        while (spawnpoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnpoints.Count);
            Vector2 spawnCenter = spawnpoints[spawnIndex];

            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2.0f;

                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + dir * Random.Range(cellRadius, 2 * cellRadius);

                if (IsValid(candidate, sampleRegionSize, cellSize, cellRadius, points, grid))
                {
                    points.Add(candidate);
                    spawnpoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;

                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted)
            {
                spawnpoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    private static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if(candidate.x >= 0 && candidate.x < sampleRegionSize.x &&
            candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;

                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }
}