using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Diagnostics;

namespace PrimeNumberDetector
{
    public class Program
    {
        private static OutputPort led;
        private static InputPort btn;
        private static Stopwatch timer;

        public static void Main()
        {
            bool currentState = false, lastState = false;
            led = new OutputPort(Pins.ONBOARD_LED, false);
            btn = new InputPort(Pins.ONBOARD_BTN, false, Port.ResistorMode.Disabled);
            
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
                    bool isp = IsPrime(clicks);
                    Debug.Print(clicks.ToString() + " > " + isp);

                    timer.Reset();

                    if (IsPrime(clicks))
                        FlashLed(6, true);
                    else
                        FlashLed(1, false);

                    clicks = 0;
                }

                lastState = currentState;
            }
        }

        #region Helpers
        private static bool IsPrime(int number)
        {
            bool isPrime = (number != 1);

            for (int i = 2; i * i <= number && isPrime; i++)
            {
                if (number % i == 0)
                    isPrime = false;
            }

            return isPrime;
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
            timer = Stopwatch.StartNew();
        }
        #endregion
    }
}
