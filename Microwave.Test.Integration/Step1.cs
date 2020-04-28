using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step1
    {
        private IOutput _output;
        private IPowerTube _powertube;
        private IDisplay _display;
        private ILight _light;
        private string _consoleOutput;
        private StringWriter stringWriter;
        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _powertube = new PowerTube(_output);
            _display = new Display(_output);
            _light = new Light(_output);

            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
                
                
            
        }

        //PowerTube og Output
        [Test]
        public void PowerTubeOutput_TurnOn()
        {
            _powertube.TurnOn(100);
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("PowerTube works with 100"));

        }

        [Test]
        public void PowerTubeOutput_TurnOff()
        {
            _powertube.TurnOn(100);
            _powertube.TurnOff();
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("PowerTube turned off"));
        }

        [Test]
        public void PowerTubeOutput_Exp_OK()
        {
            Assert.That(() => _powertube.TurnOn(50), Throws.Nothing);
        }

        [TestCase(701)]
        [TestCase(49)]
        public void PowerTubeOutput_Exp_OutOfRange(int power)
        {
            var ex = Assert.Catch<ArgumentOutOfRangeException>(() => _powertube.TurnOn(power));
            StringAssert.Contains("Must be between 50 and 700 (incl.)", ex.Message);
        }

        [Test]
        public void PowerTubeOutput_Exp_On()
        {
            _powertube.TurnOn(50);
            var ex = Assert.Catch<ApplicationException>(() => _powertube.TurnOn(50));
            StringAssert.Contains("PowerTube.TurnOn: is already on", ex.Message);
        }


        //Display og Output
        [Test]
        public void DisplayOutput_ShowTime()
        {
            _display.ShowTime(02,30);
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("Display shows: 02:30"));
        }

        [Test]
        public void DisplayOutput_ShowPower()
        {
            _display.ShowPower(40);
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("Display shows: 40 W"));
        }

        [Test]
        public void DisplayOutput_Clear()
        {
            _display.Clear();
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("Display cleared"));
        }


        //Light og Output
        [Test]
        public void LightOutput_TurnOn()
        {
            _light.TurnOn();
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("Light is turned on"));
        }

        [Test]
        public void LightOutput_TurnOff()
        {
            _light.TurnOn();
            _light.TurnOff();
            _consoleOutput = stringWriter.ToString();
            Assert.That(_consoleOutput.Contains("Light is turned off"));
        }

    }
}
