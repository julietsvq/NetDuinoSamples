using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Diagnostics;

namespace NetduinoApp2
{
    public class Program
    {
        //click, led will flash twice. click to enter a number n, wait and led will flash n times
        public static void Main()
        {
            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
            InputPort btn = new InputPort(Pins.ONBOARD_BTN, false, Port.ResistorMode.Disabled);
            bool currentState = false;
            bool lastState = false;
            int flashes = 0;
            bool ledOn = false;
            int clicks = -1;
            Stopwatch timer = Stopwatch.StartNew(); ;

            while (true)
            {
                timer.Stop();

                //if user has already introduced number
                if(timer.ElapsedMilliseconds > 3000)
                {
                    timer.Reset();

                    while(clicks > 0)
                    {
                        clicks--;
                        led.Write(true);
                        Thread.Sleep(500);
                        led.Write(false);
                        Thread.Sleep(500);
                    }
                }

                timer.Start();

                currentState = btn.Read();

                //if button is pressed
                if (currentState && !lastState)
                {
                    clicks++;

                    //flash led twice to start
                    while(flashes < 4)
                    {
                        led.Write(!ledOn);
                        Thread.Sleep(500);
                        ledOn = !ledOn;
                        flashes++;           
                    }

                    if(clicks > 0)
                    {
                        timer.Stop();
                        timer.Reset();
                        timer.Start();
                    }
                }

                lastState = currentState;
            }
        }

    }
}
