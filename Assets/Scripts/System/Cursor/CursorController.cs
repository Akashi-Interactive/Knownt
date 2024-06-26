using UnityEngine;

namespace Knownt
{
    public static class CursorController
    {
        public static bool IsCursorVisible => Cursor.visible;

        #region Cursor Toggle Method
        public static void DisableCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void EnableCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        #endregion
    }
}