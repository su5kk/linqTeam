using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace semLinqTask
{
    class Program
    {
        #region Loshki
        //var filteredData = data.Where(x => x.Type == WeatherEventType.Snow);
        ////var cheto = filteredData.Aggregate(new List<WeatherEvent>(), (ans, curr) =>
        ////{
        ////    if (ans.Count == 0 || (ans.Last().City != curr.City || ans.Last().County != curr.County || ans.Last().State != curr.State || curr.StartTime != ans.Last().EndTime))
        ////    {
        ////        ans.Add(curr);
        ////    }
        ////    else
        ////    {
        ////        ans.Last().EndTime = curr.StartTime;
        ////    }
        ////    return ans;
        ////});
        //var cheto = filteredData.Aggregate(new List<WeatherEvent>(), (ans, curr) =>
        //{
        //    if (ans.Count == 0 || (ans.Last().City != curr.City || ans.Last().County != curr.County || ans.Last().State != curr.State || IdToNum(curr.EventId) != IdToNum(ans.Last().EventId) + 1))
        //    {
        //        ans.Add(curr);
        //    }
        //    else
        //    {
        //        ans.Last().EndTime = curr.EndTime;
        //        ans.Last().EventId = curr.EventId;
        //    }
        //    return ans;
        //});

        //var hren = cheto.Aggregate(new List<WeatherEvent>(), (ans, curr) =>
        //{
        //    if (ans.Count == 0 || (ans.Last().City != curr.City || ans.Last().County != curr.County || ans.Last().State != curr.State))
        //    {
        //        ans.Add(curr);
        //    }
        //    else if (curr.Longevity > ans.Last().Longevity)
        //    {
        //        ans[ans.Count - 1] = curr;
        //    }
        //    return ans;
        //});

        //var ans = hren.GroupBy(x => x.Year)
        //    .SelectMany(y => y.OrderByDescending(z => z.Longevity))
        //    .GroupBy(a => a.Year)
        //    .Select(group => new
        //    {
        //        Year = group.Key,
        //        City = group.First().City,
        //        StartTime = group.First().StartTime,
        //        EndTime = group.First().EndTime,
        //        EventId = group.First().EventId
        //    });

        //foreach (var item in ans)
        //{
        //    Console.WriteLine($"Year: {item.Year}\nCity: {item.City}\nStart time: {item.StartTime}\nEnd time: {item.EndTime}\nEvent id: {item.EventId}");
        //}
        #endregion
        public static void CountCitiesSyntax(List<WeatherEvent> data)
        {
            var cities = from Event in data
                         select Event.City;
            cities = cities.Distinct();

            var states = from Event in data
                         select Event.State;
            states = states.Distinct();

            Console.WriteLine($"Num of cities: {cities.Count()}");
            Console.WriteLine($"Num of states: {states.Count()}");
        }
        public static void CountCitiesExtension(List<WeatherEvent> data)
        {
            // Task 0
            var cities = data.Select(element => element.City).Distinct();
            var states = data.Select(element => element.State).Distinct();
            Console.WriteLine($"Num of cities: {cities.Count()}");
            Console.WriteLine($"Num of states: {states.Count()}");
        }

        public static void EventsByYearExtension(List<WeatherEvent> data)
        {
            var posts = data.Where(p => p.Year == 2018).Count();
            Console.WriteLine($"Num of events in 2018: {posts}");
        }
        public static void EventsByYearSyntax(List<WeatherEvent> data)
        {
            var posts = from Event in data
                        where Event.Year == 2018
                        select Event.Year;
            Console.WriteLine($"Num of events in 2018: {posts.Count()}");
        }

        public static void FindRainyCitiesExtension(List<WeatherEvent> data)
        {
            var filteredData = data.Where(x => x.Year == 2019 && x.Type == WeatherEventType.Rain).GroupBy(info => info.City).Select(group => new
            {
                City = group.Key,
                Count = group.Count()
            }).OrderByDescending(y => y.Count).ToArray();
            Array.Resize(ref filteredData, 3);
            foreach (var dataElem in filteredData)
            {
                Console.WriteLine($"City: {dataElem.City}\nNumber of rains: {dataElem.Count}");
            }
        }

        public static void FindRainyCitiesSyntax(List<WeatherEvent> data)
        {
            var filteredData = from elem in data
                               where elem.Year == 2019 && elem.Type == WeatherEventType.Rain
                               group elem by elem.City into info
                               orderby info.Count() descending
                               select $"City: {info.Key}\nNumber of rains: {info.Count()}";
            var ans = filteredData.ToArray();
            Array.Resize(ref ans, 3);
            foreach (var dataElem in ans)
            {
                Console.WriteLine(dataElem);
            }
                               
        }

        public static void TopSnowCityByYearExtension(List<WeatherEvent> data)
        {
            var filteredData = data.Where(x => x.Type == WeatherEventType.Snow)
                .GroupBy(x => x.Year)
                .SelectMany(group => group.OrderByDescending(x => x.Longevity))
                .GroupBy(x => x.Year)
                .Select(group => new {
                    Year = group.Key,
                    City = group.First().City,
                    StartTime = group.First().StartTime,
                    EndTime = group.First().EndTime,
                    EventId = group.First().EventId
                });
            foreach (var item in filteredData)
            {
                Console.WriteLine($"Year: {item.Year}\nCity: {item.City}\nStart time: {item.StartTime}\nEnd time: {item.EndTime}\nEvent id: {item.EventId}");
            }
        }

        public static void TopSnowCityByYearSyntax(List<WeatherEvent> data)
        {
            var filteredData = from elem in data
                               where elem.Type == WeatherEventType.Snow
                               group elem by elem.Year into info
                               from i in info
                               orderby i.Longevity descending
                               group i by i.Year into ans
                               select $"Year: {ans.Key}\nCity: {ans.First().City}\nStart time: {ans.First().StartTime}\nEnd time: {ans.First().EndTime}\nEvent id: {ans.First().EventId}";
            foreach (var item in filteredData)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Converts id to num
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int IdToNum(string id)
        {
            id = id.Substring(2, id.Length - 2);
            return int.Parse(id);
        }

        static void Main(string[] args)
        {
            List<WeatherEvent> data = new List<WeatherEvent>();
            string path = "C:\\Users\\Tim\\shit\\Desktop\\lessons\\seminars\\30.01.21\\30.01.21\\semLinqTask\\WeatherEvents_Jan2016-Dec2020.csv";
            string supa = "C:\\Users\\Tim\\shit\\Desktop\\1.txt";
            using (StreamReader streamReader = new StreamReader(File.Open(path, FileMode.Open)))
            {
                streamReader.ReadLine();
                while (!streamReader.EndOfStream)
                {
                    string[] text = streamReader.ReadLine().Split(',');
                    WeatherEvent weatherEvent = new WeatherEvent();
                    weatherEvent.EventId = text[0];
                    weatherEvent.Type = text[1] switch
                    {
                        "Snow" => WeatherEventType.Snow,
                        "Rain" => WeatherEventType.Rain,
                        "Cold" => WeatherEventType.Cold,
                        "Fog" => WeatherEventType.Fog,
                        "Precipitation" => WeatherEventType.Precipitation,
                        "Hail" => WeatherEventType.Hail,
                        "Storm" => WeatherEventType.Storm
                    };
                    weatherEvent.Severity = text[2] switch
                    {
                        "Other" => Severity.Other,
                        "UNK" => Severity.UNK,
                        "Heavy" => Severity.Heavy,
                        "Light" => Severity.Light,
                        "Moderate" => Severity.Moderate,
                        "Severe" => Severity.Severe
                    };

                    weatherEvent.StartTime = DateTime.Parse(text[3]);
                    weatherEvent.EndTime = DateTime.Parse(text[4]);
                    weatherEvent.TimeZone = text[5] switch
                    {
                        "US/Mountain" => TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time"),
                        "US/Pacific" => TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
                        "US/Central" => TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
                        "US/Eastern" => TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time"),
                    };
                    weatherEvent.AirportCode = text[6];
                    weatherEvent.LocationLat = double.Parse(text[7]);
                    weatherEvent.LocationLng = double.Parse(text[8]);
                    weatherEvent.City = text[9];
                    weatherEvent.County= text[10];
                    weatherEvent.State = text[11];
                    try
                    {
                        weatherEvent.ZipCode = int.Parse(text[12]);
                    }
                    catch
                    {
                        weatherEvent.ZipCode = null;
                    }
                    data.Add(weatherEvent);
                }
            }
            //Console.WriteLine("Events by year extension: ");
            //EventsByYearExtension(data);
            //Console.WriteLine("\nEvents by year syntax: ");
            //EventsByYearSyntax(data);
            //Console.WriteLine("\nCount cities and states extension: ");
            //CountCitiesExtension(data);
            //Console.WriteLine("\nCount cities and states syntax: ");
            //CountCitiesSyntax(data);
            //Console.WriteLine("\nCount top 3 samyh dozhdlivyh cities v 2019 (extension): ");
            //FindRainyCitiesExtension(data);
            //Console.WriteLine("\nCount top 3 samyh dozhdlivyh cities v 2019 (syntax): ");
            //FindRainyCitiesSyntax(data);
            //Console.WriteLine("\nTop 1 snezhnyh gorodov(syntax): ");
            //TopSnowCityByYearSyntax(data);
            //Console.WriteLine("\nTop 1 sn g (extension): ");
            //TopSnowCityByYearExtension(data);
        }
    }

   
    class WeatherEvent
    {
        public string EventId { get; set; }
        public WeatherEventType Type { get; set; }
        public Severity Severity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public string AirportCode { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public int Year { get { return EndTime.Year; } }
        public double Longevity { get { return (EndTime - StartTime).TotalSeconds; } }

        public override string ToString()
        {
            return $"EventID: {this.EventId}\nType: {this.Type}\nSeverity: {this.Severity}\nStartTime: {this.StartTime}\nEndTime: {this.EndTime}\nTime zone: {this.TimeZone}\n" +
                $"AirportCode: {this.AirportCode}\nLocationLat: {this.LocationLat}\nLocationLng: {this.LocationLng}\n" +
                $"City: {this.City}\nCounty: {this.County}\nState: {this.State}\nZip-code: {((this.ZipCode == null) ? "No zip" : this.ZipCode)}";
        }
    }

    // Дополнить перечисления
    // EventId,Type,Severity,StartTime(UTC),EndTime(UTC),TimeZone,AirportCode,LocationLat,LocationLng,City,County,State,ZipCode
    enum WeatherEventType
    {
        Precipitation,
        Snow,
        Fog,
        Rain,
        Cold,
        Storm,
        Hail
    }

    enum Severity
    {
        Other,
        Light,
        Severe,
        Moderate,
        Heavy,
        UNK
    }
    // US Mountain = US Mountain Standard Time
    // US Pacific = Pacific Standard Time
    // Us Central = Central Standard Time
    // US Eastern = US Eastern Standard Time
}