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
        // ���� ������Ʈ���� Ư�� �������̽��� ������ ������Ʈ ã�� �õ�
        T interfaceComponent = currentTransform.GetComponent(typeof(T)) as T;

        // ������Ʈ�� ã������ ��ȯ
        if (interfaceComponent != null)
        {
            return interfaceComponent;
        }
        // ������Ʈ�� ã�� ���߰� �θ� ������Ʈ�� �ִ� ��� �θ�� �̵��Ͽ� ��� ȣ��
        else if (currentTransform.parent != null)
        {
            return FindInterfaceInParents<T>(currentTransform.parent);
        }
        // ������Ʈ�� ã�� ���ϰ� �θ� ������Ʈ�� ���� ��� null ��ȯ
        else
        {
            return null;
        }
    }

    [SerializeField]
    protected float dmg;

    protected bool isFirstTrigger = true;
}
