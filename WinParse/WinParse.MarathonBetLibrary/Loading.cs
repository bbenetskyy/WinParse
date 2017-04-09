//#define OneThread

using MarathonBetLibrary.Enums;
using MarathonBetLibrary.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarathonBetLibrary
{
    public class Loading
    {
        private List<string> ids;
        private ParseLogic parse;
        private List<MarathonEvent> result;
        private SportType spert;

        public Loading(SportType sportType)
        {
            spert = sportType;
            result = new List<MarathonEvent>();
            parse = new ParseLogic(sportType);
            ids = parse.LoadID();
        }

        public void LoadingEvent()
        {
            //todo count time with stopwatch
            //todo add logger
            //todo save changes for OneThread
            DateTime startTime = DateTime.Now;
#if OneThread
            for (var index = 0; index < ids.Count(); index++)
            {
                string id = ids[index].Split('#')[0];
                string name = ids[index].Split('#')[1];
                result.Add(parse.GetMarathonEvent(id, name));
            }
#else
            var tasks = new Task<MarathonEvent>[ids.Count];
            for (var index = 0; index < ids.Count(); index++)
            {
                string id = ids[index].Split('#')[0];
                string name = ids[index].Split('#')[1];
                tasks[index] = Task.Factory.StartNew(() => parse.GetMarathonEvent(id, name));
            }
            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                if (task.Result != null)
                    result.Add(task.Result);
            }
#endif
            int endTime = DateTime.Now.Minute - startTime.Minute;
        }

        public List<MarathonEvent> GetEvents()
        {
            return this.result;
        }
    }
}