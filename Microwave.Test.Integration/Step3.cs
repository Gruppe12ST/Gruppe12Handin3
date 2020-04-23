using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Step3
    {
        private IDoor _door;
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;
        private ILight _light;
        private ICookController _cookcontroller;
        private IDisplay _display;
        private IUserInterface _userinterface;

        [SetUp]
        public void Setup()
        {
            _door = new Door();
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timeButton = new Button();
            _light = Substitute.For<ILight>();
            _cookcontroller = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _userinterface = new UserInterface(_powerButton,_timeButton,_startCancelButton,_door,_display,_light,_cookcontroller);

        }

        #region ButtonPower
        [Test]
        public void ButtonPowerUserInterface_Press0()
        {
            _display.DidNotReceive().ShowPower(50);
        }

        [Test]
        public void ButtonPowerUserInterface_Press1()
        {
            _powerButton.Press();

            _display.Received(1).ShowPower(50);
        }
        [Test]
        public void ButtonPowerUserInterface_Press14()
        {
            for (int i = 0; i < 14; i++)
            {
                _powerButton.Press();
            }

            _display.Received(1).ShowPower(700);
        }

        [Test]
        public void ButtonPowerUserInterface_Press15()
        {
            for (int i = 0; i < 15; i++)
            {
                _powerButton.Press();
            }

            _display.Received(2).ShowPower(50);
        }
        #endregion

        #region ButtonTime

        [Test]
        public void ButtonTimeUserInterface_Press0()
        {
            _powerButton.Press();

            _display.DidNotReceive().ShowTime(1, 0);
        }

        [Test]
        public void ButtonTimeUserInterface_Press1()
        {
            _powerButton.Press();
            _timeButton.Press();

            _display.Received(1).ShowTime(1,0);
        }

        [Test]
        public void ButtonTimeUserInterface_Press5()
        {
            _powerButton.Press();

            for (int i = 0; i < 5; i++)
            {
                _timeButton.Press();
            }
            
            _display.Received(1).ShowTime(5, 0);
        }


        #endregion

        #region ButtonStartCancel

        [Test]
        public void ButtonStartCancelUserInterface_Press1()
        {
            _powerButton.Press();


        }
        

        #endregion

    }
}
