using UnityEngine;

namespace Knownt
{
    public class Player : MonoBehaviour
    {
        [Header("Player config")]
        public float moveSpeed = 2f;
        public float timerBetweenShoots = 10f;
        public GameObject projectile;
        public GameObject mouseCross;

        private PlayerControls playerControls;

        private Vector2 movementInput;
        private Vector3 mouseWorldPos;
        private float shootCooldown;
        private bool canShoot = true;

        public void Awake()
        {
            playerControls = new PlayerControls();

            playerControls.Player.Fire.performed += ctx => { Fire(); };
        }

        public void Update()
        {
            movementInput = playerControls.Player.Move.ReadValue<Vector2>();     
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            if ()

            Move();
            Look();
        }

        private void Move()
        {
            if (movementInput.sqrMagnitude < 0.01f)
                return;

            var scaledMoveSpeed = moveSpeed * Time.deltaTime;

            var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * (Vector3)movementInput;

            transform.position += move * scaledMoveSpeed;
        }

        private void Look()
        {
            mouseCross.transform.position = mouseWorldPos;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perpendicular = transform.position - mousePos;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
            transform.rotation *= Quaternion.Euler(0, 0, -90); // Aim correction;
        }

        private void Fire()
        {
            Debug.Log("Firing");
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            newProjectile.GetComponent<Projectile>().Initialize(mouseWorldPos);
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
            playerControls.Player.Fire.performed -= ctx => { Fire(); };
        }
    }
}
