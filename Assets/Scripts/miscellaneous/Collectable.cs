using UnityEngine;

namespace Knownt
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private RectTransform icon;
        [SerializeField] private float speed;

        private bool isMovingToUI;
        private Camera mainCamera;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Collision" + collision.gameObject.name);
            if (collision.CompareTag("Player"))
            {
                if (!isMovingToUI)
                    isMovingToUI = true;
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (isMovingToUI)
            {
                var worldPos = mainCamera.WorldToScreenPoint(icon.transform.position);
                Vector3 perpendicular = transform.position - worldPos;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
                transform.rotation *= Quaternion.Euler(0, 0, 90);

                transform.position = Vector2.MoveTowards(transform.position, worldPos, speed * Time.deltaTime);

                if (Vector2.Distance(worldPos, transform.position) < 0.1)
                {
                    isMovingToUI = false;
                    CollectableUI.instance.UpdateCollectableText();
                    Destroy(gameObject);
                }
            }
        }
    }
}
