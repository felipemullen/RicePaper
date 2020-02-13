using System;
using Foundation;
using AppKit;
using RicePaper.Lib;
using RicePaper.Lib.Model;
using System.Linq;
using RicePaper.MacOS.ViewControllers;

namespace RicePaper.MacOS
{
    public partial class ScheduleTabController : TabController
    {
        #region Initialization
        public ScheduleTabController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            UpdateUI();

            base.ViewWillLayout();
        }
        #endregion

        #region Action Handlers
        partial void ActionStepperImageInterval(NSObject sender)
        {
            var stepper = sender as NSStepper;

            FieldRefImageInterval.StringValue = stepper.StringValue;
            Settings.ImageCycle = GetCycleInfo(DropdownRefImageIntervalUnit, FieldRefImageInterval);
            UpdateImage();
            SaveSettings();
        }

        partial void ActionStepperWordInterval(NSObject sender)
        {
            var stepper = sender as NSStepper;

            FieldRefWordInterval.StringValue = stepper.StringValue;
            Settings.WordCycle = GetCycleInfo(DropdownRefWordIntervalUnit, FieldRefWordInterval);
            UpdateImage();
            SaveSettings();
        }

        partial void DropdownImageIntervalUnit(NSObject sender) => ChangeDropdowns();

        partial void DropdownWordIntervalUnit(NSObject sender) => ChangeDropdowns();

        partial void ActionImageIntervalTextField(NSObject sender)
        {
            var field = sender as NSTextField;
            bool isInt = int.TryParse(field.StringValue, out int numberValue);

            if (isInt && numberValue > 0 && numberValue != Settings.ImageCycle.Interval)
            {
                StepperRefImageInterval.StringValue = field.StringValue;
                UpdateImage();
                SaveSettings();
            }
        }

        partial void ActionWordIntervalTextField(NSObject sender)
        {
            var field = sender as NSTextField;
            bool isInt = int.TryParse(field.StringValue, out int numberValue);

            if (isInt && numberValue > 0 && numberValue != Settings.WordCycle.Interval)
            {
                StepperRefWordInterval.StringValue = field.StringValue;
                UpdateImage();
                SaveSettings();
            }
        }

        partial void ActionRadioIterateInOrder(NSObject sender)
        {
            ButtonRefRadioRandom.StringValue = "0";
            Settings.WordSelection = WordSelectionMode.InOrder;
            UpdateImage();
            SaveSettings();
        }

        partial void ActionRadioIterateRandom(NSObject sender)
        {
            ButtonRefRadioInOrder.StringValue = "0";
            Settings.WordSelection = WordSelectionMode.Random;
            UpdateImage();
            SaveSettings();
        }
        #endregion

        #region Private Helpers
        private void ChangeDropdowns()
        {
            Settings.ImageCycle = GetCycleInfo(DropdownRefImageIntervalUnit, FieldRefImageInterval);
            Settings.WordCycle = GetCycleInfo(DropdownRefWordIntervalUnit, DropdownRefWordIntervalUnit);

            UpdateImage();
            SaveSettings();
        }

        private void UpdateUI()
        {
            ButtonRefRadioInOrder.StringValue = (Settings.WordSelection == WordSelectionMode.InOrder) ? "1" : "0";
            ButtonRefRadioRandom.StringValue = (Settings.WordSelection == WordSelectionMode.Random) ? "1" : "0";

            DropdownRefImageIntervalUnit.SelectItem(DropdownRefImageIntervalUnit.Items()
                .Where(x => x.AccessibilityIdentifier == Settings.ImageCycle.CycleType.ToString()).FirstOrDefault()
            );

            DropdownRefWordIntervalUnit.SelectItem(DropdownRefWordIntervalUnit.Items()
                .Where(x => x.AccessibilityIdentifier == Settings.WordCycle.CycleType.ToString()).FirstOrDefault()
            );

            FieldRefImageInterval.StringValue = Settings.ImageCycle.Interval.ToString();
            StepperRefImageInterval.StringValue = Settings.ImageCycle.Interval.ToString();

            FieldRefWordInterval.StringValue = Settings.WordCycle.Interval.ToString();
            StepperRefWordInterval.StringValue = Settings.WordCycle.Interval.ToString();
        }

        private CycleInfo GetCycleInfo(NSObject unitDropdown, NSObject textField)
        {
            return new CycleInfo()
            {
                CycleType = GetPopupButtonValue<CycleIntervalUnit>(unitDropdown),
                Interval = GetUnitValue(textField)
            };
        }
        #endregion
    }
}
