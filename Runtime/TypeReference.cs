using System;
using System.Linq;
using UnityEngine;

namespace Rafasixteen.TypeReference.Runtime
{
    [Serializable]
    public class TypeReference<T> : ISerializationCallbackReceiver, IEquatable<TypeReference<T>>
    {
        #region FIELDS

        [NonSerialized] private Type _type;

        [SerializeField] private string _typeFullName;

        #endregion

        #region CONSTRUCTORS

        public TypeReference(Type type)
        {
            if (type != null && !typeof(T).IsAssignableFrom(type))
                throw new ArgumentException($"The provided type is not a valid type. Ensure it is a subclass of {typeof(T).Name}.");

            Type = type;
        }

        #endregion

        #region PROPERTIES

        public Type Type
        {
            get => _type;
            private set
            {
                if (value != null && !typeof(T).IsAssignableFrom(value))
                    throw new ArgumentException($"The provided type is not a valid type. Ensure it is a subclass of {typeof(T).Name}.");

                _type = value;
                _typeFullName = value?.FullName;
            }
        }

        #endregion

        #region METHODS

        public override string ToString() => Type?.Name ?? "null";

        public override int GetHashCode() => Type?.GetHashCode() ?? 0;

        public override bool Equals(object obj) => obj is TypeReference<T> other && Equals(other);

        public bool Equals(TypeReference<T> other) => other != null && Type == other.Type;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_typeFullName))
            {
                Type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(type => type.FullName == _typeFullName && typeof(T).IsAssignableFrom(type));
            }
            else
            {
                Type = null;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Type != null)
                _typeFullName = Type.FullName;
        }

        #endregion

        #region OPERATORS

        public static bool operator ==(TypeReference<T> a, TypeReference<T> b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.Type == b.Type;
        }

        public static bool operator !=(TypeReference<T> a, TypeReference<T> b) => !(a == b);

        public static implicit operator Type(TypeReference<T> reference) => reference.Type;

        public static implicit operator TypeReference<T>(Type type) => new(type);

        #endregion
    }
}