using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private BoxCollider _positionBox;
	[Space]
	[SerializeField][Min(0)] private float _rateTime = 2;
	[SerializeField] private Vector2Int _minMaxLiveTime = new Vector2Int(2, 5);
	[SerializeField][Min(1)] private int _poolMaxSize = 10;

	private ObjectPool<GameObject> _pool;

	private void Start()
	{
		_pool = new ObjectPool<GameObject>(
			createFunc: () =>
			{
				GameObject obj = Instantiate(_prefab);
				PooledObject pooledObject = obj.AddComponent<PooledObject>();
				pooledObject.Initializate(this);
				return pooledObject.gameObject;
			},
			actionOnGet: (obj) =>
			{
				obj.transform.position = GetRandomPositionInBox();
				obj.SetActive(true);
			},
			actionOnRelease: (obj) => obj.SetActive(false),
			actionOnDestroy: (obj) => Destroy(obj),
			maxSize: _poolMaxSize
		);

		StartCoroutine(SpawnRate());
	}

	public void TimerReturn(GameObject poolObj)
	{
		int liveTime = Random.Range(_minMaxLiveTime.x, _minMaxLiveTime.y);
		StartCoroutine(ReturnToPoolAfterTime(poolObj, liveTime));
	}

	private IEnumerator SpawnRate()
	{
		var rateWait = new WaitForSeconds(_rateTime);

		while (true)
		{
			_pool.Get();

			yield return rateWait;
		}
	}

	private IEnumerator ReturnToPoolAfterTime(GameObject obj, float time)
	{
		yield return new WaitForSeconds(time);

		_pool.Release(obj);
	}

	private Vector3 GetRandomPositionInBox()
	{
		Vector3 center = _positionBox.transform.TransformPoint(_positionBox.center);
		Vector3 size = _positionBox.size;

		float randomX = Random.Range(-size.x / 2, size.x / 2);
		float randomY = Random.Range(-size.y / 2, size.y / 2);
		float randomZ = Random.Range(-size.z / 2, size.z / 2);

		Vector3 localOffset = new Vector3(randomX, randomY, randomZ);
		return center + _positionBox.transform.TransformDirection(localOffset);
	}
}