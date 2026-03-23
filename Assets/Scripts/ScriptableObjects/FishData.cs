using UnityEngine;

[CreateAssetMenu(menuName = "FishData")]
public class FishData : ScriptableObject
{
    public float moveSpeed;
    [Space]
    public int health;
    public int damage;
    public int score;
    public int weight;
}
