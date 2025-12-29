using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [UnitHeaderInspectable("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;

    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFX;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shooting(Vector3 targetPosition)
    {
        Instantiate(weaponFlashFX, bulletPoint);
        Instantiate(bulletCaseFX, bulletCasePoint);

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;
        Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim));
    }




}
