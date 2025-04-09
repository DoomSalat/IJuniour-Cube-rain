using UnityEngine;

public class BombSpawner : Spawner<PooledBomb>
{
	private Transform _lastTransform;
	private Rigidbody _lastRigidbody;

	public void SetLastCubeTransform(Transform targetTransform, Rigidbody targetRigidbody)
	{
		_lastTransform = targetTransform;
		_lastRigidbody = targetRigidbody;

		Pool.Get();
	}

	protected override Vector3 GetSpawnPosition()
	{
		return _lastTransform.position;
	}

	protected override Quaternion GetSpawnRotation()
	{
		return _lastTransform.rotation;
	}

	protected override void OnGetFromPool(PooledBomb poolObj)
	{
		base.OnGetFromPool(poolObj);
		poolObj.OnSpawn(GetRandomLiveTime());
		poolObj.SetRigidbodyState(_lastRigidbody);
	}
}