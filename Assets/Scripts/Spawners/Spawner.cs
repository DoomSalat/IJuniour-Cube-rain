using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public abstract class Spawner<T> : SpawnerBase where T : PooledObject
{
	[SerializeField] private T _prefab;
	[SerializeField] private Vector2Int _minMaxLiveTime = new Vector2Int(2, 5);
	[SerializeField][Min(1)] private int _poolMaxSize = 10;

	protected ObjectPool<T> Pool;

	private int _totalSpawned = 0;

	private void Awake()
	{
		InitializePool();
	}

	public float GetRandomLiveTime()
	{
		return Random.Range(_minMaxLiveTime.x, _minMaxLiveTime.y);
	}

	protected virtual void OnGetFromPool(T poolObj)
	{
		poolObj.transform.position = GetSpawnPosition();
		poolObj.transform.rotation = GetSpawnRotation();
		poolObj.StartReturned += OnPoolReturnStartDelay;
		poolObj.gameObject.SetActive(true);
		_totalSpawned++;

		InvokeStateUpdate(_totalSpawned, Pool.CountAll, Pool.CountActive);
	}

	protected virtual void OnReleaseToPool(T poolObj)
	{
		if (poolObj == null || poolObj.gameObject == null || !poolObj)
		{
			Debug.LogWarning("Attempted to release a destroyed or null object.");
			return;
		}

		poolObj.OnReturn();
		poolObj.StartReturned -= OnPoolReturnStartDelay;
		poolObj.gameObject.SetActive(false);

		InvokeStateUpdate(_totalSpawned, Pool.CountAll, Pool.CountActive);
	}

	protected abstract Vector3 GetSpawnPosition();
	protected virtual Quaternion GetSpawnRotation() => Quaternion.identity;

	private void InitializePool()
	{
		Pool = new ObjectPool<T>(
			createFunc: CreatePooledObject,
			actionOnGet: OnGetFromPool,
			actionOnRelease: OnReleaseToPool,
			actionOnDestroy: OnDestroyPooledObject,
			defaultCapacity: _poolMaxSize,
			maxSize: _poolMaxSize,
			collectionCheck: true
		);
	}

	private T CreatePooledObject()
	{
		T obj = Instantiate(_prefab);
		return obj;
	}

	private void OnDestroyPooledObject(T poolObj)
	{
		if (poolObj != null && poolObj.gameObject != null)
		{
			poolObj.StartReturned -= OnPoolReturnStartDelay;
			Destroy(poolObj.gameObject);

			InvokeStateUpdate(_totalSpawned, Pool.CountAll, Pool.CountActive);
		}
	}

	private void OnPoolReturnStartDelay(PooledObject poolObj, float liveTime = 0)
	{
		if (liveTime == 0)
			liveTime = GetRandomLiveTime();

		StartCoroutine(ReturnToPoolAfterTime((T)poolObj, liveTime));
	}

	private IEnumerator ReturnToPoolAfterTime(T poolObj, float time)
	{
		yield return new WaitForSeconds(time);

		if (poolObj != null && poolObj.gameObject != null && Pool != null)
		{
			Pool.Release(poolObj);
		}
	}
}