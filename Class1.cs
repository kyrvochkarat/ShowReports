using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using ServerEvents = Exiled.Events.Handlers.Server;

namespace ReportBroadcast
{
    public class ReportBroadcast : Plugin<Config>
    {
        public override string Name => "Report Broadcast";
        public override string Author => "vityanvsk";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(8, 0, 0);

        public static ReportBroadcast Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            ServerEvents.ReportingCheater += OnReportingCheater;
            ServerEvents.LocalReporting += OnLocalReporting;
        }

        private void UnregisterEvents()
        {
            ServerEvents.ReportingCheater -= OnReportingCheater;
            ServerEvents.LocalReporting -= OnLocalReporting;
        }

        private void OnReportingCheater(ReportingCheaterEventArgs ev)
        {
            if (!ev.IsAllowed)
                return;

            BroadcastReport(ev.Player, ev.Target, ev.Reason);
        }

        private void OnLocalReporting(LocalReportingEventArgs ev)
        {
            if (!ev.IsAllowed)
                return;

            BroadcastReport(ev.Player, ev.Target, ev.Reason);
        }

        private void BroadcastReport(Player reporter, Player target, string reason)
        {
            if (reporter == null || target == null)
                return;

            // Формируем сообщение с цветными тегами
            string message = $"<color=red>{reporter.Nickname}</color> " +
                           $"<color=red>({reporter.Id})</color> " +
                           $"<color=yellow>на</color> " +
                           $"<color=red>{target.Nickname}</color> " +
                           $"<color=red>({target.Id})</color>";

            // Добавляем причину, если она указана в конфиге
            if (Config.ShowReason && !string.IsNullOrEmpty(reason))
            {
                message += $"\n<color=white>Причина: {reason}</color>";
            }

            // Отправляем броадкаст всем администраторам
            foreach (Player player in Player.List)
            {
                if (player.RemoteAdminAccess)
                {
                    player.Broadcast(Config.BroadcastDuration, message, Broadcast.BroadcastFlags.Normal);
                }
            }

            // Логируем репорт в консоль сервера, если включено
            if (Config.LogToConsole)
            {
                Log.Info($"[REPORT] {reporter.Nickname} ({reporter.Id}) сообщил о {target.Nickname} ({target.Id}). Причина: {reason}");
            }
        }
    }
}