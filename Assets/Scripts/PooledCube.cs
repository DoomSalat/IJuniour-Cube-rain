using UnityEngine;

[RequireComponent(typeof(FirstTouchDetector), typeof(Rigidbody))]
public class PooledCube : PooledObject
{
	private Rigidbody _rigidbody;
	private FirstTouchDetector _tucher;

	public delegate void ReturnAction(Transform selfTransform, Rigidbody selfRigidbody);
	public event ReturnAction StateReturned;

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

	private void OnTouchReturn(Collision collision)
	{
		ReturnToPool(0);
	}

	public override void OnReturn()
	{
		StateReturned?.Invoke(transform, _rigidbody);
	}
}
