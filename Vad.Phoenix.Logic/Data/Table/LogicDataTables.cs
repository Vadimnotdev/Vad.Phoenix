using System.Data;
using Vad.Phoenix.Logic.Data;
using Vad.Phoenix.Logic.Data.Table;
using Vad.Phoenix.Titan.Data;
using Vad.Phoenix.Titan.Logic.CSV;
using Vad.Phoenix.Titan.Logic.Debug;

namespace Vad.Phoenix.Logic.Data.Tables
{

    public class LogicDataTables
    {
        private static LogicDataTable[] _tables = new LogicDataTable[30];

        private const int COUNT = 52;

        public static LogicData? GetDataById(int id)
        {
            int classID = GlobalID.GetClassId(id) - 1;

            if (classID >= 0 && classID < COUNT && _tables[classID] != null)
            {
                return _tables[classID].GetItemById(id);
            }
            return null;
        }
    }
}