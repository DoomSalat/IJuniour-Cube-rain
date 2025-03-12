using UnityEngine;

[RequireComponent(typeof(Tucher))]
public class PooledObject : MonoBehaviour
{
	private Tucher _tucher;

	public event System.Action<PooledObject> TuchedReturn;

	private void Awake()
	{
		_tucher = GetComponent<Tucher>();
	}

	private void OnEnable()
	{
		_tucher.ResetVelocity();
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
