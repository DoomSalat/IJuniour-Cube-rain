using UnityEngine;

[RequireComponent(typeof(FirstTouchDetector))]
public class PooledCube : PooledObject
{
	private FirstTouchDetector _tucher;

	public delegate void ReturnAction(Vector3 position, Quaternion rotation);
	public event ReturnAction PositionReturned;

	private void Awake()
	{
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
		PositionReturned?.Invoke(transform.position, transform.rotation);
	}
}
