using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace semLinqTask
{
    class Program
    {
        static void Main(string[] args)
        {
            List<WeatherEvent> data = new List<WeatherEvent>();
            
            // Нужно дополнить модель WeatherEvent, создать список этого типа List<>
            // И заполнить его, читая файл с данными построчно через StreamReader
            // Ссылка на файл https://www.kaggle.com/sobhanmoosavi/us-weather-events

            // Написать Linq-запросы, используя синтаксис методов-расширений
            // и продублировать его, используя синтаксис запросов
            // (возможно с вкраплениями методов расширений, ибо иногда первого может быть недостаточно)

            // 0. Linq - сколько различных городов есть в датасете.
            // 1. Сколько записей за каждый из годов имеется в датасете.
            // Потом будут еще запросы

            string path = "C:\\Users\\Tim\\shit\\Desktop\\lessons\\seminars\\30.01.21\\30.01.21\\semLinqTask\\WeatherEvents_Jan2016-Dec2020.csv";
            //string suka = "C:\\Users\\Tim\\shit\\Desktop\\1.txt";
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
            // Task 0
            //var cities = new List<string>(data.Select(element => element.City).Distinct());
            //Console.WriteLine(cities.Count);

            // Task 1
            var posts = data.GroupBy(info => info.EndTime.Year).Select(group => new
            {
                Year = group.Key,
                count = group.Count()
            }).OrderBy(x => x.Year);

            foreach (var post in posts)
            {
                Console.WriteLine($"{post.Year}  {post.count}");
            }

        }
    }

    // Дополнить модель, согласно данным из файла
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