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
        List<Cell> cells = [new Cell(0,0)];
        Game game = new Game(cells);
        var allAliveCells = game.getAlliveCells();
        Assert.That(allAliveCells.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void GetNeighbourCellsOfACell()
    {
        List<Cell> cells = [new Cell(0,0),new Cell(2,2)];
        Game game = new Game(cells);
        List<Cell> neighbourCells = game.getNeighbourCells(new Cell(1,1));
        Assert.That(neighbourCells, Contains.Item(cells[0]));
        Assert.That(neighbourCells, Contains.Item(cells[1]));
    }
    
    [Test]
    public void AfterCallingTickCellSurvivesIfItHas_2_Or_3_Neighbours()
    {
        List<Cell> cells = [new Cell(0,0),new Cell(1,1), new Cell(2,2)];
        Game game = new Game(cells);
        bool cellState = game.IsAlive(new Cell(1,1));
        Assert.That(cellState, Is.True);
    }
}

public class Game
{
    public List<Cell> Cells { get; }

    public Game(List<Cell> cells)
    {
        Cells = cells;
    }

    public List<Cell> getAlliveCells()
    {
        return Cells;
    }

    public List<Cell> getNeighbourCells(Cell cell)
    {
        return Cells.Where(gamecell =>
        {
            var xDeltas = gamecell.x <= cell.x + 1 && gamecell.x >= cell.x - 1;
            var yDeltas = gamecell.y <= cell.y + 1 && gamecell.y >= cell.y - 1;
            return xDeltas ||
                   yDeltas;
        }).ToList();
    }

    public bool IsAlive(Cell cell)
    {
        var cellNeighbours = getNeighbourCells(cell);
        return cellNeighbours.Count is > 2 and < 4;
    }
}

public record Cell(int x, int y)
{
}