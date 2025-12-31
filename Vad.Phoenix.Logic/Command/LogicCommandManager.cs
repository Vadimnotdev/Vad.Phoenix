using Vad.Phoenix.Titan.Logic.DataStream;
using Vad.Phoenix.Titan.Logic.Debug;
using Vad.Phoenix.Logic.Command.Resources;
using Vad.Phoenix.Logic.Command.LogicGame;
using System.Runtime.InteropServices;

namespace Vad.Phoenix.Logic.Command
{
    public class LogicCommandManager
    {
        public static LogicCommand DecodeCommand(ByteStream stream)
        {
            int commandType = stream.ReadInt();
            LogicCommand command = CreateCommand(commandType);
            Console.WriteLine($"Received command with type {commandType}");

            if (command == null)
            {
                Debugger.Error(
                    $"LogicCommandManager.DecodeCommand - command {commandType} is NULL!"
                );
                return null;
            }

            command.Decode(stream);
            return command;
        }

        public static void EncodeCommand(ChecksumEncoder encoder, LogicCommand command)
        {
            encoder.WriteInt(command.GetCommandType());
            command.Encode(encoder);
        }

        public static LogicCommand CreateCommand(int type)
        {
            switch (type)
            {
                case 3:
                    return new LogicDiamondsAddedCommand();
                case 8:
                    return new LogicChangeGameStateCommand();
                case 500:
                    return new LogicDragCommand();
                case 501:
                    return new LogicContinueGameCommand();
                case 502:
                    return new LogicFailLevelCommand();
                case 504:
                    return new LogicLevelCompleteCommand();
                case 506:
                    return new LogicLevelStartedCommand();
                case 507:
                    return new LogicReleaseDragCommand();
                case 510:
                    return new LogicSwapHeroesCommand();
                case 511:
                    return new LogicDragHeroCommand();
                case 514:
                    return new LogicUseBoostCommand();
                case 515:
                    return new LogicResetNewItemCount();
                case 516:
                    return new LogicBuyBoostCommand();
                case 518:
                    return new LogicOpenGateCommand();
                case 519:
                    return new LogicRetryLevelCommand();
                case 600:
                    return new LogicResourceCostingCommand();
                case 700:
                    return new LogicBuyResourcesCommand();
                default:
                    return null;
            }
        }

    }
}
