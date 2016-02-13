﻿#region Using Statements
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino; 
#endregion

namespace NetduiApp1
{
    public class Program
    {

        public static void Main()
        {
            HelloWorld1();
        }

        public static void HelloWorld1()
        {
            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
            InputPort btn = new InputPort(Pins.ONBOARD_BTN, false, Port.ResistorMode.Disabled);
            bool on = true;

            bool currentState = false;
            bool lastState = false;

            while (true)
            {
                currentState = btn.Read();
                if (currentState && !lastState)
                {
                    led.Write(on);
                    on = !on;
                }

                lastState = currentState;
            }
        }
    }
}
