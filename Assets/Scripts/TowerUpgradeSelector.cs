using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUpgradeSelector : MonoBehaviour
{
    public static TowerUpgradeSelector Instance;

    private bool selectingTower = false;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableTowerSelection()
    {
        selectingTower = true;
        Debug.Log("Click on a tower to upgrade it.");
    }

    void Update()
    {
        if (selectingTower && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return; 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                TowerUpgrade tower = hit.collider.GetComponent<TowerUpgrade>();
                if (tower != null)
                {
                    tower.UpgradeTower();
                    selectingTower = false;
                }
            }
        }
    }
}
