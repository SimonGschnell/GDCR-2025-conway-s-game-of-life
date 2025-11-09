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
        var cell = new Cell(1, 1);
        List<Cell> cells = [new(0,0), new(1,1), new(2,2)];
        var game = new Game(cells);
        var neighbourCells = game.GetNeighbourCells(cell);
        Assert.That(neighbourCells, Contains.Item(cells[0]));
        Assert.That(neighbourCells, Contains.Item(cells[2]));
        Assert.That(neighbourCells, Does.Not.Contain(cell));
    }
    
    [Test]
    public void If_Cell_Has_2_Or_3_Neighbours_It_Survives()
    {
        List<Cell> cells = [new(0,0),new(1,1), new(2,2)];
        var game = new Game(cells);
        var cellState = game.IsAliveInNextGeneration(new Cell(1,1));
        Assert.That(cellState, Is.True);
    }
    
    [Test]
    public void If_Dead_Cell_Has_2_Neighbours_It_Stays_Dead()
    {
        List<Cell> cells = [new(0,0), new(2,2)];
        var game = new Game(cells);
        var cellState = game.IsAliveInNextGeneration(new Cell(1,1));
        Assert.That(cellState, Is.False);
    }
    
    [Test]
    public void If_Cell_Has_Less_Than_2_Neighbours_It_Dies()
    {
        List<Cell> cells = [new(0,0),new(1,1)];
        var game = new Game(cells);
        var cellState = game.IsAliveInNextGeneration(new Cell(1,1));
        Assert.That(cellState, Is.False);
    }
    
    [Test]
    public void If_Cell_Has_4_Or_More_Neighbours_It_Dies()
    {
        List<Cell> cells = [new(0,0),new(1,1),new(2,2),new(2,1),new(1,2)];
        var game = new Game(cells);
        var cellState = game.IsAliveInNextGeneration(new Cell(1,1));
        Assert.That(cellState, Is.False);
    }
    
    [Test]
    public void If_Cell_Has_Exactly_3_Neighbours_It_Comes_To_Life()
    {
        List<Cell> cells = [new(0,0),new(2,2),new(2,1)];
        var game = new Game(cells);
        var cellState = game.IsAliveInNextGeneration(new Cell(1,1));
        Assert.That(cellState, Is.True);
    }
    
    [Test]
    public void Next_Generation_Is_Created_Correctly()
    {
        List<Cell> cells = [new(0,0),new(2,2),new(2,1)];
        var gridConstraint = 3;
        var game = new Game(gridConstraint, cells);
        Game nextGeneration = game.NextGeneration();
        Assert.That(nextGeneration.GetLiveCells(), Contains.Item(new Cell(1,1)));
    }
}

public class Game(List<Cell> cells)
{
    public Game(int gridConstraint, List<Cell> list) : this(list)
    {
        GridConstraint = gridConstraint;
    }

    public int GridConstraint { get; } = 3;
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
            return gamecell != cell && (xDeltas || yDeltas);
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

public record Cell(int X, int Y);