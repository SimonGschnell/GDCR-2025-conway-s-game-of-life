namespace GameOfLifeLibrary;

public class Game
{
    private const int DefaultGridConstraint = 3;

    public Game(int gridConstraint, List<Cell> cells)
    {
        GridConstraint = gridConstraint;
        if (CellsOutOfBoundary(cells))
        {
            throw new Exception("Cells are out of boundary");
        }
        Cells = cells;
    }

    public Game(List<Cell> cells) : this(DefaultGridConstraint, cells){ }

    private bool CellsOutOfBoundary(List<Cell> cells)
    {
        return cells.Where(cell => cell.X >= GridConstraint || cell.Y >= GridConstraint).ToList().Count > 0;
    }

    private int GridConstraint { get; }
    private List<Cell> Cells { get; }

    public List<Cell> GetLiveCells()
    {
        return Cells;
    }

    public List<Cell> GetNeighbourCells(Cell cell)
    {
        return Cells.Where(gamecell =>
        {
            var xDeltas = gamecell.X <= cell.X + 1 && gamecell.X >= cell.X - 1;
            var yDeltas = gamecell.Y <= cell.Y + 1 && gamecell.Y >= cell.Y - 1;
            return gamecell != cell && xDeltas && yDeltas;
        }).ToList();
    }

    public bool IsAliveInNextGeneration(Cell cell)
    {
        var cellNeighbours = GetNeighbourCells(cell);
        var aliveCell = GetLiveCells().Contains(cell);
        var cellStaysAlive = aliveCell && cellNeighbours.Count is >= 2 and < 4;
        var cellGetsResurrected = cellNeighbours.Count is 3;
        return cellStaysAlive || cellGetsResurrected;
    }

    public Game NextGeneration()
    {
        List<Cell> aliveCells = [];
        for (var x = 0; x < GridConstraint; x++)
        {
            for (var y = 0; y < GridConstraint; y++)
            {
                if (IsAliveInNextGeneration(new Cell(x, y)))
                    aliveCells.Add(new Cell(x,y));
            }
        }
        return new Game(GridConstraint, aliveCells);
    }
}