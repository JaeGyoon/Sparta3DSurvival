using UnityEngine;

public interface IInteractable
{
    // 상호 작용 시 나오는 대상 설명문
    public string GetInteractionDescription();


    public void OnInteract();
}
