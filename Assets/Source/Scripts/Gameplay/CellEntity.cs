using System;

public abstract class CellEntity
{
    public Type EntityType => GetEntityType();

    protected abstract void CollideWithHead(Snake snake);
    protected abstract Type GetEntityType();
}
