using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
	public delegate void StateInfo(float totalSpawned, float countAll, float countActive);
	public event StateInfo StateUpdate;

	protected void InvokeStateUpdate(float totalSpawned, float countAll, float countActive)
	{
		StateUpdate?.Invoke(totalSpawned, countAll, countActive);
	}
}
