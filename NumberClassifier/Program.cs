using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Diagnostics;

namespace NumberClassifier
{
    public class Program
    {
        private static OutputPort led;
        private static InputPort btn;

        public static void Main()
        {
            bool currentState = false, lastState = false;
            led = new OutputPort(Pins.ONBOARD_LED, false);
            btn = new InputPort(Pins.ONBOARD_BTN, false, Port.ResistorMode.Disabled);
            Stopwatch timer = Stopwatch.StartNew();
            int clicks = 0;

            StartApp(false);

            while (true)
            {
                timer.Stop();

                currentState = btn.Read();

                //if button is pressed
                if (currentState && !lastState)
                {
                    clicks++;
                    timer.Reset();
                    timer.Start();
                }                

                //if finished entering number
                else if (timer.ElapsedMilliseconds > 3000)
                {
                    timer.Reset();
                    FlashLed(clicks, false);

                    clicks = 0;

                    StartApp(true);
                }

                lastState = currentState;
            }
        }

        private static void FlashLed(int frashes, bool fast)
        {
            bool on = true;
            int end = frashes * 2;
            int speed = 300;

            if (fast)
                speed /= 2;

            for (int i = 0; i < end; i++, on = !on)
            {
                led.Write(on);
                Thread.Sleep(speed);
            }
        }

        private static void StartApp(bool delay)
        {
            if (delay)
                Thread.Sleep(2000);

            FlashLed(3, true);
        }

    }
}
