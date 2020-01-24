﻿using System;
using System.Linq;
using AppKit;
using Foundation;
using RicePaper.Lib;
using RicePaper.Lib.Model;

namespace RicePaper.MacOS
{
    public partial class ViewController : NSViewController
    {
        #region Private Fields
        private DrawPosition _drawPosition;
        private WordSelectionMode _wordSelectionMode;
        private string _imagePath;
        private string _wordPath;
        #endregion

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
            // TODO: Dialog layer sorting

            // TODO: populate dropdowns with enum values
            UpdateEntireUI();

            base.ViewWillLayout();
        }
        #endregion

        #region Action Handlers
        partial void ActionButtonApply(NSObject sender)
        {
            AppSettings.Dictionary = GetPopupButtonValue<DictionarySelection>(DropdownRefDictionary);
            AppSettings.DrawPosition = _drawPosition;
            AppSettings.ImageCycle = GetCycleInfo(DropdownRefImageIntervalUnit, FieldRefImageInterval);
            AppSettings.ImageOption = GetPopupButtonValue<ImageOptionType>(DropdownRefImageList);
            AppSettings.ImagePath = _imagePath;
            AppSettings.TextOptions = GetTextOptionsFromUI();
            AppSettings.WordCycle = GetCycleInfo(DropdownRefWordIntervalUnit, FieldRefWordInterval);
            AppSettings.WordList = GetPopupButtonValue<WordListSelection>(DropdownRefWordList);
            AppSettings.WordListPath = _wordPath;
            AppSettings.WordSelection = _wordSelectionMode;

            SetClean();

            RiceScheduler.Update(changeImage: true, changeWord: true);
        }

        partial void ActionImageDialog(NSButton sender)
        {
            var dialog = NSOpenPanel.OpenPanel;
            dialog.CanChooseFiles = false;
            dialog.CanChooseDirectories = true;
            dialog.Title = "Select Directory";
            dialog.ShowsResizeIndicator = true;
            dialog.ShowsHiddenFiles = false;
            dialog.AllowsMultipleSelection = false;

            dialog.BeginSheet(this.View.Window, (nint outcome) =>
            {
                if (outcome == 1)
                {
                    var result = dialog.Url;

                    if (result != null)
                    {
                        _imagePath = result.Path;
                        UpdateLabels();
                        SetDirty();
                    }
                }
            });
        }

        partial void ActionButtonNextImage(NSObject sender) => RiceScheduler.Update(true, false);

        partial void ActionButtonNextWord(NSObject sender) => RiceScheduler.Update(false, true);

        partial void ActionPositionCB(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterBottom);

        partial void ActionPositionCM(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterMid);

        partial void ActionPositionCT(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterTop);

        partial void ActionPositionLB(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftBottom);

        partial void ActionPositionLM(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftMid);

        partial void ActionPositionLT(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftTop);

        partial void ActionPositionRB(NSObject sender) => UpdatePosition(sender, DrawPosition.RightBottom);

        partial void ActionPositionRM(NSObject sender) => UpdatePosition(sender, DrawPosition.RightMid);

        partial void ActionPositionRT(NSObject sender) => UpdatePosition(sender, DrawPosition.RightTop);

        partial void ActionWordListDropdown(NSObject sender) => SetDirty();

        partial void ActionWordListDialog(NSObject sender)
        {
            var dialog = NSOpenPanel.OpenPanel;
            dialog.CanChooseFiles = true;
            dialog.CanChooseDirectories = false;
            dialog.Title = "Select File";
            dialog.ShowsResizeIndicator = true;
            dialog.ShowsHiddenFiles = false;
            dialog.AllowsMultipleSelection = false;

            dialog.BeginSheet(this.View.Window, (nint outcome) =>
            {
                if (outcome == 1)
                {
                    var result = dialog.Url;

                    if (result != null)
                    {
                        _imagePath = result.Path;
                        UpdateLabels();
                        SetDirty();
                    }
                }
            });
        }

        partial void ActionImageListDropdown(NSObject sender)
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
                SetDirty();
            }
        }

        partial void DropdownImageIntervalUnit(NSObject sender) => SetDirty();

        partial void DropdownWordIntervalUnit(NSObject sender) => SetDirty();

        partial void ActionImageIntervalTextField(NSObject sender)
        {
            var field = sender as NSTextField;
            bool isInt = int.TryParse(field.StringValue, out int numberValue);

            if (isInt && numberValue > 0 && numberValue != AppSettings.ImageCycle.Interval)
            {
                StepperRefImageInterval.StringValue = field.StringValue;
                SetDirty();
            }
        }

        partial void ActionWordIntervalTextField(NSObject sender)
        {
            var field = sender as NSTextField;
            bool isInt = int.TryParse(field.StringValue, out int numberValue);

            if (isInt && numberValue > 0 && numberValue != AppSettings.WordCycle.Interval)
            {
                StepperRefWordInterval.StringValue = field.StringValue;
                SetDirty();
            }
        }

        partial void ActionRadioIterateInOrder(NSObject sender)
        {
            ButtonRefRadioRandom.StringValue = "0";
            _wordSelectionMode = WordSelectionMode.InOrder;
            SetDirty();
        }

        partial void ActionRadioIterateRandom(NSObject sender)
        {
            ButtonRefRadioInOrder.StringValue = "0";
            _wordSelectionMode = WordSelectionMode.Random;
            SetDirty();
        }

        partial void DropdownDictionary(NSObject sender)
        {
            SetDirty();
        }

        partial void ActionStepperImageInterval(NSObject sender)
        {
            var stepper = sender as NSStepper;
            int numberValue = int.Parse(stepper.StringValue);

            FieldRefImageInterval.StringValue = stepper.StringValue;
            SetDirty();
        }

        partial void ActionStepperWordInterval(NSObject sender)
        {
            var stepper = sender as NSStepper;
            int numberValue = int.Parse(stepper.StringValue);

            FieldRefWordInterval.StringValue = stepper.StringValue;
            SetDirty();
        }

        partial void CheckboxKanji(NSObject sender) => SetDirty();
        partial void CheckboxFurigana(NSObject sender) => SetDirty();
        partial void CheckboxRomaji(NSObject sender) => SetDirty();
        partial void CheckboxDefinition(NSObject sender) => SetDirty();
        partial void CheckboxEnglishSentence(NSObject sender) => SetDirty();
        partial void CheckboxJapaneseSentence(NSObject sender) => SetDirty();
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

            _drawPosition = newPosition;
            SetDirty();
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

            ButtonRefRadioInOrder.StringValue = (AppSettings.WordSelection == WordSelectionMode.InOrder) ? "1" : "0";
            ButtonRefRadioRandom.StringValue = (AppSettings.WordSelection == WordSelectionMode.Random) ? "1" : "0";

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

        private int GetUnitValue(NSObject sender)
        {
            var field = sender as NSTextField;
            int numberValue = int.Parse(field.StringValue);

            return numberValue;
        }

        private CycleInfo GetCycleInfo(NSObject unitDropdown, NSObject textField)
        {
            return new CycleInfo()
            {
                CycleType = GetPopupButtonValue<CycleIntervalUnit>(unitDropdown),
                Interval = GetUnitValue(textField)
            };
        }

        private bool GetCheckboxValue(NSObject sender)
        {
            var checkbox = sender as NSButton;
            return (checkbox.StringValue == "0") ? false : true;
        }

        private TextOptions GetTextOptionsFromUI()
        {
            return new TextOptions()
            {
                Definition = GetCheckboxValue(ButtonRefDefinition),
                EnglishSentence = GetCheckboxValue(ButtonRefEnglishSentence),
                Furigana = GetCheckboxValue(ButtonRefFurigana),
                JapaneseSentence = GetCheckboxValue(ButtonRefJapaneseSentence),
                Kanji = GetCheckboxValue(ButtonRefKanji),
                Romaji = GetCheckboxValue(ButtonRefRomaji)
            };
        }

        private void SetDirty() => ButtonRefApply.Enabled = true;
        private void SetClean() => ButtonRefApply.Enabled = false;
        #endregion
    }
}
