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
                .SphereCastAll(origin + -direction * radius, radius, direction, maxDistance + radius, layerMask,
                    queryTriggerInteraction)
                .Where(hit => hit.collider != null && hit.point != Vector3.zero && hit.distance <= maxDistance)
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
                .SphereCastAll(origin + -direction * radius, radius, direction, maxDistance + radius, layerMask,
                    queryTriggerInteraction)
                .FirstOrDefault(hit =>
                    hit.collider != null && hit.point != Vector3.zero && hit.distance <= maxDistance);
            return hitInfo.collider != null;
            UnityEngine.Physics
                .SphereCastAll(origin + -direction * radius, radius, direction, maxDistance + radius, layerMask,
                    queryTriggerInteraction)
                .First(hit => hit.collider != null && hit.point != Vector3.zero && hit.distance <= maxDistance);
        }
    }
}