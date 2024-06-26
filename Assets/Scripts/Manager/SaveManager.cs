using UnityEngine;

namespace Knownt
{
    public class SaveManager : MonoBehaviour
    {
        public static ConfigData SavedData {  get; private set; }

        private void Awake()
        {
            Init();
        }

        #region Init Method
        /// <summary> Initalize parameters. </summary>
        private void Init()
        {
            SavedData = SaveConfigUtility.LoadConfigurationData();

            if (SavedData == null)
            {
                Debug.LogWarning($"<color=blue>No config data saved.</color>");
                SavedData = new ConfigData();
                if (!SaveConfigUtility.SaveConfigurationData(SavedData))
                {
                    Debug.LogError($"<color=blue>Couldn't save new config data.</color>");
                    return;
                }
                Debug.Log($"<color=blue>New configuration created.</color>");
            }
        }
        #endregion

        #region Reload Method
        /// <summary> Reaload current config data. </summary>
        public static void RealoadConfigFile()
        {
            ConfigData configData = SaveConfigUtility.LoadConfigurationData();
            if (configData != null)
            {
                SavedData = configData;
                Debug.Log($"<color=blue>New configuration loaded.</color>");
            }
        }
        #endregion

        #region Save Method
        /// <summary> Save current config data. </summary>
        public static void SaveConfigFile()
        {
            if (!SaveConfigUtility.SaveConfigurationData(SavedData))
            {
                Debug.LogError($"<color=blue>Couldn't save new config data.</color>");
                return;
            }
            Debug.Log($"<color=blue>Configuration saved.</color>");
        }
        #endregion
    }
}
