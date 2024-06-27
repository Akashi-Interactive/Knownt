using UnityEngine;

namespace Knownt
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private float speed;
  
        private RectTransform icon;
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
            icon = CollectableUI.instance.collectableTransform;
        }

        private void FixedUpdate()
        {
            if (isMovingToUI)
            {
                /*
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(icon.transform.position);
                Vector3 perpendicular = transform.position - worldPos;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
                transform.rotation *= Quaternion.Euler(0, 0, -90);

                float scaledMoveSpeed = speed * Time.deltaTime;
                Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * (Vector3)gameObject.transform.right.normalized;
                transform.position += move * scaledMoveSpeed;*/

                Vector3 worldPos = mainCamera.ScreenToWorldPoint(icon.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, worldPos, speed * Time.deltaTime);

                if (Vector3.Distance(worldPos, transform.position) < 0.5f)
                {
                    isMovingToUI = false;
                    CollectableUI.instance.UpdateCollectableText();
                    Destroy(gameObject);
                }
            }
        }
    }
}
