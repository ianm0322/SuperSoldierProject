using UnityEngine;

namespace PrSuperSoldier.Utilities
{
    public static class CastUtility
    {
        public static bool CapsuleCast(
            in Vector3 origin,
            float radius,
            float height,
            in Vector3 up,
            in Vector3 direction,
            out RaycastHit hitInfo,
            float maxDistance = float.PositiveInfinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.CapsuleCast(
                origin + up * height / 2,
                origin - up * height / 2,
                radius,
                direction,
                out hitInfo,
                maxDistance,
                layerMask,
                queryTriggerInteraction);
        }

        public static bool CapsuleCast(
       in Vector3 origin,
       float radius,
       float height,
       in Vector3 direction,
       out RaycastHit hitInfo,
       float maxDistance = float.PositiveInfinity,
       int layerMask = Physics.DefaultRaycastLayers,
       QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return CapsuleCast(origin, radius, height, Vector3.up, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }
    }
}
