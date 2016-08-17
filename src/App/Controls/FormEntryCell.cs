﻿using System;
using Xamarin.Forms;

namespace Bit.App.Controls
{
    public class FormEntryCell : ExtendedViewCell
    {
        public FormEntryCell(
            string labelText,
            Keyboard entryKeyboard = null,
            bool IsPassword = false,
            VisualElement nextElement = null,
            bool useLabelAsPlaceholder = false,
            string imageSource = null,
            Thickness? containerPadding = null)
        {
            if(!useLabelAsPlaceholder)
            {
                Label = new Label
                {
                    Text = labelText,
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    Style = (Style)Application.Current.Resources["text-muted"],
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                if(Device.OS == TargetPlatform.Android)
                {
                    Label.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
                }
            }

            Entry = new ExtendedEntry
            {
                Keyboard = entryKeyboard,
                HasBorder = false,
                IsPassword = IsPassword,
                AllowClear = true,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            if(useLabelAsPlaceholder)
            {
                Entry.Placeholder = labelText;
            }

            if(nextElement != null)
            {
                Entry.ReturnType = Enums.ReturnType.Next;
                Entry.Completed += (object sender, EventArgs e) => { nextElement.Focus(); };
            }

            var imageStackLayout = new StackLayout
            {
                Padding = containerPadding ?? new Thickness(15, 10),
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if(imageSource != null)
            {
                var tgr = new TapGestureRecognizer();
                tgr.Tapped += Tgr_Tapped;

                var theImage = new Image
                {
                    Source = imageSource,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };
                theImage.GestureRecognizers.Add(tgr);

                imageStackLayout.Children.Add(theImage);
            }

            var formStackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            if(Device.OS == TargetPlatform.Android)
            {
                Entry.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                formStackLayout.Spacing = 0;
                if(!useLabelAsPlaceholder)
                {
                    Entry.Margin = new Thickness(-4, -2, -4, -10);
                }
            }

            if(!useLabelAsPlaceholder)
            {
                formStackLayout.Children.Add(Label);
            }

            formStackLayout.Children.Add(Entry);
            imageStackLayout.Children.Add(formStackLayout);

            Tapped += FormEntryCell_Tapped;

            View = imageStackLayout;
        }

        public Label Label { get; private set; }
        public ExtendedEntry Entry { get; private set; }

        private void Tgr_Tapped(object sender, EventArgs e)
        {
            Entry.Focus();
        }

        private void FormEntryCell_Tapped(object sender, EventArgs e)
        {
            Entry.Focus();
        }
    }
}
