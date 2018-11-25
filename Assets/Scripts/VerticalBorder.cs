using UnityEngine;

public class VerticalBorder : Border
{
    public override void Move(Vector3 mouseWorldPosition)
    {
        var position = transform.position;
        position.x = mouseWorldPosition.x;
        transform.position = position;
    }

    protected override bool FirstHalfContains(SpriteRenderer counter)
    {
        return counter.transform.position.x < transform.position.x;
    }
}
