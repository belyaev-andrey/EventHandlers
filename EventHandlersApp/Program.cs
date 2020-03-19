using System;
using System.Collections.Generic;
using System.Threading;

namespace EventHandlers
{
    
    class Program
    {

        static void Main(string[] args) {
            CurrencyExchange exchange = new CurrencyExchange();
            var ratePrinter = new RatePrinter();
            exchange.Subscribe(ratePrinter);
            exchange.RateChanged += ratePrinter.RateChangedHandler;
            
            Thread.Sleep(1000);
            exchange.PublishRate(new Rate(Currency.Usd, 76));
            Thread.Sleep(1000);
            exchange.PublishRate(new Rate(Currency.Eur, 86));
            Thread.Sleep(1000);
            exchange.PublishRate(new Rate(Currency.Usd, 78));
            Thread.Sleep(1000);
            
            Console.WriteLine("----------------");
            foreach (var rate in exchange.AllRates)
            {
                Console.WriteLine(rate.Ticker.ToString().ToUpper()+":"+rate.CurrencyRate);
            }
            Console.WriteLine("----------------");
            
            exchange.PublishRate(null); 

        }
    }

    public enum Currency
    {
        Usd,
        Eur,
        Gbp
    }

    public class Rate
    {
        public Rate(Currency ticker, long currencyRate)
        {
            Ticker = ticker;
            CurrencyRate = currencyRate;
        }

        public Currency Ticker { get; }
        public long CurrencyRate { get;  }
    }

    //public delegate void OnRateChanged(object source, EventArgs e);

    public class RateChangedArgs : EventArgs
    {
        public RateChangedArgs(Rate currencyRate)
        {
            CurrencyRate = currencyRate;
        }

        public Rate CurrencyRate { get; }
    }
    
    public class CurrencyExchange : IObservable<Rate>
    {
        private IList<IObserver<Rate>> _observers = new List<IObserver<Rate>>();
        private IDictionary<Currency, Rate> rates = new Dictionary<Currency, Rate>();

        public event EventHandler<RateChangedArgs> RateChanged;
        
        public void PublishRate(Rate rate)
        {
            if (rate == null)
            {
                foreach (var observer in _observers)
                {
                    observer.OnCompleted();
                }
                rates.Clear();
                return;
            }
            rates[rate.Ticker] = rate;
            
            RateChanged?.Invoke(this, new RateChangedArgs(rate));
            
            foreach (var observer in _observers)
            {
                observer.OnNext(rate);
            }
        }

        public ICollection<Rate> AllRates => rates.Values;

        public IDisposable Subscribe(IObserver<Rate> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        class Unsubscriber : IDisposable
        {
            private IList<IObserver<Rate>> _observers;
            private IObserver<Rate> observer;

            public Unsubscriber(IList<IObserver<Rate>> observers, IObserver<Rate> observer)
            {
                _observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                _observers.Remove(observer);
            }
        }
        
    }
    
}