using UnityEngine;

public class PooledBomb : PooledObject
{
	[SerializeField] private Exploser _exploser;
	[SerializeField] private MaterialFade _fade;

	public void OnSpawn(float liveDuration)
	{
		_fade.StartFade(liveDuration);
		ReturnToPool(liveDuration);
	}

	public override void OnReturn()
	{
		_exploser.Explode();
	}
}
