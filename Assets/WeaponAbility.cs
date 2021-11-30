using UnityEngine;

public class WeaponAbility : AbilityDragHandler
{
    public WeaponScript weapon;
    [SerializeField] float duration = 0;
    GameObject marker;
    private void Awake()
    {
        marker = new GameObject("Temp");
    }
    // Start is called before the first frame update
    public override void SpawnAbility(Vector2 target)
    {
        CancelInvoke();
        marker.transform.position = target;
        weapon.currentTarget = marker;
        Invoke("CancelTarget", duration);
    }
    void CancelTarget()
    {
        weapon.currentTarget = null;
    }
}
