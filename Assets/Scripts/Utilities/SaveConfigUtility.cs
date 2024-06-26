using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Knownt
{
    /// <summary> <c>SaveConfigUtility</c> process load and save configuration file- </summary>
    public class SaveConfigUtility : MonoBehaviour
    {
        public static string FileDirectory
        {
            get { return Path.Combine(Application.persistentDataPath, "Configuration"); }
        }

        private static string FilePath
        {
            get { return Path.Combine(FileDirectory, "config.json"); }
        }

        #region Save Configuration File Method
        /// <summary> Save configuration file. </summary>
        /// <param name="data">ConfigData format data</param>
        /// <returns> True if the file is saved. False if couldn't save the file.</returns>
        public static bool SaveConfigurationData(ConfigData data)
        {
            if (!VerifyDirectory())
            {
                if (!CreateDirectory())
                {
                    return false;
                }
            }

            try
            {
                string json = JsonUtility.ToJson(data);

                File.WriteAllText(FilePath, json);

                string currentHashFile = CalculateHashFile();
                PlayerPrefs.SetString("HashConfiguration", currentHashFile);

                Debug.Log($"<color=green>Configuration file saved.</color>");

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error trying to save configuration file due to: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Load Configuration File Method
        /// <summary> Loads current configuration file. </summary>
        /// <returns> ConfigData data format. </returns>
        public static ConfigData LoadConfigurationData()
        {
            if (!VerifyDirectory())
            {
                if (!CreateDirectory())
                {
                    return null;
                }
            }

            if (VerifyFile())
            {
                try
                {
                    string json = File.ReadAllText(FilePath);

                    string savedHashFile = PlayerPrefs.GetString("HashConfiguration");
                    string currentHashFile = CalculateHashFile();

                    if (savedHashFile.Equals(currentHashFile))
                    {
#if DEBUG
                        Debug.Log("Configuration file data not alterated.");
#endif
                        return JsonUtility.FromJson<ConfigData>(json);
                    }
                    else
                    {
#if DEBUG
                        Debug.LogWarning("Configuration file data alterated.");
#endif
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error trying to load configuration file due to: " + ex.Message);
                }
            }
            return null;
        }
        #endregion

        #region Hash Calculator Method
        /// <summary> Converts to hash data the configuration file. </summary>
        /// <returns> The hash file data </returns>
        private static string CalculateHashFile()
        {
            try
            {
                using var sha256 = SHA256.Create();
                using var Stream = File.OpenRead(FilePath);
                byte[] hash = sha256.ComputeHash(Stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error calculating file hash: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Directory Methods
        /// <summary> Verifies if the configuration directory exists. </summary>
        /// <returns> True if directory exists. False if directory does not exist. </returns>
        private static bool VerifyDirectory()
        {
            return Directory.Exists(FileDirectory);
        }

        /// <summary> Try to create the configuration directory. </summary>
        /// <returns> True if direcotry has been created. False if directory couldn't be created.</returns>
        private static bool CreateDirectory()
        {
            try
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(FileDirectory);

                if (directoryInfo.Exists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Cannot create directory at: " + FileDirectory + "\nError: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region File Methods
        /// <summary> Verifies if the config file exists. </summary>
        /// <returns> True if file exists. False if file does not exist.</returns>
        private static bool VerifyFile()
        {
            return File.Exists(FilePath);
        }
        #endregion
    }
}
