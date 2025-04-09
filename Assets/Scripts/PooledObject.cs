using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
	public event System.Action<PooledObject, float> StartReturned;

	public virtual void OnReturn() { }

	protected virtual void ReturnToPool(float timeReturn)
	{
		StartReturned?.Invoke(this, 0);
	}
}
