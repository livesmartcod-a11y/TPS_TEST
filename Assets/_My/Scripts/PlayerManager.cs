using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController controller;
    private Animator anim;


    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam; // 조준시 확대되는 카메라 불러오기 
    [SerializeField]
    private GameObject aimImage; // 조준점 이미지 
    [SerializeField]
    private GameObject aimObj; // 아군 식별용 
    [SerializeField]
    private float aimObjDis = 10f; // 아군 식별용 테스트 오브젝트 거리
    [SerializeField]
    private LayerMask targetLayer; // 타겟 설정을 위한 레이어 
    [Header("IK")]
    [SerializeField]
    private Rig handRig;
    [SerializeField]
    private Rig aimRig;

    [Header("Weapon Sound Effect")]
    [SerializeField]
    private AudioClip shootingSound;
    [SerializeField]
    private AudioClip[] reloadSound;
    private AudioSource weaponSound;





    private Enemy enemy;



    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AimCheck();
    }

    private void AimCheck()
    {
        Vector3 targetPosition;
        Transform camTransform = Camera.main.transform;
        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
        {
            // Debug.Log("Name : " + hit.transform.gameObject.name);
            targetPosition = hit.point;
            aimObj.transform.position = hit.point;

            enemy = hit.collider.gameObject.GetComponent<Enemy>();
        }
        else 
        {
            targetPosition = camTransform.position + camTransform.forward * aimObjDis;
            aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
            enemy = null;
        }

        // 조준 시에만 카메라와 조준점 UI 활성화
        AimControll(input.aim);

        // 조준 또는 발사 시 플레이어가 총구 방향을 보도록 회전하고, IK Rig와 애니메이션 레이어 활성화
        if (input.aim || input.shoot)
        {
            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);
            SetRigWeight(1);
            anim.SetLayerWeight(1, 1);
        }
        else // 조준도 발사도 하지 않을 때
        {
            SetRigWeight(0);
            anim.SetLayerWeight(1, 0);
        }

        if (input.shoot)
        {
            anim.SetBool("Shoot", true);
            GameManager.instance.Shooting(targetPosition, enemy, weaponSound, shootingSound);
        }
        else
        {
            anim.SetBool("Shoot", false);
        }
    }

    private void AimControll(bool isCheck)
    {
        aimCam.gameObject.SetActive(isCheck); // 확대
        aimImage.gameObject.SetActive(isCheck); // 조준점 이미지 생성 
                                                // controller.isAimMove = isCheck; // 조준 할때 & 풀 때, 걷게만 하거나 다시 뛸 수 있게 하는 코드인데, 나는 이거 안쓸거임. 
    }

    private void SetRigWeight(float weight)
    {
        if (aimRig != null) aimRig.weight = weight;
        if (handRig != null) handRig.weight = weight;
    }

    public void ReroadWeaponClip()
    {
        GameManager.instance.ReroadClip();
        // PlayWeaponSound(reroadSound[0]);
    }

    private void PlayWeaponSound(AudioClip sound)
    {
        weaponSound.clip = sound;
        weaponSound.Play();
    }


}
