using UnityEngine;

public interface IInteractable
{
    // ��ȣ �ۿ� �� ������ ��� ����
    public string GetInteractionDescription();


    public void OnInteract();
}
