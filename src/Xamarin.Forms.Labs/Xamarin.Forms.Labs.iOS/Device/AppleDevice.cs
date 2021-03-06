﻿using System;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs
{
    /// <summary>
    /// Apple device base class.
    /// </summary>
    public abstract class AppleDevice : IDevice
    {
        private const string iPhoneExpression = "iPhone([1-6]),([1-4])";
        private const string iPodExpression = "iPod([1-5]),([1])";
        private const string iPadExpression = "iPad([1-4]),([1-6])";

        private static IDevice device;

        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Forms.Labs.AppleDevice"/> class.
        /// </summary>
        protected AppleDevice()
        {
            this.Battery = new Battery();
            this.Accelerometer = new Accelerometer();
            this.FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;

            if (Labs.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }
        }

        /// <summary>
        /// Gets the runtime device for Apple's devices.
        /// </summary>
        /// <value>
        /// The current device.
        /// </value>
        public static IDevice CurrentDevice
        {
            get
            {
                if (device != null)
                {
                    return device;
                }

                var hardwareVersion = GetSystemProperty("hw.machine");

                var regex = new Regex(iPhoneExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Phone(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(iPodExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pod(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(iPadExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pad(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                return device = new Simulator();
            }
        }

        #region IDevice implementation
        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>
        /// The id for the device.
        /// </value>
        public string Id
        {
            get
            {
                // TODO: implement Unique Id for iOS
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the display information for the device.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public IDisplay Display
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the battery for the device.
        /// </summary>
        /// <value>
        /// The battery.
        /// </value>
        public IBattery Battery
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        public IMediaPicker MediaPicker
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets or sets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        public string HardwareVersion
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer
        {
            get
            {
                return "Apple";
            }
        }
        #endregion

        /// <summary>
        /// Gets the system property.
        /// </summary>
        /// <returns>The system property value.</returns>
        /// <param name="property">Property to get.</param>
        public static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }
    }
}

