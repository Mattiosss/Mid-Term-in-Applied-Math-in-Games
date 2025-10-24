using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    private GameObject towerPrefab; 
    private GameObject previewTower;
    private bool isPlacing = false;

    void Update()
    {
        if (!isPlacing) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        previewTower.transform.position = mousePos;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject newTower = Instantiate(towerPrefab, mousePos, Quaternion.identity);
            ArrowTower arrowTower = newTower.GetComponent<ArrowTower>();
            if (arrowTower != null)
        {
            arrowTower.enabled = true; 
        }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(previewTower);
            isPlacing = false;
        }
    }

    public void StartPlacingTower(GameObject selectedTowerPrefab)
    {
        if (selectedTowerPrefab == null)
        {
            Debug.LogWarning("No tower prefab assigned!");
            return;
        }

        towerPrefab = selectedTowerPrefab;
        isPlacing = true;

        previewTower = Instantiate(towerPrefab);
        SpriteRenderer sr = previewTower.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            sr.sortingOrder = 100;
        }
    }
}
