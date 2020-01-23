using System;
using System.Linq;
using AppKit;
using Foundation;
using RicePaper.Lib;
using RicePaper.Lib.Model;

namespace RicePaper.MacOS
{
    public partial class ViewController : NSViewController
    {
        #region Properties
        public override NSObject RepresentedObject
        {
            get { return base.RepresentedObject; }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        public AppSettings AppSettings { get; set; }
        public RiceScheduler RiceScheduler { get; internal set; }
        #endregion

        #region Constructor
        public ViewController(IntPtr handle) : base(handle) { }
        #endregion

        #region NS Lifecycle
        public override void ViewWillLayout()
        {
            base.ViewWillLayout();
            // TODO: populate dropdowns with enum values
            UpdateEntireUI();
        }
        #endregion

        #region Action Handlers
        partial void ButtonImageDialog(NSButton sender)
        {
            try
            {
                var dialog = NSOpenPanel.OpenPanel;
                dialog.CanChooseFiles = false;
                dialog.CanChooseDirectories = true;
                dialog.Title = "Select Directory";
                dialog.ShowsResizeIndicator = true;
                dialog.ShowsHiddenFiles = false;
                dialog.AllowsMultipleSelection = false;

                var outcome = dialog.RunModal();
                if (outcome == 1)
                {
                    var result = dialog.Url;

                    if (result != null)
                    {
                        var path = result.Path;
                        AppSettings.ImagePath = path;
                        UpdateLabels();
                        RiceScheduler.Update(changeImage: true, changeWord: false);
                    }
                }
                else
                {
                    // was canceled
                    return;
                }
            }
            catch (Exception) { }
        }

        partial void ButtonNextWord(NSObject sender)
        {
            RiceScheduler.Update(changeImage: false, changeWord: true);
        }

        partial void ButtonPositionCB(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterBottom);

        partial void ButtonPositionCM(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterMid);

        partial void ButtonPositionCT(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterTop);

        partial void ButtonPositionLB(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftBottom);

        partial void ButtonPositionLM(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftMid);

        partial void ButtonPositionLT(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftTop);

        partial void ButtonPositionRB(NSObject sender) => UpdatePosition(sender, DrawPosition.RightBottom);

        partial void ButtonPositionRM(NSObject sender) => UpdatePosition(sender, DrawPosition.RightMid);

        partial void ButtonPositionRT(NSObject sender) => UpdatePosition(sender, DrawPosition.RightTop);

        partial void ButtonWordListDialog(NSObject sender)
        {
            try
            {
                var dialog = NSOpenPanel.OpenPanel;
                dialog.CanChooseFiles = true;
                dialog.CanChooseDirectories = false;
                dialog.Title = "Select File";
                dialog.ShowsResizeIndicator = true;
                dialog.ShowsHiddenFiles = false;
                dialog.AllowsMultipleSelection = false;

                var outcome = dialog.RunModal();
                if (outcome == 1)
                {
                    var result = dialog.Url;

                    if (result != null)
                    {
                        var path = result.Path;
                        AppSettings.WordListPath = path;
                        UpdateLabels();
                        RiceScheduler.Update(changeImage: false, changeWord: true);
                    }
                }
                else
                {
                    // was canceled
                    return;
                }
            }
            catch (Exception) { }
        }

        partial void CheckboxDefinition(NSObject sender)
        {
            AppSettings.TextOptions.Definition = !AppSettings.TextOptions.Definition;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        partial void CheckboxEnglishSentence(NSObject sender)
        {
            AppSettings.TextOptions.EnglishSentence = !AppSettings.TextOptions.EnglishSentence;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        partial void CheckboxFurigana(NSObject sender)
        {
            AppSettings.TextOptions.Furigana = !AppSettings.TextOptions.Furigana;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        partial void CheckboxJapaneseSentence(NSObject sender)
        {
            AppSettings.TextOptions.JapaneseSentence = !AppSettings.TextOptions.JapaneseSentence;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        partial void CheckboxKanji(NSObject sender)
        {
            AppSettings.TextOptions.Kanji = !AppSettings.TextOptions.Kanji;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        partial void CheckboxRomaji(NSObject sender)
        {
            AppSettings.TextOptions.Romaji = !AppSettings.TextOptions.Romaji;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        partial void DropdownDictionary(NSObject sender)
        {
            // At the moment only jisho.org is supported
        }

        partial void DropdownImageIntervalUnit(NSObject sender)
        {
            var choice = GetPopupButtonValue<CycleIntervalUnit>(sender);

            if (AppSettings.ImageCycle.CycleType != choice)
            {
                AppSettings.ImageCycle.CycleType = choice;
                RiceScheduler.Update(changeImage: true, changeWord: false);
            }
        }

        partial void DropdownImageList(NSObject sender)
        {
            var choice = GetPopupButtonValue<ImageOptionType>(sender);

            if (AppSettings.ImageOption != choice)
            {
                AppSettings.ImageOption = choice;
                if (choice == ImageOptionType.Custom)
                    ButtonRefImagePicker.Enabled = true;
                else
                    ButtonRefImagePicker.Enabled = false;

                UpdateLabels();
            }
        }

        partial void DropdownWordIntervalUnit(NSObject sender)
        {
            var choice = GetPopupButtonValue<CycleIntervalUnit>(sender);

            if (AppSettings.WordCycle.CycleType != choice)
            {
                AppSettings.WordCycle.CycleType = choice;
                RiceScheduler.Update(changeImage: false, changeWord: true);
            }
        }

        partial void DropdownWordList(NSObject sender)
        {
            var choice = GetPopupButtonValue<WordListSelection>(sender);

            if (AppSettings.WordList != choice)
            {
                AppSettings.WordList = choice;
                if (choice == WordListSelection.Custom)
                    ButtonRefWordListPicker.Enabled = true;
                else
                    ButtonRefWordListPicker.Enabled = false;

                UpdateLabels();
            }
        }

        partial void FieldImageInterval(NSObject sender)
        {
            var field = sender as NSTextField;
            bool isInt = int.TryParse(field.StringValue, out int numberValue);

            if (isInt && numberValue > 0 && numberValue != AppSettings.ImageCycle.Interval)
            {
                StepperRefImageInterval.StringValue = field.StringValue;

                AppSettings.ImageCycle.Interval = numberValue;
                RiceScheduler.Update(changeImage: true, changeWord: false);
            }
        }

        partial void FieldWordInterval(NSObject sender)
        {
            var field = sender as NSTextField;
            bool isInt = int.TryParse(field.StringValue, out int numberValue);

            if (isInt && numberValue > 0 && numberValue != AppSettings.WordCycle.Interval)
            {
                StepperRefWordInterval.StringValue = field.StringValue;

                AppSettings.WordCycle.Interval = numberValue;
                RiceScheduler.Update(changeImage: false, changeWord: true);
            }
        }

        partial void RadioIterateInOrder(NSObject sender)
        {
            ButtonRefRadioRandom.StringValue = "0";
            AppSettings.WordSelection = WordSelection.InOrder;
            RiceScheduler.Update(changeImage: false, changeWord: true);
        }

        partial void RadioIterateRandom(NSObject sender)
        {
            ButtonRefRadioInOrder.StringValue = "0";
            AppSettings.WordSelection = WordSelection.Random;
            RiceScheduler.Update(changeImage: false, changeWord: true);
        }

        partial void StepperImageInterval(NSObject sender)
        {
            var stepper = sender as NSStepper;
            int numberValue = int.Parse(stepper.StringValue);

            FieldRefImageInterval.StringValue = stepper.StringValue;

            AppSettings.ImageCycle.Interval = numberValue;
            RiceScheduler.Update(changeImage: false, changeWord: true);
        }

        partial void StepperWordInterval(NSObject sender)
        {
            var stepper = sender as NSStepper;
            int numberValue = int.Parse(stepper.StringValue);

            FieldRefWordInterval.StringValue = stepper.StringValue;

            AppSettings.WordCycle.Interval = numberValue;
            RiceScheduler.Update(changeImage: true, changeWord: false);
        }
        #endregion

        #region Private Helpers
        private void UpdatePosition(NSObject sender, DrawPosition newPosition)
        {
            ButtonRefPosLT.AlphaValue = 0.6f;
            ButtonRefPosLM.AlphaValue = 0.6f;
            ButtonRefPosLB.AlphaValue = 0.6f;
            ButtonRefPosCT.AlphaValue = 0.6f;
            ButtonRefPosCM.AlphaValue = 0.6f;
            ButtonRefPosCB.AlphaValue = 0.6f;
            ButtonRefPosRT.AlphaValue = 0.6f;
            ButtonRefPosRM.AlphaValue = 0.6f;
            ButtonRefPosRB.AlphaValue = 0.6f;

            var button = sender as NSButton;
            button.AlphaValue = 1.0f;

            AppSettings.DrawPosition = newPosition;
            RiceScheduler.Update(changeImage: false, changeWord: false);
        }

        private void UpdateEntireUI()
        {
            ButtonRefImagePicker.Enabled = (AppSettings.ImageOption == ImageOptionType.Custom);
            ButtonRefWordListPicker.Enabled = (AppSettings.WordList == WordListSelection.Custom);

            ButtonRefPosCB.AlphaValue = 0.6f;
            ButtonRefPosCM.AlphaValue = 0.6f;
            ButtonRefPosCT.AlphaValue = 0.6f;
            ButtonRefPosLB.AlphaValue = 0.6f;
            ButtonRefPosLM.AlphaValue = 0.6f;
            ButtonRefPosLT.AlphaValue = 0.6f;
            ButtonRefPosRB.AlphaValue = 0.6f;
            ButtonRefPosRM.AlphaValue = 0.6f;
            ButtonRefPosRT.AlphaValue = 0.6f;

            // This is a dirty switch statement but oh well
            switch (AppSettings.DrawPosition)
            {
                case DrawPosition.LeftTop: ButtonRefPosLT.AlphaValue = 1; break;
                case DrawPosition.LeftMid: ButtonRefPosLM.AlphaValue = 1; break;
                case DrawPosition.LeftBottom: ButtonRefPosLB.AlphaValue = 1; break;
                case DrawPosition.CenterTop: ButtonRefPosCT.AlphaValue = 1; break;
                case DrawPosition.CenterMid: ButtonRefPosCM.AlphaValue = 1; break;
                case DrawPosition.CenterBottom: ButtonRefPosCB.AlphaValue = 1; break;
                case DrawPosition.RightTop: ButtonRefPosRT.AlphaValue = 1; break;
                case DrawPosition.RightMid: ButtonRefPosRM.AlphaValue = 1; break;
                case DrawPosition.RightBottom: ButtonRefPosRB.AlphaValue = 1; break;
            }

            ButtonRefRadioInOrder.StringValue = (AppSettings.WordSelection == WordSelection.InOrder) ? "1" : "0";
            ButtonRefRadioRandom.StringValue = (AppSettings.WordSelection == WordSelection.Random) ? "1" : "0";

            DropdownRefImageList.SelectItem(DropdownRefImageList.Items()
                .Where(x => x.AccessibilityIdentifier == AppSettings.ImageOption.ToString()).FirstOrDefault()
            );

            DropdownRefWordList.SelectItem(DropdownRefWordList.Items()
                .Where(x => x.AccessibilityIdentifier == AppSettings.WordList.ToString()).FirstOrDefault()
            );

            FieldRefImageInterval.StringValue = AppSettings.ImageCycle.Interval.ToString();
            StepperRefImageInterval.StringValue = AppSettings.ImageCycle.Interval.ToString();

            FieldRefWordInterval.StringValue = AppSettings.WordCycle.Interval.ToString();
            StepperRefWordInterval.StringValue = AppSettings.WordCycle.Interval.ToString();

            LabelImagePath.StringValue = AppSettings.ImagePathLabel;
            LabelWordListPath.StringValue = AppSettings.WordListPathLabel;

            ButtonRefKanji.StringValue = (AppSettings.TextOptions.Kanji) ? "1" : "0";
            ButtonRefFurigana.StringValue = (AppSettings.TextOptions.Furigana) ? "1" : "0";
            ButtonRefRomaji.StringValue = (AppSettings.TextOptions.Romaji) ? "1" : "0";
            ButtonRefDefinition.StringValue = (AppSettings.TextOptions.Definition) ? "1" : "0";
            ButtonRefEnglishSentence.StringValue = (AppSettings.TextOptions.EnglishSentence) ? "1" : "0";
            ButtonRefJapaneseSentence.StringValue = (AppSettings.TextOptions.JapaneseSentence) ? "1" : "0";
        }

        private void UpdateLabels()
        {
            LabelImagePath.StringValue = AppSettings.ImagePathLabel;
            LabelWordListPath.StringValue = AppSettings.WordListPathLabel;
        }

        /// <summary>
        /// This method relies on the "Accessibility Identifier" being
        /// set in XCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        private T GetPopupButtonValue<T>(NSObject sender) where T : struct
        {
            var dropdown = sender as NSPopUpButton;
            var choiceString = dropdown.SelectedItem.AccessibilityIdentifier;
            T choice = Enum.Parse<T>(choiceString);

            return choice;
        }
        #endregion
    }
}
