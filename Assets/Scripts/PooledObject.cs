using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
	public event System.Action<PooledObject, float> StartReturned;

	public virtual void OnReturn() { }

	protected void ReturnToPool(float timeReturn)
	{
		StartReturned?.Invoke(this, timeReturn);
	}
}
