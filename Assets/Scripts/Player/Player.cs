using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition playerCondition;
    private void Awake()
    {
        CharacterManager.Instance.Player = this;

        controller = GetComponent<PlayerController>();

        playerCondition = GetComponent<PlayerCondition>();
    }
}
