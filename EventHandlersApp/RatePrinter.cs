using System;

namespace EventHandlers
{
    public class RatePrinter : IObserver<Rate>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Trading is finished");
        }

        public void OnError(Exception error)
        {
            Console.Error.WriteLine(error.Message);
        }

        public void OnNext(Rate value)
        {
            Console.WriteLine("New rate: "+value.Ticker.ToString().ToUpper()+":"+value.CurrencyRate);
        }
        
        public void RateChangedHandler(object source, RateChangedArgs e)
        {
            Console.WriteLine("Rate changed!!! New rate: "+e.CurrencyRate.Ticker+":"+e.CurrencyRate.CurrencyRate);
        }

    }
}