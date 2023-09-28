using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace CarFinder.ViewModels
{
    internal class CFViewModel : INotifyPropertyChanged
    {
        private readonly string url = "https://baza-gai.com.ua";

        private readonly string key = "0d441e4b96bcb7ac4271120b69df6a96";

        private string photoUrl = string.Empty;

        private string requestCount = string.Empty;

        private List<Comment>? carComments;

        private async void find()
        {
            //string number = "CB9868AX";
            string selector = @"/nomer/";
            if (IsVIN) selector = @"/vin/";
            using HttpRequestMessage request = new(HttpMethod.Get, url + selector + FindString);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("X-Api-Key", key);
            using HttpClient httpClient = new();
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                CarInfo info = JsonSerializer.Deserialize<CarInfo>(content) ?? new();

                Elements[2].Value = info.vendor;
                Elements[6].Value = info.region.name_ua;
                Elements[0].Value = info.digits;
                Elements[1].Value = info.vin;
                Elements[11].Value = info.operations[^1].is_registered_to_company ? "Tak" : "Hi";
                Elements[9].Value = info.operations[^1].color.ua;
                Elements[3].Value = info.model;
                Elements[4].Value = info.model_year.ToString();
                Elements[8].Value = info.region.new_code;
                Elements[7].Value = info.region.old_code;
                Elements[10].Value = info.is_stolen ? "Tak" : "Hi";
                Elements[5].Value = info.operations[^1].kind.ua;
                Elements[12].Value = info.operations[^1].address;
                Elements[13].Value = info.operations[^1].department;
                Elements[14].Value = info.operations[^1].registered_at;
                Elements[15].Value = info.operations[^1].operation.ua;
                Elements[16].Value = info.operations[^1].operation_group.ua;
                PhotoUrl = info.photo_url;
                RequestCount = response.Headers.FirstOrDefault(x => x.Key == "X-RateLimit-Remaining").Value.ElementAt(0);
                carComments = info.comments;
                OnPropertyChanged(nameof(CarComments));
            }
            else MessageBox.Show(response.StatusCode.ToString());
        }

        public string PhotoUrl
        {
            get => photoUrl;
            set
            {
                photoUrl = value;
                OnPropertyChanged();
            }
        }

        public string RequestCount 
        {
            get => requestCount;
            set
            {
                requestCount = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Comment>? CarComments => carComments;

        public List<InfoViewElement> Elements { get; set; }

        public bool IsVIN { get; set; }

        public string FindString { get; set; } = string.Empty;

        public CFViewModel()
        {
            Elements = new()
            {
                new(){Title = "Номер", Value ="" },
                new(){Title = "VIN", Value ="" },
                new(){Title = "Виробник", Value ="" },
                new(){Title = "Модель", Value ="" },
                new(){Title = "Рік моделі", Value ="" },
                new(){Title = "Тип", Value ="" },
                new(){Title = "Область", Value ="" },
                new(){Title = "Старий код", Value ="" },
                new(){Title = "Новий код", Value ="" },
                new(){Title = "Колір", Value ="" },
                new(){Title = "Викрадена", Value ="" },
                new(){Title = "Зареестрована на компанію", Value ="" },
                new(){Title = "Адрес", Value ="" },
                new(){Title = "Відділ", Value ="" },
                new(){Title = "Реєстрація", Value ="" },
                new(){Title = "Остання операція", Value ="" },
                new(){Title = "Група операції", Value ="" },
            };
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public RelayCommand Exit => new((o) => { Environment.Exit(0); });

        public RelayCommand Find => new((o) => find(), (o) => (IsVIN && FindString.Trim().Length == 17) || ( !IsVIN && FindString.Trim().Length == 8));

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
