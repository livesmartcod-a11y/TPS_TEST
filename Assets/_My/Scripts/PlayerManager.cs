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


    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AimCheck();
    }

    private void AimCheck()
    {
        if (input.aim) // 조준할 때 (오른쪽 클릭)
        {
            AimControll(true);

            anim.SetLayerWeight(1,1); // 1번째 인자값: 몇 번째의 layer인가, 2번째 인자값: weight 값 설정 

            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform;
            RaycastHit hit;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                // Debug.Log("Name : " + hit.transform.gameObject.name);
                targetPosition = hit.point;
                aimObj.transform.position = hit.point;
            }
            else
            {
                targetPosition = camTransform.position + camTransform.forward * aimObjDis;
                aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
            }


            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);

            SetRigWeight(1);


            if (input.shoot)
            {
                anim.SetBool("Shoot", true);
            }
            else
            {
                anim.SetBool("Shoot", false);
            }


        }

        else // 오른쪽 클릭 (조준) X 
        {
            AimControll(false);
            SetRigWeight(0);
            anim.SetLayerWeight(1,0);
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
        aimRig.weight = weight;
        handRig.weight = weight;
          if (aimRig != null) aimRig.weight = weight;
        if (handRig != null) handRig.weight = weight;
    }



}
