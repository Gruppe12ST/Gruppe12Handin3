using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class Step2
    {
        private ICookController _cookController;
        private IUserInterface _userInterface;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private IOutput _output;
        private string _consoleOutput;
        private StringWriter stringWriter;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _timer = Substitute.For<ITimer>();
            _userInterface = Substitute.For<IUserInterface>();
            _cookController = new CookController(_timer,_display,_powerTube,_userInterface);

            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        [TestCase(50,60)]
        [TestCase(500,60)]
        [TestCase(700,60)]
        public void CookControllerPowertube_StartCooking(int power, int time)
        {
            _cookController.StartCooking(power,time);
            
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("PowerTube works with " + power));
        }

        [TestCase(49, 1)]
        [TestCase(701, 1)]
        public void CookControllerPowertube_Exp_StartCooking(int power, int time)
        {
            var ex = Assert.Catch<ArgumentOutOfRangeException>(() => _cookController.StartCooking(power, time));
            StringAssert.Contains("Must be between 50 and 700 (incl.)", ex.Message);
        }

        [Test]
        public void CookControllerPowertube_ExpOn_StartCooking()
        {
            _cookController.StartCooking(50,60);
            var ex = Assert.Catch<ApplicationException>(() => _cookController.StartCooking(50, 60));
            StringAssert.Contains("PowerTube.TurnOn: is already on", ex.Message);
        }

        [Test]
        public void CookControllerPowertube_Stop()
        {
            _cookController.StartCooking(100,60);
            _cookController.Stop();
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("PowerTube turned off"));
        }

        [Test]
        public void CookControllerPowertube_TimerExpired()
        {
            _cookController.StartCooking(50,60);
            _timer.Expired += Raise.EventWith(EventArgs.Empty);
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("PowerTube turned off"));
        }

        [Test]
        public void CookControllerDisplay_OnTimerTick()
        {
            _cookController.StartCooking(50, 60);
            
            _timer.TimeRemaining.Returns(50);
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);

            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("Display shows: 00:50"));
        }
    }
}
