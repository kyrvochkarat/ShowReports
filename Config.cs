using System.ComponentModel;
using Exiled.API.Interfaces;

namespace ReportBroadcast
{
    public class Config : IConfig
    {
        [Description("Включен ли плагин")]
        public bool IsEnabled { get; set; } = true;

        [Description("Включен ли дебаг режим")]
        public bool Debug { get; set; } = false;

        [Description("Длительность броадкаста в секундах")]
        public ushort BroadcastDuration { get; set; } = 10;

        [Description("Показывать ли причину репорта")]
        public bool ShowReason { get; set; } = true;

        [Description("Логировать ли репорты в консоль сервера")]
        public bool LogToConsole { get; set; } = true;
    }
}