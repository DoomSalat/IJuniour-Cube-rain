using UnityEngine;

[RequireComponent(typeof(FirstTouchDetector), typeof(Rigidbody))]
public class PooledCube : PooledObject
{
	public event System.Action<Transform, Rigidbody> StateReturned;

	private Rigidbody _rigidbody;
	private FirstTouchDetector _tucher;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_tucher = GetComponent<FirstTouchDetector>();
	}

	private void OnEnable()
	{
		_tucher.Tuched += OnTouchReturn;
	}

	private void OnDisable()
	{
		_tucher.Tuched -= OnTouchReturn;
	}

	public override void OnReturn()
	{
		StateReturned?.Invoke(transform, _rigidbody);
	}

	private void OnTouchReturn(Collision collision)
	{
		ReturnToPool(0);
	}
}
