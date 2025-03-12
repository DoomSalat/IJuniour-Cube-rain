using UnityEngine;

[RequireComponent(typeof(FirstTouchDetector))]
public class PooledObject : MonoBehaviour
{
	private FirstTouchDetector _tucher;

	public event System.Action<PooledObject> TuchedReturn;

	private void Awake()
	{
		_tucher = GetComponent<FirstTouchDetector>();
	}

	private void OnEnable()
	{
		_tucher.Tuched += ReturnPool;
	}

	private void OnDisable()
	{
		_tucher.Tuched -= ReturnPool;
	}

	private void ReturnPool(Collision collision)
	{
		TuchedReturn?.Invoke(this);
	}
}
