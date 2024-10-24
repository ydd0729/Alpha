using UnityEngine;

namespace Yd.Collection
{
    public class KeyValueNameAttribute : PropertyAttribute
    {
        public KeyValueNameAttribute(string keyName, string valueName)
        {
            KeyName = keyName;
            ValueName = valueName;
        }

        public string KeyName { get; private set; }
        public string ValueName { get; private set; }
    }
}