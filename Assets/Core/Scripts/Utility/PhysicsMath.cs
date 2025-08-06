using UnityEngine;

namespace PrSuperSoldier
{
    public static class PhysicsMath
    {
        /// <summary>
        /// vector를 normal 평면 위에 투영한 단위 벡터를 반환한다.
        /// </summary>
        /// <param name="vector">평면 위에 충돌한 벡터</param>
        /// <param name="normal">평면의 노말</param>
        /// <returns></returns>
        public static Vector3 CalculateTangent(Vector3 vector, Vector3 normal)
        {
            float vDotn = Vector3.Dot(vector, normal);
            Vector3 offsetVector = vDotn * -normal;
            Vector3 parallelVector = vector + offsetVector;

            return parallelVector.normalized;
        }
    }
}
