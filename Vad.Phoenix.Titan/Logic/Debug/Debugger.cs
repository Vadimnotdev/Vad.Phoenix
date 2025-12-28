namespace Vad.Phoenix.Titan.Logic.Debug
{
    public class Debugger
    {
        public static void Error(string error)
        {
            Console.WriteLine($"[Error]: {error}");
        }

        public static void Warning(string warning)
        {
            Console.WriteLine($"[Warning]: {warning}");
        }

        public static void Log(string log)
        {
            Console.WriteLine($"[Log]: {log}");
        }

    }
}
