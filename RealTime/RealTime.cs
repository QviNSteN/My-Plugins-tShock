using System;
using System.Timers;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace RealTime
{
    [ApiVersion(1, 16)]
    public class RealTime : TerrariaPlugin
    {
        public RealTime(Main game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("tshock.RealTime", RealTimeOnOff, "rt", "realtime"));
            RealTimeTimer();
        }

        #region start
        public override Version Version
        {
            get { return new Version("1.0"); }
        }
        public override string Name
        {
            get { return "RealTime"; }
        }
        public override string Author
        {
            get { return "QviNSteN"; }
        }
        public override string Description
        {
            get { return ""; }
        }
        #endregion start

        System.Timers.Timer RTT;
        int hours;
        int minutes;
        string[] array;
        decimal time;

        void RealTimeTimer()
        {
            RTT = new System.Timers.Timer(1000);
            RTT.Elapsed += new ElapsedEventHandler(RealTimeSet);
            RTT.Enabled = true;
        }

        void RealTimeSet(object source, ElapsedEventArgs e)
        {
            array = DateTime.Now.ToShortTimeString().Split(':');
            if (!int.TryParse(array[0], out hours) || hours < 0 || hours > 23
                || !int.TryParse(array[1], out minutes) || minutes < 0 || minutes > 59)
            {
                return;
            }
            time = hours + minutes / 60.0m;
            time -= 4.50m;
            if (time < 0.00m)
                time += 24.00m;
            if (time >= 15.00m)
                TSPlayer.Server.SetTime(false, (double)((time - 15.00m) * 3600.0m));
            else
                TSPlayer.Server.SetTime(true, (double)(time * 3600.0m));
        }

        void RealTimeOnOff(CommandArgs args)
        {
            if (RTT.Enabled)
            {
                RTT.Enabled = false;
                RTT.Stop();
                args.Player.SendInfoMessage("[RealTime] The plugin is disabled.");
            }
            else
            {
                RTT.Enabled = true;
                RTT.Start();
                args.Player.SendInfoMessage("[RealTime] The plugin is enabled.");
            }

        }
    }
}
