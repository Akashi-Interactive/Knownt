using UnityEngine;

namespace Knownt
{
    public class MiniGhost : MonoBehaviour
    {
        public CircleCollider2D circleCollider2D;

        public void AlertEnemies()
        {
            Vector3 center = circleCollider2D.bounds.center;
            float halfExtents = circleCollider2D.radius;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, halfExtents);

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    collider.gameObject.GetComponent<Enemy>().OnAlerted(this.gameObject);
                    Debug.Log("<color=red>Enemy Alerted from " + gameObject.name + "</color>");
                }
            }

        }
    }
}