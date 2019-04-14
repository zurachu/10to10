using System;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public virtual void Move(Vector3 mouseWorldPosition)
    {
    }

    public void CheckResult(List<SpriteRenderer> counters, Action<int> onCount)
    {
        var firstHalfCount = 0;

        foreach (var counter in counters)
        {
            if (FirstHalfContains(counter))
            {
                counter.color = Color.blue;
                firstHalfCount++;
            }
            else
            {
                counter.color = Color.red;
            }
        }

        onCount?.Invoke(firstHalfCount);
    }

    protected virtual bool FirstHalfContains(SpriteRenderer counter)
    {
        return true;
    }
}
