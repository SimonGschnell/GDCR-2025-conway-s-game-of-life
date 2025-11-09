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
        var cell = new Cell(0, 1);
        List<Cell> cells = [new(0,0), new(0,1), new(2,2), new(2,1)];
        var game = new Game(cells);
        var neighbourCells = game.GetNeighbourCells(cell);
        Assert.That(neighbourCells, Contains.Item(new Cell(0,0)));
        Assert.That(neighbourCells, Does.Not.Contain(cell));
        Assert.That(neighbourCells, Has.Count.EqualTo(1));
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
        const int gridConstraint = 3;
        var game = new Game(gridConstraint, cells);
        var nextGeneration = game.NextGeneration();
        Assert.That(nextGeneration.GetLiveCells(), Contains.Item(new Cell(1,1)));
        Assert.That(nextGeneration.GetLiveCells(), Has.Count.EqualTo(1));
    }
    
    [Test]
    public void Next_Generation_Is_Created_Correctly_After_2_Generations()
    {
        List<Cell> cells = [new(0,0),new(2,2),new(2,1)];
        const int gridConstraint = 3;
        var game = new Game(gridConstraint, cells);
        var firstGeneration = game.NextGeneration();
        var secondGeneration = firstGeneration.NextGeneration();
        Assert.That(secondGeneration.GetLiveCells(), Has.Count.EqualTo(0));
    }
    
    [Test]
    public void Not_Possible_To_Create_Cells_Outside_Of_GridConstraint()
    {
        List<Cell> cells = [new(0,0),new(2,2),new(2,1), new(4,4)];
        const int gridConstraint = 4;
        Assert.Throws<Exception>(()=>
        {
            _ = new Game(gridConstraint,cells);
        });
    }
}

public class Game
{
    public Game(int gridConstraint, List<Cell> list) : this(list)
    {
        GridConstraint = gridConstraint;
    }

    public Game(List<Cell> cells)
    {
        if (CellsOutOfBoundary(cells))
        {
            throw new Exception("Cells are out of boundary");
        }
        Cells = cells;
    }

    private bool CellsOutOfBoundary(List<Cell> cells)
    {
        return cells.Where(cell => cell.X >= GridConstraint || cell.Y >= GridConstraint).ToList().Count > 0;
    }

    private int GridConstraint { get; } = 3;
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

public record Cell(int X, int Y);