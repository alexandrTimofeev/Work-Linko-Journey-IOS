using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Adapted from:
/// Polygon Collider Simplification - by j-bbr - under MIT License
/// https://github.com/j-bbr/PolygonColliderSimplification
/// </summary>

namespace Collider2DOptimization
{
    /// <summary>
    /// Polygon collider optimizer. Removes points from the collider polygon with 
    /// the given reduction Tolerance
    /// </summary>
    public static class PolygonColliderOptimizer
    {
        public static void Optimize(PolygonCollider2D coll, float tolerance)
        {
            if (tolerance > 0)
            {
                List<Vector2> path = new List<Vector2>();
                path.AddRange(coll.points);
                path = ShapeOptimizationHelper.DouglasPeuckerReduction(path, tolerance);
                coll.SetPath(0, path.ToArray());
            }
        }
    }
}