using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinParse.DataSaver.RavenDB
{
    public static class RavenDB_Extention
    {
        public static int GetPageCount<T>(this IRavenQueryable<T> queryable, int pageSize)
        {
            if (pageSize < 1)
            {
                throw new ArgumentException("Page size is less then 1");
            }

            RavenQueryStatistics stats;
            queryable.Statistics(out stats).Take(0).ToArray(); //Without recounting statistic do not work

            var result = stats.TotalResults / pageSize;

            if (stats.TotalResults % pageSize > 0) //Rounded Up
            {
                result++;
            }

            return result;
        }

        public static IEnumerable<T> GetPage<T>(this IRavenQueryable<T> queryable, int page, int pageSize)
        {
            return queryable
            .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToArray();
        }

        public static IEnumerable<T> GetAllRecords<T>(this IRavenQueryable<T> queryable, int pageSize)
        {
            var resList = new List<T>();
            for (int i = 1; i <= queryable.GetPageCount(pageSize); i++)
            {
                resList.AddRange(queryable.GetPage(i, pageSize));
            }
            return resList;
        }
    }
}