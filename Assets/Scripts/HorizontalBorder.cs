using UnityEngine;

public class HorizontalBorder : Border
{
    public override void Move(Vector3 mouseWorldPosition)
    {
        var position = transform.position;
        position.y = mouseWorldPosition.y;
        transform.position = position;
    }

    protected override bool FirstHalfContains(SpriteRenderer counter)
    {
        return counter.transform.position.y > transform.position.y;
    }
}
