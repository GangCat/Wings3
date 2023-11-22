using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broker
{
    public static void Regist(EPublisherType _publisherType)
    {
        if (arrListSub[(int)_publisherType] != null)
            return;

        arrListSub[(int)_publisherType] = new List<ISubscriber>();
    }

    public static void Subscribe(ISubscriber _sub, EPublisherType _publisherType)
    {
        if (arrListSub[(int)_publisherType] == null)
            Regist(_publisherType);

        arrListSub[(int)_publisherType].Add(_sub);
    }

    public static void UnSubscribe(ISubscriber _sub, EPublisherType _publisherType)
    {
        arrListSub[(int)_publisherType].Remove(_sub);
    }

    public static void AlertMessageToSub(EMessageType _message, EPublisherType _publisherType)
    {
        int count = arrListSub[(int)_publisherType].Count;
        for (int i = 0; i < count; ++i)
            arrListSub[(int)_publisherType][i].ReceiveMessage(_message);
    }

    public static void Clear()
    {
        for(int i = 0; i < arrListSub.Length; ++i)
        {
            if(arrListSub[i] != null)
                arrListSub[i].Clear();
        }
    }

    private static List<ISubscriber>[] arrListSub = new List<ISubscriber>[(int)EPublisherType.LENGTH];
}