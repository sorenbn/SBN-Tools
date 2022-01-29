using System.Collections.Generic;
using UnityEngine;

namespace SBN.Utilities.Mathematics
{
    public static class Mathfs
    {
        /// <summary>
        /// Determines if the given point is inside the range of the target.
        /// </summary>
        public static bool PointInRangeSqr(Vector2 point, Vector2 target, float targetRange)
        {
            Vector3 displacemnt = target - point;
            return displacemnt.sqrMagnitude <= targetRange * targetRange;
        }

        /// <summary>
        /// Determines if the given point is inside the cone angle.
        /// </summary>
        public static bool PointInCone(Vector2 point, Vector2 coneOrigin, Vector2 coneDirection, float angleInDeg)
        {
            Vector2 dirToPoint = (point - coneOrigin).normalized;
            float dot = Vector2.Dot(coneDirection, dirToPoint);
            float angleToTarget = Mathf.Acos(Mathf.Clamp(dot, -1, 1)) * Mathf.Rad2Deg;

            return angleToTarget <= angleInDeg;
        }

        /// <summary>
        /// Rotates a vector by the given angle.
        /// </summary>
        public static Vector2 RotateVectorBy(Vector2 vector, float angleInDeg)
        {
            angleInDeg *= Mathf.Deg2Rad;

            var ca = Mathf.Cos(angleInDeg);
            var sa = Mathf.Sin(angleInDeg);
            var rx = vector.x * ca - vector.y * sa;

            return new Vector2((float)rx, (float)(vector.x * sa + vector.y * ca));
        }

        /// <summary>
        /// Adds a random rotation to a vector.
        /// </summary>
        public static Vector2 AddRandomRotation(Vector2 value, float minAngleInDeg, float maxAngleInDeg)
        {
            return Quaternion.Euler(0.0f, 0.0f, Random.Range(minAngleInDeg, maxAngleInDeg)) * value;
        }

        /// <summary>
        /// Adds a signed random rotation to a vector, meaning it will either be the positive.
        /// or negative angle in degrees
        /// </summary>
        public static Vector2 AddRandomRotationSign(Vector2 value, float angleInDeg)
        {
            return Quaternion.Euler(0.0f, 0.0f, RandomSign() * angleInDeg) * value;
        }

        /// <summary>
        /// Gets a percentage of distance travelled between start and end.
        /// </summary>
        public static float GetPointPercentageAlong(Vector3 point, Vector3 start, Vector3 end)
        {
            var ab = end - start;
            var ac = point - start;
            return Vector3.Dot(ac, ab.normalized) / ab.magnitude;
        }

        /// <summary>
        /// Gets a direction from the given angle. 
        /// </summary>
        public static Vector2 AngleToDirection(float angInDeg)
        {
            float angInRad = angInDeg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angInRad), Mathf.Sin(angInRad));
        }

        /// <summary>
        /// Gets an angle from a direction vector.
        /// </summary>
        public static float DirectionToAngle(Vector2 value)
        {
            float angle = Mathf.Abs(Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg);
            Vector3 cross = Vector3.Cross(Vector2.right, value);

            if (cross.z < 0)
                angle = 360 - angle;

            return angle;
        }

        /// <summary>
        /// Gets a quaternion rotation based on a given direction.
        /// </summary>
        public static Quaternion GetRotationByDirection(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90.0f;

            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Converts a local point to world space.
        /// </summary>
        public static Vector2 LocalToWorld(Transform localOrigin, Vector2 localPoint)
        {
            Vector2 pointOffset = localOrigin.right * localPoint.x + localOrigin.up * localPoint.y;
            return (Vector2)localOrigin.position + pointOffset;
        }

        /// <summary>
        /// Converts a world point to local space.
        /// </summary>
        public static Vector2 WorldToLocal(Transform localOrigin, Vector2 worldPoint)
        {
            var relativePoint = worldPoint - (Vector2)localOrigin.position;
            float x = Vector2.Dot(relativePoint, localOrigin.right);
            float y = Vector2.Dot(relativePoint, localOrigin.up);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Reflects a vector off a surface based on the surface normal.
        /// </summary>
        public static Vector3 Reflect(Vector3 directionVector, Vector3 surfaceNormal)
        {
            return directionVector - (2.0f * Vector3.Dot(directionVector, surfaceNormal) * surfaceNormal);
        }

        /// <summary>
        /// Gets the average vector from a given list of vectors.
        /// </summary>
        public static Vector2 GetAverageVector(ICollection<Vector2> vectors)
        {
            Vector2 avg = Vector2.zero;

            foreach (Vector2 pos in vectors)
                avg += pos;

            avg /= vectors.Count;

            return avg;
        }

        /// <summary>
        /// Gets a vector that's been fixed to the given angle
        /// i.e. if 90 degrees is given, it would stick to every 90 degrees.
        /// </summary>
        public static Vector3 FixVectorToAngle(Vector3 value, float angleInDeg)
        {
            float angle = Vector3.Angle(value, Vector3.up);

            if (angle < angleInDeg / 2.0f)
                return Vector3.up * value.magnitude;

            if (angle > 180.0f - angleInDeg / 2.0f)
                return Vector3.down * value.magnitude;

            float t = Mathf.Round(angle / angleInDeg);
            float deltaAngle = (t * angleInDeg) - angle;

            Vector3 axis = Vector3.Cross(Vector3.up, value);
            Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);

            Vector3 result = q * value;

            if (Mathf.Abs(result.x) <= 0.01f)
                result.x = 0.0f;

            if (Mathf.Abs(result.y) <= 0.01f)
                result.y = 0.0f;

            return result;
        }

        /// <summary>
        /// Gets the angle from a vector that's been fixed by an angle
        /// i.e. if 90 degrees is given, the angles would return every 90, 180, 270
        /// etc. based on the vector.
        /// </summary>
        public static float GetFixedAngleFromVector(Vector3 value, float angleInDeg)
        {
            float angle = -Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg + 90.0f;
            angle = Mathf.Round(angle / angleInDeg) * angleInDeg;

            return angle;
        }

        /// <summary>
        /// Gets a random point inside the annulus which is the area between two concentric circles.
        /// https://mathworld.wolfram.com/Annulus.html
        /// </summary>
        public static Vector2 GetRandomPointInAnnulus(float minRadius, float maxRadius)
        {
            Vector2 posInCircle = Random.insideUnitCircle;
            float length = posInCircle.magnitude;
            float ratioRadius = minRadius / maxRadius;

            return (((1.0f - ratioRadius) * length + ratioRadius) / length) * maxRadius * posInCircle;
        }

        /// <summary>
        /// Gets the closest point from a list, based on the given point.
        /// </summary>
        public static Vector2 GetClosestPoint(Vector2 point, Vector2[] points, out int index)
        {
            float closestDistanceSqr = Mathf.Infinity;

            index = 0;

            for (int i = 0; i < points.Length; i++)
            {
                float distanceSqr = (points[i] - point).sqrMagnitude;

                if (distanceSqr < closestDistanceSqr * closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    index = i;
                }
            }

            return points[index];
        }

        /// <summary>
        /// Gets the surface area of any given mesh.
        /// </summary>
        public static float GetMeshSurfaceArea(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            float totalSurfaceAreaSum = 0.0f;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                totalSurfaceAreaSum += GetTriangleSurfaceArea
                (
                    a: vertices[triangles[i]],
                    b: vertices[triangles[i + 1]],
                    c: vertices[triangles[i + 2]]
                );
            }

            return totalSurfaceAreaSum;
        }

        /// <summary>
        /// Gets the surface area of a single triangle.
        /// </summary>
        public static float GetTriangleSurfaceArea(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 left = b - a;
            Vector3 right = c - a;
            Vector3 cross = Vector3.Cross(left, right);

            return cross.magnitude * 0.5f;
        }

        /// <summary>
        /// Returns a value that has been remapped from the current min/max values to the new min/max values
        /// </summary>
        public static float RemapRange(float value, float currentMin, float currentMax, float newMin, float newMax)
        {
            return (value - currentMin) / (currentMax - currentMin) * (newMax - newMin) + newMin;
        }

        /// <summary>
        /// Returns a random sign (-1 or 1)
        /// </summary>
        public static int RandomSign()
        {
            return Random.Range(0, 2) * 2 - 1;
        }
    }
}
