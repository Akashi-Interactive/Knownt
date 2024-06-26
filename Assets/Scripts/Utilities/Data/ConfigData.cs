using System;

namespace Knownt
{
    [Serializable]
    public class ConfigData
    {
        public ConfigData()
        {
            AudioData = new AudioData();
        }

        public AudioData AudioData;
    }

    public class AudioData
    {
        public AudioData() { }

        public float Music_Volume = 0.8f;
        public float Effect_Volume = 0.9f;
    }
}
