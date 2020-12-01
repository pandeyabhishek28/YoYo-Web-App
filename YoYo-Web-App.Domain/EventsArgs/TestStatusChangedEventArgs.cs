using System;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.Domain.EventsArgs
{
    public class TestStatusChangedEventArgs : EventArgs
    {
        public Test UpdatedTest { get; private set; }
        public TestStatusChangedEventArgs(Test test)
        {
            UpdatedTest = test;
        }
    }
}
