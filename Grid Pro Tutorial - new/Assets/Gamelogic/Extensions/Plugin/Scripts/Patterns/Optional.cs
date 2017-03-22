using System;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// The base class of the generic Optional class.
	/// </summary>
	/// <remarks>This is an empty class; the reason it exists is so that a single property drawer can be used 
	/// for all class that derive from the generic Optional class.</remarks>
	public class Optional{ }

	/// <summary>
	/// Useful for displaying optional values in the inspector. 
	/// </summary>
	/// <typeparam name="T">The type of the optional value.</typeparam>
	/// <remarks>For this class to be displayable in the editor you cannot use it directly. You have to use one of the provided
	/// subclasses (or derive your own).</remarks>
	[Serializable]
	public class Optional<T> : Optional
	{
		[SerializeField]
		private bool useValue = false;

		[SerializeField]
		private T value;

		public bool UseValue
		{
			get { return useValue; }
			set { useValue = value; }
		}

		public T Value
		{
			get { return value; }
			set { this.value = value; }
		}
	}

	/// <summary>
	/// Represents an optional int value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Int32}" />
	[Serializable] public class OptionalInt : Optional<int>{ }

	/// <summary>
	/// Represents an optional float value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Single}" />
	[Serializable] public class OptionalFloat : Optional<float>{ }

	/// <summary>
	/// Represents an optional string value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{String}" />
	[Serializable] public class OptionalString : Optional<string>{ }

	/// <summary>
	/// Represents an optional GameObject.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{GameObject}" />
	[Serializable] public class OptionalGameObject : Optional<GameObject>{ }

	/// <summary>
	/// Represents an optional Vector2 value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Vector2}" />
	[Serializable] public class OptionalVector2 : Optional<Vector2> { }

	/// <summary>
	/// Represents an optional Vector3 value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Vector3}" />
	[Serializable] public class OptionalVector3 : Optional<Vector3> { }

	/// <summary>
	/// Represents an optional MonoBehaviour.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{MonoBehaviour}" />
	[Serializable] public class OptionalMonoBehaviour : Optional<MonoBehaviour> { }
}