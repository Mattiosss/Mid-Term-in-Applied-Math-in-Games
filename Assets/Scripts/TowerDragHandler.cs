using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject towerPrefab;
    private GameObject towerInstance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        towerInstance = Instantiate(towerPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        towerInstance.transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        towerInstance = null;
    }
}
