using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition playerCondition;

    public ItemData itemData;

    public Action addItem;

    public Transform itemDropPoint;
    private void Awake()
    {
        CharacterManager.Instance.Player = this;

        controller = GetComponent<PlayerController>();

        playerCondition = GetComponent<PlayerCondition>();
    }
}
