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

                Elements["Виробник"].Value = info.vendor;
                Elements["Область"].Value = info.region.name_ua;
                Elements["Номер"].Value = info.digits;
                Elements["VIN"].Value = info.vin ?? "VIN доступний для авто з регістрацією після 2021";
                Elements["Зареестрована на компанію"].Value = info.operations[^1].is_registered_to_company ? "Tak" : "Hi";
                Elements["Колір"].Value = info.operations[^1].color.ua;
                Elements["Модель"].Value = info.model;
                Elements["Рік моделі"].Value = info.model_year.ToString();
                Elements["Новий код"].Value = info.region.new_code;
                Elements["Старий код"].Value = info.region.old_code;
                Elements["Викрадена"].Value = info.is_stolen ? "Tak" : "Hi";
                Elements["Тип"].Value = info.operations[^1].kind.ua;
                Elements["Адрес"].Value = info.operations[^1].address;
                Elements["Відділ"].Value = info.operations[^1].department;
                Elements["Реєстрація"].Value = info.operations[^1].registered_at;
                Elements["Остання операція"].Value = info.operations[^1].operation.ua;
                Elements["Група операції"].Value = info.operations[^1].operation_group.ua;
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

        public Dictionary<string,InfoViewElement> Elements { get; set; }

        public bool IsVIN { get; set; }

        public string FindString { get; set; } = string.Empty;

        public CFViewModel()
        {
            Elements = new()
            {
                { "Номер", new() },
                { "VIN", new() },
                { "Виробник", new() },
                { "Модель", new() },
                { "Рік моделі", new() },
                { "Колір", new() },
                { "Тип", new() },
                { "Область", new() },
                { "Старий код", new() },
                { "Новий код", new() },
                { "Викрадена", new() },
                { "Зареестрована на компанію", new() },
                { "Адрес", new() },
                { "Відділ", new() },
                { "Реєстрація", new() },
                { "Остання операція", new() },
                { "Група операції", new() },
            };
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public RelayCommand Exit => new((o) => { Environment.Exit(0); });

        public RelayCommand Find => new((o) => find(), (o) => (IsVIN && FindString.Trim().Length == 17) || ( !IsVIN && FindString.Trim().Length == 8));

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
