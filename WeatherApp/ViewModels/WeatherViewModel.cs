using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.ViewModels.Commands;

namespace WeatherApp.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private string _query;

        public string Query
        {
            get { return _query; }
            set {
                _query = value;
                OnPropertyChange("Query");
            }
        }

        public ObservableCollection<City> Cities { get; set; }

        private CurrentConditions _currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return _currentConditions;  }
            set { 
                _currentConditions = value;
                OnPropertyChange("CurrentConditions");
            }
        }

        private City _selectedCity;

        public City SelectedCity
        {
            get { return _selectedCity; }
            set {
                _selectedCity = value;
                OnPropertyChange("SelectedCity");
                GetCurrentConditions();
            }
        }

        public SearchCommand SearchCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new City()
                {
                    LocalizedName = "Richmond"
                };

                CurrentConditions = new CurrentConditions()
                {
                    WeatherText = "Cloudy",
                    Temperature = new Temperature()
                    {
                        Metric = new Units()
                        {
                            Value = "24"
                        }
                    }
                };
            }
            SelectedCity = new City();
            CurrentConditions = new CurrentConditions();
            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        private  void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task GetCurrentConditions()
        {
            //Query = string.Empty;
            //Cities.Clear();
            CurrentConditions = await AccuWeatherHelper.GetCurrentWeather(_selectedCity.Key);

        }

        public async Task MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(_query);
            Cities.Clear();

            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }

    }
}
