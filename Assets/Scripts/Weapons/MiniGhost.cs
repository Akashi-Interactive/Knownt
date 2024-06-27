using UnityEngine;

namespace Knownt
{
    public class MiniGhost : MonoBehaviour
    {
        public float halfExtents;
        public float lifeTime;

        public void AlertEnemies()
        {
            Vector3 center = gameObject.transform.position;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, halfExtents);

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    collider.gameObject.GetComponent<Enemy>().OnAlerted(gameObject);
                    Debug.Log("<color=red>Enemy Alerted from " + gameObject.name + "</color>");
                }
            }

            Invoke(nameof(OnDestroy), lifeTime);

        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}