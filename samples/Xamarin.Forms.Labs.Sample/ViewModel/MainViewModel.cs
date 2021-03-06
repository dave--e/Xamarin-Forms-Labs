﻿using System;
using Xamarin.Forms.Labs.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        private readonly IDevice device;
        private string numberToCall = "+1 (855) 926-2746";
        private string textToSpeak = "Hello from Xamarin Forms Labs";
        private string deviceTimerInfo = string.Empty;
        private ObservableCollection<string> items;
        private ObservableCollection<string> images;
        private Command<string> searchCommand;
        private Command callCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel ()
        {
            SpeakCommand = new Command (() => DependencyService.Get<ITextToSpeechService> ().Speak (TextToSpeak));

            Items = new ObservableCollection<string> ();
            Images = new ObservableCollection<string> ();
            for (var i = 0; i < 10; i++) {
                Images.Add ("ad16.jpg");
                Items.Add (string.Format ("item {0}", i));
            }

            this.device = Resolver.Resolve<IDevice> ();
        }

        /// <summary>
        /// The start timer.
        /// </summary>
        public void StartTimer ()
        {
            Device.StartTimer (new TimeSpan (6000), () => {
                DeviceTimerInfo = "This text was updated using the Device Timer";
                return true;
            });
        }

        /// <summary>
        /// Gets the device manufacturer.
        /// </summary>
        /// <value>
        /// The device manufacturer.
        /// </value>
        public string DeviceManufacturer {
            get {
                return string.Format ("Device was manufactured by {0}", this.device.Manufacturer);
            }
        }

        /// <summary>
        /// Gets the device name.
        /// </summary>
        /// <value>
        /// The device name.
        /// </value>
        public string DeviceName {
            get {
                return string.Format ("Device is called {0}", this.device.Name);
            }
        }

        /// <summary>
        /// Gets or sets the number to call.
        /// </summary>
        /// <value>
        /// The number to call.
        /// </value>
        public string NumberToCall {
            get {
                return numberToCall;
            }
            set {
                this.SetProperty (ref numberToCall, value);
            }
        }

        /// <summary>
        /// Gets or sets the text to speak.
        /// </summary>
        /// <value>
        /// The text to speak.
        /// </value>
        public string TextToSpeak {
            get {
                return textToSpeak;
            }
            set {
                this.SetProperty (ref textToSpeak, value);
            }
        }

        private string deviceUIThreadInfo = string.Empty;

        /// <summary>
        /// Gets or sets the device UI thread info.
        /// </summary>
        /// <value>
        /// The device UI thread info.
        /// </value>
        public string DeviceUIThreadInfo {
            get {
                return deviceUIThreadInfo;
            }
            set { 
                this.SetProperty (ref deviceUIThreadInfo, value);
            }
        }

        /// <summary>
        /// Gets or sets the device timer info.
        /// </summary>
        /// <value>
        /// The device timer info.
        /// </value>
        public string DeviceTimerInfo {
            get {
                return deviceTimerInfo;
            }
			
            set { 
                this.SetProperty (ref deviceTimerInfo, value);
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ObservableCollection<string> Items {
            get {
                return items;
            }
            set {
                this.SetProperty (ref items, value);
            }
        }

        /// <summary>
        /// Gets or sets the demo images.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
        public ObservableCollection<string> Images {
            get {
                return images;
            }
            set {
                this.SetProperty (ref images, value);
            }
        }

        /// <summary>
        /// Gets the speak command.
        /// </summary>
        /// <value>
        /// The speak command.
        /// </value>
        public Command SpeakCommand { get; private set; }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        /// <value>
        /// The search command.
        /// </value>
        public Command<string> SearchCommand {
            get {
                return searchCommand ?? (searchCommand = new Command<string> (
                    obj => {
                    },
                    obj => !string.IsNullOrEmpty (obj)));
            }
        }

        /// <summary>
        /// Gets the call command.
        /// </summary>
        /// <value>
        /// The call command.
        /// </value>
        public Command CallCommand {
            get {
                return callCommand ?? (callCommand = new Command (
                    () => this.device.PhoneService.DialNumber (NumberToCall),
                    () => true)); 
            }
        }
    }
}

