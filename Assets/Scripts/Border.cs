using System;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public virtual void Move(Vector3 mouseWorldPosition)
    {
    }

    public int CheckResult(List<SpriteRenderer> counters)
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

        return firstHalfCount;
    }

    protected virtual bool FirstHalfContains(SpriteRenderer counter)
    {
        return true;
    }
}
