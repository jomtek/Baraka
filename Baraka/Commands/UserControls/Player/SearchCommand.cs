using Baraka.Utils.MVVM.Command;
using Baraka.ViewModels.UserControls.Player.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Baraka.Services.Quran;
using Baraka.Services;
using System.Collections.ObjectModel;
using Baraka.Models;

namespace Baraka.Commands.UserControls.Player
{
    public class SearchCommand : AsyncCommandBase
    {
        private readonly SuraTabViewModel _viewModel;

        public SearchCommand(SuraTabViewModel suraTabViewModel) : base((ex) => throw ex)
        {
            _viewModel = suraTabViewModel;
        }

        protected override async Task ExecuteAsync(object param)
        {
            string rawQuery = (string)param;
            
            await Task.Delay(350);
            
            // Confirm that the query is actually what the user is looking for
            if (rawQuery != _viewModel.SearchQuery)
            {
                return;
            }

            rawQuery = SearchUtils.PrepareQuery(rawQuery);

            _viewModel.ScrollStateStore.Value = 0;
            _viewModel.SuraList = SuraInfoService.LookUp(rawQuery);
        }
    }
}
