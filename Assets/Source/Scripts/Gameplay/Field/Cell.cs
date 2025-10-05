public class Cell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public CellView View { get; private set; }
    public CellEntity Entity { get; private set; }
    public bool IsOccupied => Entity != null;

    public Cell(int x, int y, CellView view)
    {
        X = x;
        Y = y;
        View = view;
    }

    public void SetEntity(CellEntity cellEntity)
    {
        Entity = cellEntity;
    }

    public override string ToString()
    {
        return $"Cell: {X}:{Y}, View: {View}, View pos: {View.Position} ";
    }
}
