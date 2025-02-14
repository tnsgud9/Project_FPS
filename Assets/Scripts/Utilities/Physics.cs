using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class Physics
    {
        public static RaycastHit[] CylinderCastAll(
            Vector3 origin,
            float radius,
            Vector3 direction,
            float maxDistance = Mathf.Infinity,
            int layerMask = -5,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return UnityEngine.Physics
                //Add SphereCast to shoot up and down to validate the radius from the start to the destination points in that direction.
                .SphereCastAll(origin + -direction * radius, radius, direction, maxDistance + radius, layerMask,
                    queryTriggerInteraction)
                .Where(hit =>
                    // Ignore if the SphereCast HitInfo distance is 0 and set hit.point to Vector3.zero.
                    hit.collider != null && hit.point != Vector3.zero &&

                    // Ignore hit occurrence exceptions above the start position
                    Vector3.Dot((hit.point - origin).normalized, direction) > 0 &&

                    // Ignore hemispheres (below) at maximum distance + radius
                    !(hit.distance > maxDistance &&
                      Vector3.Dot((hit.point - (origin + direction * maxDistance)).normalized, direction) > 0))
                .ToArray();
        }


        public static bool CylinderCast(
            Vector3 origin,
            float radius,
            Vector3 direction,
            out RaycastHit hitInfo,
            float maxDistance = Mathf.Infinity,
            int layerMask = -5,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            hitInfo = UnityEngine.Physics
                //Add SphereCast to shoot up and down to validate the radius from the start to the destination points in that direction.
                .SphereCastAll(origin + -direction * radius, radius, direction, maxDistance + radius, layerMask,
                    queryTriggerInteraction)
                .FirstOrDefault(hit =>
                    // Ignore if the SphereCast HitInfo distance is 0 and set hit.point to Vector3.zero.
                    hit.collider != null && hit.point != Vector3.zero &&

                    // Ignore hit occurrence exceptions above the start position
                    Vector3.Dot((hit.point - origin).normalized, direction) > 0 &&

                    // Ignore hemispheres (below) at maximum distance + radius
                    !(hit.distance > maxDistance &&
                      Vector3.Dot((hit.point - (origin + direction * maxDistance)).normalized, direction) > 0));
            return hitInfo.collider != null;
        }
    }
}