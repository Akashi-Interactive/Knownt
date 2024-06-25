using UnityEngine;

namespace Knownt
{
    public class Player : MonoBehaviour
    {
        [Header("Player config")]
        public float moveSpeed;
        public float aimSpeed;
        public GameObject projectile;
        public GameObject mouseCross;

        private PlayerControls playerControls;

        private Vector2 movementInput;
        private Vector3 mouseWorldPos;

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
        }

        private void Fire()
        {/*
            var transform = this.transform;
            var newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position + transform.forward * 0.6f;
            newProjectile.transform.rotation = transform.rotation;
            const int size = 1;
            newProjectile.transform.localScale *= size;
            newProjectile.GetComponent<Rigidbody>().mass = Mathf.Pow(size, 3);
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
            newProjectile.GetComponent<MeshRenderer>().material.color =
                new Color(Random.value, Random.value, Random.value, 1.0f);*/
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
