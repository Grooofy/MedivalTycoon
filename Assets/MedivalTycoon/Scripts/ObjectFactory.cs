using UnityEngine;

namespace Characters
{
    public static class ObjectFactory
    {
        public static T CreateObjectWithComponent<T>(string name) where T : Component
        {
            GameObject go = new GameObject(name);
            return go.AddComponent<T>();
        }
    }
}