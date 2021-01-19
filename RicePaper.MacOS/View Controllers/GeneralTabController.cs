using System;
using Foundation;
using AppKit;
using RicePaper.Lib;
using RicePaper.Lib.Model;
using System.Linq;
using RicePaper.Lib.Dictionary;
using RicePaper.MacOS.ViewControllers;

namespace RicePaper.MacOS
{
    public partial class GeneralTabController : TabController
    {
        #region Initialization 
        public GeneralTabController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            UpdateUI();

            base.ViewWillLayout();
        }
        #endregion

        #region Action Handlers
        partial void CheckboxKanji(NSObject _) => ChangeTextOption();
        partial void CheckboxFurigana(NSObject _) => ChangeTextOption();
        partial void CheckboxRomaji(NSObject _) => ChangeTextOption();
        partial void CheckboxDefinition(NSObject _) => ChangeTextOption();
        partial void CheckboxEnglishSentence(NSObject _) => ChangeTextOption();
        partial void CheckboxJapaneseSentence(NSObject _) => ChangeTextOption();

        partial void ActionWordListDropdown(NSObject sender)
        {
            var choice = GetPopupButtonValue<WordListSelection>(sender);

            if (Settings.WordList != choice)
            {
                if (choice == WordListSelection.Custom)
                {
                    // Don't update unless there is already a custom path stored
                    if (string.IsNullOrWhiteSpace(Settings.UserWordListPath) == false)
                    {
                        RiceDictionary.Load(Settings.UserWordListPath);
                        UpdateImage(true);
                    }

                    ButtonRefWordListPicker.Enabled = true;
                }
                else
                {
                    ButtonRefWordListPicker.Enabled = false;
                    RiceDictionary.Load(choice);
                    UpdateImage(true);
                }

                Settings.WordList = choice;
                UpdateLabels();
            }
        }

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
                        Settings.UserWordListPath = result.Path;
                        RiceDictionary.Load(result.Path);

                        UpdateLabels();
                        UpdateImage();
                    }
                }
            });
        }

        partial void DropdownDictionary(NSObject sender)
        {
            Settings.Dictionary = GetPopupButtonValue<DictionarySelection>(sender);
        }
        #endregion

        #region Private Helpers
        private void UpdateUI()
        {
            ButtonRefWordListPicker.Enabled = (Settings.WordList == WordListSelection.Custom);

            DropdownRefWordList.SelectItem(DropdownRefWordList.Items()
                .Where(x => x.AccessibilityIdentifier == Settings.WordList.ToString()).FirstOrDefault()
            );

            SetCheckboxValue(ButtonRefKanji, Settings.TextOptions.Kanji);
            SetCheckboxValue(ButtonRefFurigana, Settings.TextOptions.Furigana);
            SetCheckboxValue(ButtonRefRomaji, Settings.TextOptions.Romaji);
            SetCheckboxValue(ButtonRefDefinition, Settings.TextOptions.Definition);
            SetCheckboxValue(ButtonRefEnglishSentence, Settings.TextOptions.EnglishSentence);
            SetCheckboxValue(ButtonRefJapaneseSentence, Settings.TextOptions.JapaneseSentence);

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if (Settings.WordList == WordListSelection.Custom && string.IsNullOrWhiteSpace(Settings.UserWordListPath) != true)
                LabelWordListPath.StringValue = Settings.UserWordListPath;
            else
                LabelWordListPath.StringValue = $"Word List: {DropdownRefWordList.Title}";
        }

        private void ChangeTextOption()
        {
            Settings.TextOptions = new TextOptions()
            {
                Definition = GetCheckboxValue(ButtonRefDefinition),
                EnglishSentence = GetCheckboxValue(ButtonRefEnglishSentence),
                Furigana = GetCheckboxValue(ButtonRefFurigana),
                JapaneseSentence = GetCheckboxValue(ButtonRefJapaneseSentence),
                Kanji = GetCheckboxValue(ButtonRefKanji),
                Romaji = GetCheckboxValue(ButtonRefRomaji)
            };

            UpdateImage();
        }
        #endregion
    }
}
