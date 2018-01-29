﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2Styles
{
    public partial class ListView
    {
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                SelectItem(item.Content as BasicInfoViewModel);
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (listView != null && e.Key == Key.Enter)
            {
                SelectItem(listView.SelectedItem as BasicInfoViewModel);
            }
        }

        private void SelectItem(BasicInfoViewModel item)
        {
            if (item is MetadataInfoViewModel && !BaseMainViewModel.BaseInstance.IsSelectionEnabled())
            {
                return;
            }

            BaseMainViewModel.BaseInstance.ProcessItem(item);
        }

        private async void OnProjectDetailsTemplatesPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (listView != null && e.Key == Key.F2)
            {
                var savedTemplate = listView.SelectedItem as SavedTemplateViewModel;
                if (savedTemplate != null)
                {
                    savedTemplate.Focus();
                }
            }
        }
    }
}
