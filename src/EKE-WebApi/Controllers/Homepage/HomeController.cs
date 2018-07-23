using EKE.Data.Entities.Home;
using EKE.Service.Services.Admin.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EKE_WebApi.Controllers.Homepage
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly Dictionary<string, string> _cities = new Dictionary<string, string> {
            { "Cluj-Napoca", "Kolozsvár" },
            { "Moldovenești", "Várfalva" },
            { "Brașov", "Brassó" },
            { "Miercurea Ciuc", "Csíkszereda" },
            { "Frumoasa", "Csíkszépvíz" },
            { "Gheorgheni", "Gyergyószentmiklós" },
            { "Târgu Mureș", "Marosvásárhely" },
            { "Baia Mare ", "Nagybánya" },
            { "Oradea ", "Nagyvárad" },
            { "Săcele", "Négyfalu" },
            { "Satu Mare", "Szatmárnémeti" },
            { "Sfântu Gheorghe", "Sepsiszentgyörgy" },
            { "Odorheiu Secuiesc", "Székelyudvarhely" },
            { "Timișoara", "Temesvár" },
            { "Turda ", "Torda" },
        };
        private readonly IMainService _mainService;
        private IMemoryCache _cache;

        public HomeController(IMainService mainService, IMemoryCache memoryCache)
        {
            _mainService = mainService;
            _cache = memoryCache;
        }

        [HttpGet("[action]")]
        public List<H_Article> ElementList()
        {
            var result = _mainService.GetAllElements();
            return result.Data;
        }

        [HttpGet("[action]")]
        public List<H_Article> HomeNews()
        {
            var result = _mainService.GetNewsElements();
            return result.Data;
        }

        [HttpGet("[action]")]
        public H_Article GetNewsBySlug(string slug)
        {
            var result = _mainService.GetArticleBySlug(slug);
            return result.Data;
        }

        [HttpGet("[action]")]
        public List<H_Article> LatestElementList()
        {
            var result = _mainService.GetLatestElements();
            return result.Data;
        }

        [HttpGet("[action]")]
        public List<H_Article> ArticleList()
        {
            var result = _mainService.GetAllArticles();
            return result.Data;
        }

        [HttpGet("[action]")]
        public List<H_Article> LatestArticleList()
        {
            var result = _mainService.GetLatestArticles();
            return result.Data;
        }

        [HttpGet("[action]")]
        public List<CalendarEvent> EventList()
        {
            var result = _mainService.GetAllEvents();

            return result.Data.Select(x => new CalendarEvent
            {
                Title = x.Title,
                Start = x.EventStartDate,
                End = x.EventEndDate,
            }).ToList();
        }

        [HttpGet("[action]")]
        public List<H_Article> FutureEventList()
        {
            var result = _mainService.GetFutureEvents();
            return result.Data;
        }

        [HttpGet("[action]/{city}")]
        public async Task<ResponseWeather> GetWeatherForecast(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=7b0b64dbef122dc4b7491ecd5e441edf&units=metric");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    ResponseWeather openWeather = JsonConvert.DeserializeObject<ResponseWeather>(result);
                    openWeather.name = _cities.GetValueOrDefault(city);

                    return openWeather;
                }

                catch (HttpRequestException httpRequestException)
                {
                    return null;
                }
            }
        }

        [HttpGet("[action]")]
        public List<ResponseWeather> WeatherForecast()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Weather, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(15);
                var weathers = new List<ResponseWeather>();
                foreach (var city in _cities)
                {
                    var result = GetWeatherForecast(city.Key);
                    if (result.Status != TaskStatus.Faulted) weathers.Add(result.Result);
                }
                return weathers;
            });

            return cacheEntry;
        }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    public class ResponseWeather
    {
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public double dt { get; set; }
        public double id { get; set; }
        public string name { get; set; }
        public double cod { get; set; }
    }

    public static class CacheKeys
    {
        public static readonly string Weather = "_Weather";
    }

    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}