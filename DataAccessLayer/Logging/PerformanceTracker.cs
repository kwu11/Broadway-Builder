using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Logging
{
    public class PerformanceTracker
    {
        private readonly Stopwatch _sw;
        private readonly LogDetail _infoToLog;

        public PerformanceTracker(string name, int userId,
                   string location, string product, string layer)
        {
            _sw = Stopwatch.StartNew();
            _infoToLog = new LogDetail()
            {
                Message = name,
                UserId = userId,
                Product = product,
                Layer = layer,
                Location = location,
                Hostname = Environment.MachineName
            };

            var beginTime = DateTime.Now;
            _infoToLog.AdditionalInfo = new Dictionary<string, object>()
            {
                { "Started", beginTime.ToString(CultureInfo.InvariantCulture)   }
            };
        }

        public PerformanceTracker(string name, int userId,
                   string location, string product, string layer,
                   Dictionary<string, object> perfParams)
            : this(name, userId, location, product, layer)
        {
            foreach (var item in perfParams)
                _infoToLog.AdditionalInfo.Add("input-" + item.Key, item.Value);
        }

        public void Stop()
        {
            _sw.Stop();
            _infoToLog.ElapsedMilliseconds = _sw.ElapsedMilliseconds;
            //Logger.WritePerf(_infoToLog);
        }
    }
}
