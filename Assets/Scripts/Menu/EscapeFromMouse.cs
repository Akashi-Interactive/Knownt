using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Knownt
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EscapeFromMouse : MonoBehaviour
    {
        private bool moving = false; 
        [SerializeField] private float speed = 1f;

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = (Physics2D.GetRayIntersection(ray));
            {
                if(hit.collider != null && hit.collider.gameObject == this.gameObject && !moving)
                {
                    MoveSomewhereElse();
                }
            }

        }

        private void MoveSomewhereElse()
        {
            Vector2 currentScreenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
            Vector2 newPosition = GetRandomScreenPoint();

            while(currentScreenPosition == newPosition)
            {
                newPosition = GetRandomScreenPoint();
            }

            StartCoroutine(_Move(newPosition));
        }

        private Vector2 GetRandomScreenPoint()
        {
            Vector2 screenPosition;

            float screenPositionY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float screenPositionX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            screenPosition = new Vector2(screenPositionX, screenPositionY);

            return screenPosition;
        }

        private IEnumerator _Move(Vector2 destination)
        {
            moving = true;

            while(Vector2.Distance(destination, this.transform.position) > 0.01f)
            {
                Vector3 direction = (Vector3)destination - this.transform.position;
                direction.Normalize();
                var step = speed * Time.deltaTime; 
                transform.position += direction * step;
                yield return null;
            }
            moving = false;
        }
    }
}