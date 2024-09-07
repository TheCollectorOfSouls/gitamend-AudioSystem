using System;
using UnityEngine;

namespace AudioSystem.Utils
{
    [Serializable]
    public struct HashKey : IEquatable<HashKey>
    {
        [SerializeField] [HideInInspector]private string name;
        [SerializeField] [HideInInspector]private int hashedKey;

        public HashKey(string name)
        {
            this.name = name;
            hashedKey = name.ComputeFNV1aHash();
        }

        public bool Equals(HashKey other) => hashedKey == other.hashedKey;

        public override bool Equals(object obj) => obj is HashKey other && Equals(other);

        public override int GetHashCode() => hashedKey;
        public override string ToString() => name;
        
        public static bool operator ==(HashKey lhs, HashKey rhs) => lhs.hashedKey == rhs.hashedKey;
        public static bool operator !=(HashKey lhs, HashKey rhs) => !(lhs == rhs);
    }
}
