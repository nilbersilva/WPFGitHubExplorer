using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFGitHubExplorer.Models;

namespace WPFGitHubExplorer.ViewModels
{
    public class GitHubUserVM : INotifyPropertyChanged
    {
        private const string URLDefault = "https://api.github.com";
        private string URLSearchDefault = $"users/";
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify([CallerMemberName] string propertyName = null)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                Notify();
            }
        }

        public string UserLogin { get; set; }

        private GitHubRepositoryUser _User;
        public GitHubRepositoryUser User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
                Notify();
            }
        }

        public async Task LoadData()
        {
            IsBusy = true;
            try
            {
                string sURLSearch = $"{URLSearchDefault}{UserLogin}";
               

                var handler = new System.Net.Http.HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli };
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                using (var client = new HttpClient(handler, true) { BaseAddress = new Uri(URLDefault) })
                {
                    ProductHeaderValue header = new ProductHeaderValue("WPFGitHubExplorer", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    ProductInfoHeaderValue userAgent = new ProductInfoHeaderValue(header);
                    client.DefaultRequestHeaders.UserAgent.Add(userAgent);
                    using (var resp = await client.GetAsync(sURLSearch))
                    {
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            using (var stream = await resp.Content.ReadAsStreamAsync())
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                User = await System.Text.Json.JsonSerializer.DeserializeAsync<GitHubRepositoryUser>(stream);
                            }
                        }
                        else
                        {
                            MessageBox.Show(await resp.Content.ReadAsStringAsync(), "API Return", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.InnerException?.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
