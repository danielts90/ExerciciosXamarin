using PhoneWord.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhoneWord
{
    public class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        private string translatedNumber;

        public MainPage()
        {
            this.Padding = new Thickness(20, Device.OnPlatform<double>(40, 20, 20), 20, 20);

            StackLayout panel = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 15,
            };

            panel.Children.Add(new Label
            {
                Text = "Enter a Phoneword:",
            });

            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "1-855-XAMARIN",
            });

            panel.Children.Add(translateButton = new Button
            {
                Text = "Translate"
            });

            panel.Children.Add(callButton = new Button
            {
                Text = "Call",
                IsEnabled = false,
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Core.PhonewordTranslator.ToNumber(enteredNumber);
            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = $"Call {translatedNumber}";
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        async void OnCall(object sender, System.EventArgs e)
        {
            if (await DisplayAlert(
               "Dial a Number",
               "Would you like to call " + translatedNumber + "?",
               "Yes",
               "No"))
            {
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                {
                    dialer.Dial(translatedNumber);
                }
            }
        }
    }
}
