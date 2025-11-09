using GameOfLifeLibrary;

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
        const int gridConstraint = 5;
        Assert.DoesNotThrow(()=>
        {
            _ = new Game(gridConstraint,cells);
        });
        Assert.Throws<Exception>(()=>
        {
            _ = new Game(cells);
        });
    }
}