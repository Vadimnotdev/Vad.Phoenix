namespace Vad.Phoenix.Titan.Logic.CSV
{
    public class CSVRow
    {
        private CSVTable _table;
        private int _rowOffset;

        public CSVRow() { }

        public CSVRow(CSVTable table)
        {
            _table = table;
            _rowOffset = table.GetColumnRowCount();
        }

        public string GetValueAt(int columnID, int ID)
        {
            return _table.GetValueAt(columnID, _rowOffset + ID);
        }

        public int GetLongestArraySize()
        {
            int longestArraySize = 1;

            for (int i = _table.GetColumnCount() - 1; i > 0; i--)
            {
                int arraySizeAt = _table.GetArraySizeAt(this, i);
                if (arraySizeAt > longestArraySize)
                    longestArraySize = arraySizeAt;
            }

            return longestArraySize;
        }

        public int GetColumnCount()
        {
            return _table.GetColumnCount();
        }

        public int GetArraySizeAt(int ID)
        {
            return _table.GetArraySizeAt(this, ID);
        }

        public string GetValue(string name, int ID)
        {
            return _table.GetValue(name, _rowOffset + ID);
        }

        public string GetClampedValue(string name, int ID)
        {
            int columnID = _table.GetColumnIndexByName(name);

            if (columnID != -1)
            {
                int arraySize = _table.GetArraySizeAt(this, columnID);

                if (arraySize >= 1 && arraySize <= ID) ID = arraySize - 1;

                return _table.GetValueAt(columnID, _rowOffset + ID);
            }

            return string.Empty;
        }

        public int GetColumnIndexByName(string name)
        {
            return _table.GetColumnIndexByName(name);
        }

        public int GetIntegerValueAt(int cID, int ID)
        {
            return _table.GetIntegerValueAt(cID, _rowOffset + ID);
        }

        public int GetIntegerValue(string name, int ID)
        {
            return _table.GetIntegerValue(name, _rowOffset + ID);
        }

        public int GetClampedIntegerValue(string name, int ID)
        {
            int columnID = _table.GetColumnIndexByName(name);

            if (columnID != -1)
            {
                int arraySize = _table.GetArraySizeAt(this, columnID);

                if (arraySize >= 1 && arraySize <= ID)
                    ID = arraySize - 1;

                return _table.GetIntegerValueAt(columnID, _rowOffset + ID);
            }

            return 0;
        }

        public bool GetBooleanValueAt(int cID, int ID)
        {
            return _table.GetBooleanValueAt(cID, _rowOffset + ID);
        }

        public bool GetBooleanValue(string name, int ID)
        {
            return _table.GetBooleanValue(name, _rowOffset + ID);
        }

        public bool GetClampedBooleanValue(string name, int ID)
        {
            int columnID = _table.GetColumnIndexByName(name);

            if (columnID != -1)
            {
                int arraySize = _table.GetArraySizeAt(this, columnID);

                if (arraySize >= 1 && arraySize <= ID)
                    ID = arraySize - 1;

                return _table.GetBooleanValueAt(columnID, _rowOffset + ID);
            }
            return false;
        }

        public int GetArraySize(string column)
        {
            int columnIndex = GetColumnIndexByName(column);

            if (columnIndex == -1) return 0;

            return _table.GetArraySizeAt(this, columnIndex);
        }

        public int GetRowOffset()
        {
            return _rowOffset;
        }

        public int GetRowCount()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return _table.GetValueAt(0, _rowOffset);
        }

        public CSVTable GetTable()
        {
            return _table;
        }

        public void SetStringValueAt(string value, int cId, int id)
        {
            _table.SetStringValueAt(value, cId, id);
        }
    }
}