using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzCore.Blazor.Shared
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

//温度
        public int TemperatureC { get; set; }

//
        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
