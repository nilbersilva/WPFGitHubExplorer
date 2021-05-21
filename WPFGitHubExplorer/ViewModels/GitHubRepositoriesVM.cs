using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Input;
using WPFGitHubExplorer.Classes;
using WPFGitHubExplorer.Models;

namespace WPFGitHubExplorer.ViewModels
{
    public class GitHubRepositoriesVM : INotifyPropertyChanged
    {
        private const string URLDefault = "https://api.github.com";
        private string URLSearchDefault = $"search/repositories?";
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

        private List<GitHubRepository> _Repositories;
        public List<GitHubRepository> Repositories
        {
            get
            {
                return _Repositories;
            }
            set
            {
                _Repositories = value;
                Notify();
            }
        }

        public long TotalCount { get; set; }

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

        private int _ItemsPerPage = 10;
        public int ItemsPerPage
        {
            get
            {
                return _ItemsPerPage;
            }
            set
            {
                _ItemsPerPage = value;
                Notify();
            }
        }

        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
                Notify();
            }
        }

        private string _Search;
        public string Search
        {
            get
            {
                return _Search;
            }
            set
            {
                _Search = value;
                Notify();
            }
        }

        public ICommand CommandSearch { get; }
        public ICommand CommandChangeItemsPerPage { get; }

        public GitHubRepositoriesVM()
        {
            CommandSearch = new Command(async (x) =>
             {
                 if (IsBusy) return;
                 await LoadData();
             });
            CommandChangeItemsPerPage = new Command(async (x) =>
            {
                ItemsPerPage = int.Parse(x.ToString());
                await LoadData();
            });
        }

        public async Task LoadData()
        {
            IsBusy = true;
            try
            {
                string sURLSearch = $"{URLSearchDefault}q=";
                if (!string.IsNullOrWhiteSpace(Search))
                {
                    sURLSearch += $"{Uri.EscapeDataString(Search)}";                                             
                }
                else
                {
                    sURLSearch += "{query}";
                }

                if (CurrentPage == 0) CurrentPage = 1;
                sURLSearch += $"&page={CurrentPage}";

                if (ItemsPerPage < 5) ItemsPerPage = 5;

                sURLSearch += $"&per_page={ItemsPerPage}";

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
                                var repositorySearch = await System.Text.Json.JsonSerializer.DeserializeAsync<GitHubRepositoriesSearch>(stream);
                                TotalCount = (long)Math.Round(repositorySearch.total_count / (decimal)ItemsPerPage);
                                Repositories = repositorySearch.items;

                            }
                        }
                        else
                        {
                            MessageBox.Show(await resp.Content.ReadAsStringAsync());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
