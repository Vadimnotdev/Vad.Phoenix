using Vad.Phoenix.Titan.Logic.Debug;

namespace Vad.Phoenix.Titan.Logic.CSV
{
    public class CSVTable
    {
        private CSVNode _node;

        private readonly List<string> _columnNameList;
        private readonly List<CSVColumn> _columnList;
        private readonly List<CSVRow> _rowList;

        private readonly int _size;

        public CSVTable(CSVNode node, int size)
        {
            _columnNameList = [];
            _columnList = [];
            _rowList = [];

            _node = node;
            _size = size;
        }

        public void AddColumn(string name)
        {
            _columnNameList.Add(name);
        }

        public void AddColumnType(int type)
        {
            _columnList.Add(new CSVColumn(type, _size));
        }

        public void AddAndConvertValue(string value, int idx)
        {
            CSVColumn column = _columnList[idx];

            if (!string.IsNullOrEmpty(value))
            {
                switch (column.GetType())
                {
                    case 0:
                        column.AddStringValue(value);
                        break;
                    case 1:
                        column.AddIntegerValue(int.Parse(value));
                        break;
                    case 2:
                        if (bool.TryParse(value, out bool booleanValue)) column.AddBooleanValue(booleanValue);
                        else
                        {
                            Debugger.Error($"CSVTable::addAndConvertValue invalid value '{value}' in Boolean column '{_columnNameList[idx]}', {GetFileName()}");

                            column.AddBooleanValue(false);
                        }
                        break;
                }
            }
            else column.AddEmptyValue();
        }

        public string GetFileName()
        {
            return _node.GetName();
        }

        public string GetColumnName(int idx)
        {
            return _columnNameList[idx];
        }

        public int GetColumnCount()
        {
            return _columnNameList.Count;
        }

        public string GetValueAt(int columnIdx, int idx)
        {
            if (columnIdx != -1)
                return _columnList[columnIdx].GetStringValue(idx);

            return string.Empty;
        }

        public string GetValue(string name, int idx)
        {
            return GetValueAt(_columnNameList.IndexOf(name), idx);
        }

        public int GetColumnIndexByName(string name)
        {
            return _columnNameList.IndexOf(name);
        }

        public int GetIntegerValueAt(int columnIdx, int idx)
        {
            if (columnIdx != -1)
                return _columnList[columnIdx].GetIntegerValue(idx);

            return 0;
        }

        public int GetIntegerValue(string name, int idx)
        {
            return GetIntegerValueAt(_columnNameList.IndexOf(name), idx);
        }

        public bool GetBooleanValueAt(int columnIdx, int idx)
        {
            if (columnIdx != -1) return _columnList[columnIdx].GetBooleanValue(idx);
            return false;
        }

        public bool GetBooleanValue(string name, int idx)
        {
            return GetBooleanValueAt(_columnNameList.IndexOf(name), idx);
        }

        public CSVRow GetRowAt(int idx)
        {
            return _rowList[idx];
        }

        public void AddRow(CSVRow row)
        {
            _rowList.Add(row);
        }

        public int GetColumnRowCount()
        {
            return _columnList[0].GetSize();
        }

        public int GetColumnType(int idx)
        {
            return _columnList[idx].GetType();
        }

        public int GetRowCount()
        {
            return _rowList.Count;
        }

        public int GetArraySizeAt(CSVRow row, int columnIdx)
        {
            if (_rowList.Count > 0)
            {
                int rowIdx = _rowList.IndexOf(row);

                if (rowIdx != -1)
                {
                    CSVColumn column = _columnList[columnIdx];
                    return column.GetArraySize(_rowList[rowIdx].GetRowOffset(), rowIdx + 1 >= _rowList.Count ? column.GetSize() : _rowList[rowIdx + 1].GetRowOffset()); // refactor this shit
                }
            }

            return 0;
        }

        public void SetStringValueAt(string value, int cIdx, int idx)
        {
            _columnList[cIdx].SetStringValue(value, idx);
        }

        public void CreateRow()
        {
            _rowList.Add(new CSVRow(this));
        }

        public void ColumnNamesLoaded()
        {
            _columnList.EnsureCapacity(_columnNameList.Count);
        }

        public void ValidateColumnTypes()
        {
            if (_columnNameList.Count != _columnList.Count)
            {
                Debugger.Log($"CSVTable::ValidateCollumnType(): Column name count {_columnNameList.Count}, column type count {_columnList.Count}, file {GetFileName()}");
            }
        }
    }
}