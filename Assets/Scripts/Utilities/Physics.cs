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
                .SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction)
                .Where(hit => Vector3.Distance(origin, hit.point) <= maxDistance).ToArray();
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
            RaycastHit? hit = UnityEngine.Physics
                .SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction)
                // .OrderBy(hit => hit.distance)
                .FirstOrDefault(hit => Vector3.Distance(origin, hit.point) <= maxDistance);
            if (hit != null)
            {
                hitInfo = (RaycastHit)hit;
                return true;
            }
            hitInfo = default;
            return false;
        }

        
    }
}