namespace GameOfLifeTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetAliveCellsContainsItems()
    {
        List<Cell> cells = [new(0,0)];
        var game = new Game(cells);
        var allAliveCells = game.GetLiveCells();
        Assert.That(allAliveCells, Is.Not.Empty);
    }
    
    [Test]
    public void GetNeighbourCellsOfACell()
    {
        List<Cell> cells = [new(0,0),new(2,2)];
        var game = new Game(cells);
        var neighbourCells = game.GetNeighbourCells(new Cell(1,1));
        Assert.That(neighbourCells, Contains.Item(cells[0]));
        Assert.That(neighbourCells, Contains.Item(cells[1]));
    }
    
    [Test]
    public void If_Cell_Has_2_Or_3_Neighbours_It_Lives()
    {
        List<Cell> cells = [new(0,0),new(1,1), new(2,2)];
        var game = new Game(cells);
        var cellState = game.IsAlive(new Cell(1,1));
        Assert.That(cellState, Is.True);
    }
    
    [Test]
    public void If_Cell_Has_Less_Than_2_Neighbours_It_Dies()
    {
        List<Cell> cells = [new(0,0),new(1,1)];
        var game = new Game(cells);
        var cellState = game.IsAlive(new Cell(1,1));
        Assert.That(cellState, Is.False);
    }
}

public class Game(List<Cell> cells)
{
    private List<Cell> Cells { get; } = cells;

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
            return xDeltas ||
                   yDeltas;
        }).ToList();
    }

    public bool IsAlive(Cell cell)
    {
        var cellNeighbours = GetNeighbourCells(cell);
        return cellNeighbours.Count is > 2 and < 4;
    }
}

public record Cell(int X, int Y);