using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyBlueprint : ScriptableObject
{
    new public string name;
    public GameObject prefab;
    public float minSpeed, maxSpeed;
    public int minValue, maxValue;
    public float healthDelta;
    public float chance;
    public int unlockWave;
}