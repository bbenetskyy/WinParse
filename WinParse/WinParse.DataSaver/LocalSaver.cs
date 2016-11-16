using DataParser.Enums;
using DataSaver.Models;
using DataSaver.RavenDB;
using FormulasCollection.Enums;
using FormulasCollection.Models;
using NLog;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataSaver
{
    public class LocalSaver
    {
        internal DocumentStore _store;

        private IDocumentSession _session;

        public const int PageSize = 128;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public LocalSaver()
        {
            _store = new DocumentStore
            {
                Url = "http://localhost:8765",
                DefaultDatabase = "Parser",
                Conventions = { ShouldCacheRequest = url => false }
            };
            _store.Initialize();
            //_store.DatabaseCommands.DisableAllCaching();
        }

        public void ClearDatabase()
        {
            var indexDefinitions = _store.DatabaseCommands.GetIndexes(0, 100);
            foreach (var indexDefinition in indexDefinitions)
            {
                _store.DatabaseCommands.DeleteByIndex(indexDefinition.Name, new IndexQuery());
            }
        }

        internal IDocumentSession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = _store.OpenSession();
                }

                // ReSharper disable once InvertIf
                if (_session.Advanced.NumberOfRequests ==
                    _session.Advanced.MaxNumberOfRequestsPerSession)
                {
                    _session.Dispose();
                    _session = _store.OpenSession();
                }
                return _session;
            }
        }

        public void InsertForks(List<Fork> forkList)
        {
            if (forkList == null) return;

            foreach (var fork in forkList)
            {
                try
                {
                    Session.Store(MapForkToForkRow(fork));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message); _logger.Error(ex.StackTrace);
                }
            }
            Session.SaveChanges();
        }

        public void ClearAndInsertForks(List<Fork> forkList, SportType sportType)
        {
            if (forkList == null || forkList.Count == 0) return;

            var rowsForDelete = MoveForks(forkList, sportType);
            ClearForks(rowsForDelete);
            InsertForks(forkList);
        }

        public List<ForkRow> MoveForks(List<Fork> forkList, SportType sportType)
        {
            if (forkList == null || forkList.Count == 0) return new List<ForkRow>();

            var savedForks = GetAllForkRows()
                ?.ToList();

            if (savedForks == null || savedForks.Count == 0) return new List<ForkRow>();

            var rowsForDelete = savedForks.Where(fBase => fBase.Sport == sportType.ToString())
                                          .ToList();
            foreach (var fBase in rowsForDelete.Where(f => f.Type == ForkType.Saved))
            {
                var fork = forkList.FirstOrDefault(fNew => IsSameFork(fNew, fBase));
                if (fork == null) continue;
                forkList.Remove(fork);
            }
            rowsForDelete.RemoveAll(f => f.Type == ForkType.Saved);
            return rowsForDelete;
        }

        public bool IsSameFork(Fork fNew,
            ForkRow fBase)
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            bool bRes;

            bRes = fNew.Event == fBase.Event;
            bRes = bRes && fNew.MatchDateTime == fBase.MatchDateTime;
            bRes = bRes && fNew.TypeFirst == fBase.TypeFirst;
            bRes = bRes && fNew.TypeSecond == fBase.TypeSecond;

            return bRes;
        }

        public void ClearForks(List<ForkRow> rowsForDelete)
        {
            if (rowsForDelete == null || rowsForDelete.Count <= 0)
                return;
            rowsForDelete.ForEach(f =>
                    _store.DatabaseCommands.Delete(f.Id,
                        null));
            rowsForDelete.Clear();
        }

        public IEnumerable<ForkRow> GetAllForkRows()
        {
            var jsonList = new List<JsonDocument>();
            using (_store.DatabaseCommands.DisableAllCaching())
            {
                for (var i = 0;
                    i <=
                    Session.Query<ForkRow>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).GetPageCount(PageSize);
                    i += PageSize)
                {
                    jsonList.AddRange(_store.DatabaseCommands.GetDocuments(i, PageSize));
                }
            }
            jsonList.RemoveAll(json => !json.Key.Contains("ForkRows/"));

            if (jsonList.Count == 0)
                return new List<ForkRow>();

            var resList = jsonList.Select(MapJsonDocumentToForkRow);
            return resList;
        }

        public async Task<List<Fork>> GetForksAsync(Filter searchCriteria, ForkType forkType)
        {
            return await Task.Run(() => GetForks(searchCriteria, forkType));
        }

        public List<Fork> GetForks(Filter searchCriteria, ForkType forkType)
        {
            return GetAllForkRows().Where(f => f.Type == forkType &&
                ((searchCriteria.Basketball && f.Sport == SportType.Basketball.ToString()) ||
                (searchCriteria.Football && f.Sport == SportType.Soccer.ToString()) ||
                (searchCriteria.Hockey && f.Sport == SportType.Hockey.ToString()) ||
                (searchCriteria.Volleyball && f.Sport == SportType.Volleyball.ToString()) ||
                (searchCriteria.Tennis && f.Sport == SportType.Tennis.ToString())))
                .Select(MapForkRowToFork).ToList();
        }

        public void UpdateFork(ForkRow forkRow)
        {
            var forkDocument = Session.Load<ForkRow>(forkRow.Id);
            forkDocument.Type = forkRow.Type;
            Session.SaveChanges();
        }

        public void DeleteFork(ForkRow forkRow)
        {
            var forkDocument = Session.Load<ForkRow>(forkRow.Id);
            Session.Delete(forkDocument);
            Session.SaveChanges();
        }

        public void UpdateFork(Fork fork)
        {
            UpdateFork(MapForkToForkRow(fork));
        }

        public void DeleteFork(Fork fork)
        {
            DeleteFork(MapForkToForkRow(fork));
        }

        protected ForkRow MapJsonDocumentToForkRow(JsonDocument json)
        {
            var prices = new List<string>();
            json.DataAsJson.Value<RavenJArray>("prices")?
                .ToList()
                .ForEach(price => prices.Add(price.Value<string>()));
            var result = new ForkRow
            {
                Id = json.Key,
                Event = json.DataAsJson.Value<string>("Event"),
                Sport = json.DataAsJson.Value<string>("Sport"),
                TypeFirst = json.DataAsJson.Value<string>("TypeFirst"),
                CoefFirst = json.DataAsJson.Value<string>("CoefFirst"),
                TypeSecond = json.DataAsJson.Value<string>("TypeSecond"),
                CoefSecond = json.DataAsJson.Value<string>("CoefSecond"),
                MatchDateTime = json.DataAsJson.Value<string>("MatchDateTime"),
                BookmakerFirst = json.DataAsJson.Value<string>("BookmakerFirst"),
                BookmakerSecond = json.DataAsJson.Value<string>("BookmakerSecond"),
                MarathonEventId = json.DataAsJson.Value<string>("MarathonEventId"),
                PinnacleEventId = json.DataAsJson.Value<string>("PinnacleEventId"),
                League = json.DataAsJson.Value<string>("League"),
                Type = (ForkType)Enum.Parse(typeof(ForkType), json.DataAsJson.Value<string>("Type")),
                LineId = json.DataAsJson.Value<string>("LineId"),
                sn = json.DataAsJson.Value<string>("sn"),
                mn = json.DataAsJson.Value<string>("mn"),
                ewc = json.DataAsJson.Value<string>("ewc"),
                cid = json.DataAsJson.Value<string>("cid"),
                prt = json.DataAsJson.Value<string>("prt"),
                ewf = json.DataAsJson.Value<string>("ewf"),
                epr = json.DataAsJson.Value<string>("epr"),
                prices = prices,
                selection_key = json.DataAsJson.Value<string>("selection_key"),
                MarRate = json.DataAsJson.Value<string>("MarRate"),
                MarSuccess = json.DataAsJson.Value<string>("MarSuccess"),
                PinRate = json.DataAsJson.Value<string>("PinRate"),
                PinSuccess = json.DataAsJson.Value<string>("PinSuccess")
            };
            return result;
        }
        protected User MapJsonDocumentToUsers(JsonDocument json)
        {
            var result = new User
            {
                Id = json.Key,
                LoginPinnacle = json.DataAsJson.Value<string>("LoginPinnacle"),
                PasswordPinnacle = json.DataAsJson.Value<string>("PasswordPinnacle"),
                LoginMarathon = json.DataAsJson.Value<string>("LoginMarathon"),
                PasswordMarathon = json.DataAsJson.Value<string>("PasswordMarathon"),
                AntiGateCode = json.DataAsJson.Value<string>("AntiGateCode")
            };
            return result;
        }

        protected ForkRow MapForkToForkRow(Fork fork)
        {
            var result = new ForkRow
            {
                Id = fork.Id,
                BookmakerFirst = fork.BookmakerFirst,
                BookmakerSecond = fork.BookmakerSecond,
                CoefFirst = fork.CoefFirst,
                CoefSecond = fork.CoefSecond,
                Event = fork.Event,
                MatchDateTime = fork.MatchDateTime,
                Sport = fork.Sport,
                TypeFirst = fork.TypeFirst,
                TypeSecond = fork.TypeSecond,
                Type = fork.Type,
                MarathonEventId = fork.MarathonEventId,
                PinnacleEventId = fork.PinnacleEventId,
                League = fork.League,
                LineId = fork.LineId,

                sn = fork.sn,
                mn = fork.mn,
                ewc = fork.ewc,
                cid = fork.cid,
                prt = fork.prt,
                ewf = fork.ewf,
                epr = fork.epr,
                prices = fork.prices,
                selection_key = fork.selection_key,


                MarRate = fork.MarRate,
                PinRate = fork.PinRate,
                MarSuccess = fork.MarSuccess,
                PinSuccess = fork.PinSuccess
            };
            return result;
        }


        protected Fork MapForkRowToFork(ForkRow forkRow)
        {
            var result = new Fork
            {
                Id = forkRow.Id,
                BookmakerFirst = forkRow.BookmakerFirst,
                BookmakerSecond = forkRow.BookmakerSecond,
                CoefFirst = forkRow.CoefFirst,
                CoefSecond = forkRow.CoefSecond,
                Event = forkRow.Event,
                MatchDateTime = forkRow.MatchDateTime,
                Sport = forkRow.Sport,
                TypeFirst = forkRow.TypeFirst,
                TypeSecond = forkRow.TypeSecond,
                Type = forkRow.Type,
                MarathonEventId = forkRow.MarathonEventId,
                PinnacleEventId = forkRow.PinnacleEventId,
                League = forkRow.League,
                LineId = forkRow.LineId,

                sn = forkRow.sn,
                mn = forkRow.mn,
                ewc = forkRow.ewc,
                cid = forkRow.cid,
                prt = forkRow.prt,
                ewf = forkRow.ewf,
                epr = forkRow.epr,
                prices = forkRow.prices,
                selection_key = forkRow.selection_key,

                MarRate = forkRow.MarRate,
                PinRate = forkRow.PinRate,
                MarSuccess = forkRow.MarSuccess,
                PinSuccess = forkRow.PinSuccess
            };
            return result;
        }

        public void UpdateUser(User user)
        {
            var userDocument = Session.Load<User>(user.Id);

            userDocument.LoginPinnacle = user.LoginPinnacle;
            userDocument.PasswordPinnacle = user.PasswordPinnacle;
            userDocument.LoginMarathon = user.LoginMarathon;
            userDocument.PasswordMarathon = user.PasswordMarathon;
            userDocument.AntiGateCode = user.AntiGateCode;

            Session.SaveChanges();
        }

        /// <summary>
        /// Add new User into Raven Db
        /// </summary>
        /// <param name="loginPinnacle">User pinnacle login</param>
        /// <param name="passwordPinnacle">User pinnacle password</param>
        /// <param name="loginMarathon">User marathon login</param>
        /// <param name="passwordMarathon">User marathon password</param>
        /// <param name="antiGateCode">anticaptcha user private Id</param>
        /// <returns></returns>
        public bool AddUserToDb(string loginPinnacle,
            string passwordPinnacle,
            string loginMarathon,
            string passwordMarathon,
            string antiGateCode)
        {
            var user = new User
            {
                LoginPinnacle = loginPinnacle,
                PasswordPinnacle = passwordPinnacle,
                LoginMarathon = loginMarathon,
                PasswordMarathon = passwordMarathon,
                AntiGateCode = antiGateCode
            };

            try
            {
                Session.Store(user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
            Session.SaveChanges();
            return true;
        }

        public bool AddUserToDb(User user)
        {
            try
            {
                Session.Store(user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
            Session.SaveChanges();
            return true;
        }


        public User FindUser()
        {
            var jsonList = new List<JsonDocument>();
            using (_store.DatabaseCommands.DisableAllCaching())
            {
                for (var i = 0;
                    i <=
                    Session.Query<ForkRow>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).GetPageCount(PageSize);
                    i += PageSize)
                {
                    jsonList.AddRange(_store.DatabaseCommands.GetDocuments(i, PageSize));
                }
            }
            jsonList.RemoveAll(json => !json.Key.Contains("users/"));
            var resList = jsonList.Select(MapJsonDocumentToUsers);
            return resList.FirstOrDefault();
        }
    }
}
