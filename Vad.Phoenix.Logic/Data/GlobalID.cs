namespace Vad.Phoenix.Logic.Data
{
    public class GlobalID
    {
        public static int CreateGlobalID(int classID, int instanceID)
        {
            return classID <= 0 ? 1000000 + instanceID : classID * 1000000 + instanceID;
        }
        public static int GetClassId(int globalID)
        {
            return globalID / 1000000;
        }
        public static int GetInstanceId(int globalID)
        {
            return globalID % 1000000;
        }
    }
}