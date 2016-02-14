using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Diagnostics;

namespace PrimeNumberFlasher
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

            int clicks = 0;

            StartApp(false);

            while (true)
            {
                currentState = btn.Read();

                //if button is pressed
                if (currentState && !lastState)
                {
                    clicks++;
                    bool prime = IsPrime(clicks);
                    if (prime)
                        FlashLed(1, true);

                    Debug.Print(clicks + " > " + prime);
                }

                lastState = currentState;
            }
        }

        #region Helpers
        private static bool IsPrime(int number)
        {
            bool isPrime = (number != 1) && (number != 0);
            
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
        }
        #endregion
    }
}
