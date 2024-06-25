using UnityEngine;

namespace Knownt
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Config")]
        public float moveSpeed = 2.5f;
        public float lifeTime = 5f;

        private Vector3 destination;

        private void Update()
        {
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                OnDestroy();
            }
        }

        private void FixedUpdate()
        {
            var scaledMoveSpeed = moveSpeed * Time.deltaTime;

            var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * (Vector3)gameObject.transform.right.normalized;

            transform.position += move * scaledMoveSpeed;
        }

        public void Initialize(Vector3 destination)
        {
            Invoke(nameof(OnDestroy), lifeTime);
            this.destination = destination;
            GetComponentInChildren<SpriteRenderer>().color =
                new Color(Random.value, Random.value, Random.value, 1.0f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnDestroy();
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}