﻿using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using WinUIDesktopApp.Contracts.ViewModels;
using WinUIDesktopApp.Core.Contracts.Services;
using WinUIDesktopApp.Core.Models;

namespace WinUIDesktopApp.ViewModels
{
    public class MasterDetailViewModel : ObservableRecipient, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
        private ListDetailsView _masterDetailsView;
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public MasterDetailViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public void Initialize(ListDetailsView masterDetailsView)
        {
            _masterDetailsView = masterDetailsView;
        }

        public async void OnNavigatedTo(object parameter)
        {
            SampleItems.Clear();

            var data = await _sampleDataService.GetMasterDetailDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            if (_masterDetailsView.ViewState == ListDetailsViewState.Both)
            {
                Selected = SampleItems.First();
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
