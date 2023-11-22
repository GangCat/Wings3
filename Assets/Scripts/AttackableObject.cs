using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObject : MonoBehaviour
{
    protected virtual bool AttackDmg(Collider _other)
    {
        IDamageable damagable = FindInterfaceInParents<IDamageable>(_other.transform);
        if (damagable != null)
        {
            damagable.GetDamage(dmg);
            return true;
        }
        return false;
    }

    protected virtual bool KnockBack(Collider _other)
    {
        IKnockBackable damagable = FindInterfaceInParents<IKnockBackable>(_other.transform);
        if (damagable != null)
        {
            damagable.KnockBack(gameObject);
            return true;
        }
        return false;
    }

    protected T FindInterfaceInParents<T>(Transform currentTransform) where T : class
    {
        // 현재 오브젝트에서 특정 인터페이스를 구현한 컴포넌트 찾기 시도
        T interfaceComponent = currentTransform.GetComponent(typeof(T)) as T;

        // 컴포넌트를 찾았으면 반환
        if (interfaceComponent != null)
        {
            return interfaceComponent;
        }
        // 컴포넌트를 찾지 못했고 부모 오브젝트가 있는 경우 부모로 이동하여 재귀 호출
        else if (currentTransform.parent != null)
        {
            return FindInterfaceInParents<T>(currentTransform.parent);
        }
        // 컴포넌트를 찾지 못하고 부모 오브젝트도 없는 경우 null 반환
        else
        {
            return null;
        }
    }

    [SerializeField]
    protected float dmg;

    protected bool isFirstTrigger = true;
}
