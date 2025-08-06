using UnityEngine;

namespace PrSuperSoldier
{
    public class ProjectionTest : MonoBehaviour
    {
        public float distance = 10000;

        private void OnDrawGizmos()
        {
            Vector3 vector = transform.forward * distance;
            Gizmos.DrawRay(transform.position, vector);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance))
            {
                Vector3 leftVector = transform.forward * (distance - hit.distance);
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(hit.point, leftVector);

                // calculate parallel vector;
                float vDotn = Vector3.Dot(leftVector, hit.normal);
                Vector3 offsetVector = vDotn * -hit.normal;
                Vector3 parallelVector = leftVector + offsetVector;

                Gizmos.color = Color.red;
                Gizmos.DrawRay(hit.point, parallelVector);
            }
        }
    }
}
