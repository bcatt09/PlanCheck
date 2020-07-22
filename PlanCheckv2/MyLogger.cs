using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public static class MyLogger
    {
        public static void Initialize(ScriptContext context)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "ESAPIScripts1.log",
                Layout = "${date:format=HH\\:mm\\:ss:padding=10}|${level:uppercase=true:padding=10}||${gdc:item=User:padding=35}|${gdc:item=Patient:padding=35}|${Plan:padding=15} (${Course:padding=15})|${message}"
            };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            GlobalDiagnosticsContext.Set("User", $"{context.CurrentUser.Name} ({context.CurrentUser.Id})");
            GlobalDiagnosticsContext.Set("Patient", context.Patient);
            GlobalDiagnosticsContext.Set("Course", context.Course.Id);
            GlobalDiagnosticsContext.Set("Plan", context.PlanSetup.Id);
        }
    }
}
