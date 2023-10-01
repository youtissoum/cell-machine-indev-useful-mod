using Modding;
using Modding.PublicInterfaces.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Indev2
{
    internal class UsefulCellCellProcessor : TickedCellStepper
    {
        public UsefulCellCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "SelfDestruct";
        public override int CellType => 10;
        public override string CellSpriteIndex => "Useful";

        private IDictionary<int, bool> hasCellMoved = new Dictionary<int, bool>();

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            hasCellMoved.Remove(GetCell1DLoc(ref basicCell));
            return true;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (force <= 0)
                return false;

            var target = cell.Transform.Position + direction.AsVector2Int;
            if (!_cellGrid.InBounds(target))
                return false;
            var targetCell = _cellGrid.GetCell(target);

            if (targetCell is null)
            {
                DoMove(cell.Transform.Position, target);
                _cellGrid.MoveCell(cell, target);
                return true;
            }

            if (!_cellGrid.PushCell(targetCell.Value, direction, force))
                return false;


            DoMove(cell.Transform.Position, target);
            _cellGrid.MoveCell(cell, target);
            return true;
        }

        public override void OnCellInit(ref BasicCell cell)
        {
            hasCellMoved.Add(GetCell1DLoc(ref cell), false);
        }

        public override void Step(CancellationToken ct)
        {
            foreach(var cell in GetCells())
            {
                if (ct.IsCancellationRequested)
                    return;

                var targetCell = _cellGrid.GetCell(cell.Transform.Position);
                if (targetCell == null) continue;

                BasicCell swapCell = (BasicCell)targetCell;
                if (swapCell.Instance.Type != CellType)
                    continue;

                if (!hasCellMoved.ContainsKey(GetCell1DLoc(ref swapCell)))
                    hasCellMoved[GetCell1DLoc(ref swapCell)] = false; // if it's null, assume it's false because yes

                if (!hasCellMoved[GetCell1DLoc(ref swapCell)])
                {
                    Application.Quit();
                }
                hasCellMoved[GetCell1DLoc(ref swapCell)] = false;
            }
        }

        public override void Clear()
        {
            hasCellMoved.Clear();
        }

        private void DoMove(Vector2Int origin, Vector2Int target)
        {
            hasCellMoved[target.x + target.y * _cellGrid.Width] = true;
            hasCellMoved.Remove(origin.x + origin.y * _cellGrid.Width);
        }

        private int GetCell1DLoc(ref BasicCell cell)
        {
            Vector2Int pos = cell.Transform.Position;
            return pos.x + pos.y * _cellGrid.Width;
        }
    }
}
