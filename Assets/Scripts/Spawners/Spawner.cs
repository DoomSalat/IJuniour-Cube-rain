using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System.Collections;

public abstract class Spawner<T> : MonoBehaviour where T : PooledObject
{
	[SerializeField] private T _prefab;
	[SerializeField] private Vector2Int _minMaxLiveTime = new Vector2Int(2, 5);
	[SerializeField][Min(1)] private int _poolMaxSize = 10;

	[Header("UI")]
	[SerializeField] private TextMeshProUGUI _totalSpawnedText;
	[SerializeField] private TextMeshProUGUI _createdObjectsText;
	[SerializeField] private TextMeshProUGUI _activeObjectsText;

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

	protected virtual void OnGetFromPool(T poolObj)
	{
		poolObj.transform.position = GetSpawnPosition();
		poolObj.transform.rotation = GetSpawnRotation();
		poolObj.StartReturned += OnPoolReturnStartDelay;
		poolObj.gameObject.SetActive(true);
		_totalSpawned++;

		UpdateUI();
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

		UpdateUI();
	}

	private void OnDestroyPooledObject(T poolObj)
	{
		if (poolObj != null && poolObj.gameObject != null)
		{
			poolObj.StartReturned -= OnPoolReturnStartDelay;
			Destroy(poolObj.gameObject);
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

	protected abstract Vector3 GetSpawnPosition();
	protected virtual Quaternion GetSpawnRotation() => Quaternion.identity;

	private void UpdateUI()
	{
		if (_totalSpawnedText != null)
			_totalSpawnedText.text = $"Total Spawned: {_totalSpawned}";

		if (_createdObjectsText != null)
			_createdObjectsText.text = $"Created Objects: {Pool.CountAll}";

		if (_activeObjectsText != null)
			_activeObjectsText.text = $"Active Objects: {Pool.CountActive}";
	}
}