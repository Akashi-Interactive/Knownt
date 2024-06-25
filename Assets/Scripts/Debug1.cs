using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knownt
{
    public class Debug1 : MonoBehaviour
    {
        [SerializeField]
        Enemy enemy;

        [ContextMenu("Print")]
        public void Alert()
        {
            enemy.OnAlerted(gameObject);
        }
    }
}
