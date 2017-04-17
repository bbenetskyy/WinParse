//#define OneThread

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinParse.MarathonBetLibrary.Enums;
using WinParse.MarathonBetLibrary.Model;

namespace WinParse.MarathonBetLibrary
{
    public class Loading
    {
        private List<string> _ids;
        private ParseLogic _parse;
        private List<MarathonEvent> _result;
        private SportType _spert;

        public Loading(SportType sportType)
        {
            _spert = sportType;
            _result = new List<MarathonEvent>();
            _parse = new ParseLogic(sportType);
            _ids = _parse.LoadId();
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
            var tasks = new Task<MarathonEvent>[_ids.Count];
            for (var index = 0; index < _ids.Count(); index++)
            {
                string id = _ids[index].Split('#')[0];
                string name = _ids[index].Split('#')[1];
                tasks[index] = Task.Factory.StartNew(() => _parse.GetMarathonEvent(id, name));
            }
            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                if (task.Result != null)
                    _result.Add(task.Result);
            }
#endif
            int endTime = DateTime.Now.Minute - startTime.Minute;
        }

        public List<MarathonEvent> GetEvents()
        {
            return _result;
        }
    }
}