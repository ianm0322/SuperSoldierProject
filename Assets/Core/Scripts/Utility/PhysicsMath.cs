using UnityEngine;

namespace PrSuperSoldier
{
    public static class PhysicsMath
    {
        /// <summary>
        /// vector�� normal ��� ���� ������ ���� ���͸� ��ȯ�Ѵ�.
        /// </summary>
        /// <param name="vector">��� ���� �浹�� ����</param>
        /// <param name="normal">����� �븻</param>
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
