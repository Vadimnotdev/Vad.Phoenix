using Vad.Phoenix.Logic.Data;
using Vad.Phoenix.Titan.Data;
using Vad.Phoenix.Titan.Logic.CSV;
using Vad.Phoenix.Titan.Logic.Debug;

namespace Vad.Phoenix.Logic.Data.Table
{
    public class LogicDataTable
    {

        protected List<LogicData> _items;
        protected CSVTable _table;

        private int _tableIndex;

        private bool _isLoaded;

        public LogicDataTable(CSVTable table, int index)
        {
            _table = table;
            _tableIndex = index;
            _items = [];

            LoadTable();
        }

        public void LoadTable()
        {
            int rowCount = _table.GetRowCount();

            for (int i = 0; i < rowCount; i++)
            {
                _items.Add(CreateItem(_table.GetRowAt(i)));
            }
        }

        public LogicData? CreateItem(CSVRow row)
        {
            LogicData item = null;

            switch (_tableIndex)
            {
                // ....
                // case 0:
                //     item = new Logic....Data(row, this);
                //     break;

                default:
                    Debugger.Error($"LogicDataTable::CreateItem() invaild table id: {_tableIndex}");
                    break;
            }
            return item;
        }

        public virtual void CreateReferences()
        {
            if (!_isLoaded)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    _items[i].CreateReferences();
                }
                _isLoaded = true;
            }
        }

        public LogicData? GetItemById(int id)
        {
            int instanceID = GlobalID.GetInstanceId(id);

            if (instanceID <= 0 || instanceID >= _items.Count)
            {
                Debugger.Warning("LogicDataTable::GetItemById() - instance id out of bounds !");
                return null;
            }
            else return _items[instanceID];
        }

        public int GetTableIndex()
        {
            return _tableIndex;
        }

        public int GetItemCount()
        {
            return _items.Count;
        }

        public LogicData GetDataByName(string name, LogicData? caller)
        {
            if (!string.IsNullOrEmpty(name))
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    LogicData data = _items[i];

                    if (data.GetName().Equals(name)) return data;
                }

                if (caller != null)
                {
                    Debugger.Warning($"CSV row ({caller.GetName()}) has an invalid reference ({name})");
                }
            }

            return new LogicData();
        }
    }
}