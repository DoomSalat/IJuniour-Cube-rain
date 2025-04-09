using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PooledBomb : PooledObject
{
	[SerializeField] private Exploser _exploser;
	[SerializeField] private MaterialFade _fade;

	private Rigidbody _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void OnSpawn(float liveDuration)
	{
		_fade.StartFade(liveDuration);
		ReturnToPool(liveDuration);
	}

	public void SetRigidbodyState(Rigidbody cloneRigidbody)
	{
		_rigidbody.linearVelocity = cloneRigidbody.linearVelocity;
		_rigidbody.angularVelocity = cloneRigidbody.angularVelocity;
	}

	public override void OnReturn()
	{
		_exploser.Explode();
	}
}
