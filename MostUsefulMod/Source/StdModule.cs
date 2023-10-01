using System;
using System.Collections.Generic;
using CellEncoding;
using Indev2;
using Modding;


//MUST BE NAMED MOD
public class Mod : IMod
{
    public static Interface Interface;
    public string UniqueName => "Useful Mod";
    public string DisplayName => "The Most Useful Mod In Indev History";
    public string Author => "Youtissoum";
    public string Version => "1.0.0";
    public ILevelFormat LevelFormat => null;
    public string Description => "Vanilla(no mods)";
    public string[] Dependencies => Array.Empty<string>();

    public void Initialize(Interface @interface)
    {
        Interface = @interface;
    }

    public IEnumerable<CellProcessor> GetCellProcessors(ICellGrid cellGrid)
    {
        yield return new BasicCellProcessor(cellGrid);
        yield return new SlideCellProcessor(cellGrid);
        yield return new FreezeProcessor(cellGrid);
        yield return new GeneratorCellProcessor(cellGrid);
        yield return new CWRotateProcessor(cellGrid);
        yield return new CCWRotateProcessor(cellGrid);
        yield return new MoverCellProcessor(cellGrid);
        yield return new WallCellProcessor(cellGrid);
        yield return new TrashCellProcessor(cellGrid);
        yield return new EnemyCellProcessor(cellGrid);
        yield return new UsefulCellCellProcessor(cellGrid);
    }
}
