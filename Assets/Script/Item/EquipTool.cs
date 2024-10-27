using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;        // 공격 속도 (공격 간격을 나타내는 시간 간격)
    private bool attacking;          // 현재 공격 중인지 여부를 나타내는 변수
    public float attackDistance;     // 공격할 수 있는 최대 거리

    [Header("Resource Gathering")]
    public bool doesGatherResources; // 자원을 채집할 수 있는지 여부를 나타내는 변수

    [Header("Combat")]
    public bool doesDealDamage;      // 대상을 공격하여 피해를 줄 수 있는지 여부를 나타내는 변수
    public int damage;               // 공격이 가하는 피해량 (데미지)

    private Animator animator;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        { 
            if(doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
        }
    }

}
