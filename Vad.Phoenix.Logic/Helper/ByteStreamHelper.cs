using Vad.Phoenix.Logic.Data;
using Vad.Phoenix.Logic.Data.Tables;
using Vad.Phoenix.Titan.Data;
using Vad.Phoenix.Titan.Logic.DataStream;

namespace Vad.Phoenix.Logic.Helper
{
    public class ByteStreamHelper
    {
        public static void WriteDataReference(ChecksumEncoder encoder, LogicData data)
        {
            if (data != null)
            {
                encoder.WriteInt(data.GetGlobalID());
            }
            else encoder.WriteInt(0);
        }

        public static LogicData? ReadDataReference(ByteStream stream, int ID)
        {
            int id = stream.ReadInt();
            int classID = GlobalID.GetClassId(id);

            if (classID == ID + 1)
            {
                return LogicDataTables.GetDataById(classID);
            }
            return null;
        }

        public static LogicData? ReadDataReference(ByteStream stream)
        {
            int id = stream.ReadInt();
            return LogicDataTables.GetDataById(id);
        }
    }
}