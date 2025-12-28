using Vad.Phoenix.Titan.Logic.CSV;
using Vad.Phoenix.Logic.Data;
using Vad.Phoenix.Logic.Data.Table;

namespace Vad.Phoenix.Titan.Data
{
    public class LogicData
    {
        protected LogicDataTable _table;

        protected CSVRow _row;

        protected int _id;

        protected string _iconSWF = "";
        protected string _iconExportName = "";

        protected string _tid = "";
        protected string _tidInfo = "";

        public LogicData() { }

        public LogicData(CSVRow row, LogicDataTable table)
        {
            _row = row;
            _table = table;

            _id = GlobalID.CreateGlobalID(table.GetTableIndex() + 1, table.GetItemCount());
        }

        public virtual void CreateReferences()
        {
            _iconSWF = _row.GetValue("IconSWF", 0);
            _iconExportName = _row.GetValue("IconExportName", 0);

            _tid = _row.GetValue("TID", 0);
            _tidInfo = _row.GetValue("InfoTID", 0);
        }

        public int GetGlobalID()
        {
            return _id;
        }

        public string GetIconSWF()
        {
            return _iconSWF;
        }

        public string GetIconExportName()
        {
            return _iconExportName;
        }

        public int GetInstanceID()
        {
            return GlobalID.GetInstanceId(_id);
        }

        public string GetName()
        {
            return _row.GetName();
        }

        public override string ToString()
        {
            return GetName();
        }
    }
}