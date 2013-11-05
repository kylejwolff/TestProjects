using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Reactive.Linq;
using System.Threading;

namespace ReactiveBufferTest
{
    class Program
    {
        static readonly object oclock = new object();
        static readonly ObservableCollection<string> oc = new ObservableCollection<string>();        
        static void Main(string[] args)
        {

            var one = new HashSet<MailAddress> { new MailAddress("one@durf.com"), new MailAddress("two@hurf.com") };
            var two = new HashSet<MailAddress> { new MailAddress("two@hurf.com"), new MailAddress("one@durf.com"), };

            var equal = one.SetEquals(two);
                        
            var sink = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(x => oc.CollectionChanged += x, y => oc.CollectionChanged -= y)
                .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add);

            var window = sink.Window(10).Subscribe(x => { if(oc.Count >= 10) Send("window"); });
            var throttle = sink.Throttle(TimeSpan.FromSeconds(2)).Subscribe(x => Send("throttle"));

            unchecked
            {
                for(int i = 1; i <= 3; i++)
                    Seed(i * 1000, i.ToString());
            }

            Console.WriteLine("Press any key to exit..");
            Console.ReadKey(true);

            window.Dispose();
            throttle.Dispose();
        }

        private static void Send(string sender)
        {
            string[] items = null;
            //lock(oclock)
            //{
                items = oc.ToArray();
                foreach(var item in items)
                    oc.Remove(item);
            //}
            if(items.Length == 0)
                Console.WriteLine("Wasted time.");
            else
                foreach(var i in items)
                    Console.WriteLine(string.Format("{2} removed item \"{0}\" at {1:HH:mm:ss}", i, DateTime.Now, sender));
        }

        private static void Seed(int wait, string name)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                int total = wait/1000 * 2;
                int count = 0;
                do
                {
                    Insert(name + "|" + DateTime.Now.ToShortTimeString());
                    Thread.Sleep(wait);
                } while(++count < total);
            });
        }

        private static void Insert(string value)
        {
            Console.WriteLine(string.Format("Added item \"{0}\" at {1:HH:mm:ss}", value, DateTime.Now));
            //lock(oclock)
                oc.Add(value);
        }
    }
}
