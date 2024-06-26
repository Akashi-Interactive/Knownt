using UnityEngine;
using UnityEngine.InputSystem;

namespace Knownt
{
    public class Player : MonoBehaviour
    {
        [Header("Player config")]
        public float moveSpeed = 2f;
        public float timerBetweenShoots = 10f;
        [Space]
        [Header("Objects")]
        public GameObject projectile;
        public GameObject shootPoint;
        public GameObject mouseCross;

        private PlayerControls playerControls;

        private Vector2 movementInput;
        private Vector2 crossInput;
        private Vector3 mouseWorldPos;
        private float crossSpeed = 1f;
        private float shootCooldown;
        private bool isReloading = false;

        public void Awake()
        {
            playerControls = new PlayerControls();

            AddCallbacksInputs();
        }

        public void Update()
        {
            movementInput = playerControls.Player.Move.ReadValue<Vector2>();
            crossInput = playerControls.Player.Look.ReadValue<Vector2>();
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            if (isReloading)
            {
                shootCooldown += Time.deltaTime;

                if (shootCooldown >= timerBetweenShoots)
                {
                    isReloading = false;
                    AddCallbacksInputs();
                    Debug.Log("<color=#1FA0E6>Shoot cooldown finished.</color>");
                }
            }

            Move();
            Look();
        }

        private void Move()
        {
            if (movementInput.sqrMagnitude < 0.01f)
                return;

            float scaledMoveSpeed = moveSpeed * Time.deltaTime;

            Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * (Vector3)movementInput;

            transform.position += move * scaledMoveSpeed;
        }

        private void Look()
        {
            if(crossInput.sqrMagnitude > 0.1f)
            {
                float scaledMoveSpeed = crossSpeed * Time.deltaTime;

                Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * (Vector3)crossInput;

                mouseCross.transform.position += move * scaledMoveSpeed;
            } else {
                mouseCross.transform.position = mouseWorldPos;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 perpendicular = transform.position - mousePos;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
                transform.rotation *= Quaternion.Euler(0, 0, -90); // Aim correction;
            }
        }

        private void Fire(InputAction.CallbackContext context)
        {
            Debug.Log("<color=#1FA0E6>Player shooted.</color>");
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.SetPositionAndRotation(shootPoint.transform.position, transform.rotation);
            newProjectile.GetComponent<Projectile>().Initialize(mouseWorldPos);
            RemoveCallbacksInputs();
            isReloading = true;
            shootCooldown = 0;
        }

        /*
        private IEnumerator BurstFire(int burstAmount)
        {
            for (var i = 0; i < burstAmount; ++i)
            {
                Fire();
                yield return new WaitForSeconds(0.1f);
            }
        }*/

        private void AddCallbacksInputs()
        {
            playerControls.Player.Fire.performed += Fire;
        }

        private void RemoveCallbacksInputs()
        {
            playerControls.Player.Fire.performed -= Fire;
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnDestroy()
        {
            RemoveCallbacksInputs();
        }
    }
}
