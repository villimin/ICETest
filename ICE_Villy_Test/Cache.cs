using System.Collections.Generic;


namespace ICE_Villy_Test
{
    public static class Cache
    {
        public static Dictionary<string, float> Repository;

        static Cache()
        {
            Repository = new Dictionary<string, float>();
        }

        public static void Set(string key, float value)
        {
            Repository[key] = value;
        }

        public static bool Get(string key, out float value)
        {
            value = 0;

            if (Repository.ContainsKey(key))
            {
                value = Repository[key];
                return true;
            }

            return false;
        }
    }
}