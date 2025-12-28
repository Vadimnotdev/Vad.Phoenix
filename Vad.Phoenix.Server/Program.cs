using System.ComponentModel;
using System.Resources;
using Vad.Phoenix.Server.Network.TCP;
using Vad.Phoenix.Titan.Logic.Debug;

namespace Vad.Phoenix.Server;

class Program
{
    private static readonly TaskCompletionSource<bool> _exitEvent = new TaskCompletionSource<bool>();

    public static async Task Main()
    {
        Program.Init();

        TCPGateway tcpGateway = new();
        tcpGateway.Start();

        await Program.WaitForExit();
        await tcpGateway.Stop();
    }

    private static void Init()
    {
        Program.DisplayBanner();
    }


    private static void DisplayBanner()
    {
        Console.WriteLine("""

    @@@   *@@                    @@@         @@@@@@@@@@                                                @@:              
    @@@   @@*   @@@@@@@@         @@@         @@@   @@@@  @@@   @@@   @@@@@@@.   @@@@@@@@  #@@@@+  @@=       @@@  @@@-   
    @@@* @@@         @@@   @@@@@@@@@        -@@@  *@@@@  @@@   @@@  @@@   @@@. @@@@  +@@@ #@@@@@  @@=  @@-   @@@@@@     
    -@@@@@@     @@@@@@@@  %@@@   @@@        -@@@@@@@@    @@@@@@@@@  @@@   @@@= @@@@@@@@@@ #@@@@@@@@@=  @@     @@@@      
     @@@@@*    @@@#  @@@  @@@.   @@@  @@@   -@@@         @@@   @@@  @@@   @@@* @@@*       #@@  @@@@@=  @@    @@@@@@     
      @@@@      @@@@@@@@   @@@@@@@@@  @@@+  -@@@         @@@   @@@  #@@@@@@@#  :@@@@@@@   .@@   @@@@-  @@   @@@  @@@          
    """);
        Console.WriteLine("By Vad.NotCore.\n");

    }

    private static async Task WaitForExit()
    {
        await Program._exitEvent.Task;
    }

    private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
        Program._exitEvent.TrySetResult(true);
    }
}