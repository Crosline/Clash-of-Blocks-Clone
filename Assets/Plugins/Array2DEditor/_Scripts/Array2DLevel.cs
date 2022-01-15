using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    public class Array2DLevel : Array2D<GridType>
    {
        [SerializeField]
        CellRowExampleEnum[] cells = new CellRowExampleEnum[Consts.defaultGridSize];

        protected override CellRow<GridType> GetCellRow(int idx)
        {
            return cells[idx];
        }
    }
    
    [System.Serializable]
    public class CellRowExampleEnum : CellRow<GridType> { }
}
