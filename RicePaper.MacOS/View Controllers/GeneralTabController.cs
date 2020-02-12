using System;
using Foundation;
using AppKit;
using RicePaper.Lib;
using RicePaper.Lib.Model;
using System.Linq;
using RicePaper.Lib.Dictionary;
using RicePaper.MacOS.ViewControllers;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RicePaper.MacOS
{
    public partial class GeneralTabController : TabController
    {
        #region Private Fields
        private WordListSelection _wordListSelection;
        private string _wordListPath;
        #endregion

        #region Initialization 
        public GeneralTabController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            LoadVolatileValues();
            UpdateEntireUI();

            base.ViewWillLayout();
        }
        #endregion

        #region Action Handlers
        partial void CheckboxKanji(NSObject sender) => SetDirty();
        partial void CheckboxFurigana(NSObject sender) => SetDirty();
        partial void CheckboxRomaji(NSObject sender) => SetDirty();
        partial void CheckboxDefinition(NSObject sender) => SetDirty();
        partial void CheckboxEnglishSentence(NSObject sender) => SetDirty();
        partial void CheckboxJapaneseSentence(NSObject sender) => SetDirty();

        partial void ActionWordListDropdown(NSObject sender)
        {
            var choice = GetPopupButtonValue<WordListSelection>(sender);

            if (_wordListSelection != choice)
            {
                _wordListSelection = choice;

                ButtonRefWordListPicker.Enabled = (choice == WordListSelection.Custom);

                UpdateLabels();
                SetDirty();
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
                        _wordListPath = result.Path;
                        UpdateLabels();
                        SetDirty();
                    }
                }
            });
        }

        partial void ActionButtonApply(NSObject sender)
        {
            if (!ValidateSettings())
                return;

            bool imageFolderWasChanged = false;
            bool wordListWasChanged = false;

            if (Settings.WordList != _wordListSelection)
            {
                if (_wordListSelection == WordListSelection.Custom)
                {
                    RiceDictionary.Load(_wordListPath);
                    Settings.UserWordListPath = _wordListPath;
                }
                else
                {
                    RiceDictionary.Load(_wordListSelection);
                }

                Settings.WordList = GetPopupButtonValue<WordListSelection>(DropdownRefWordList);
                wordListWasChanged = true;
            }
            else if (Settings.WordList == WordListSelection.Custom && _wordListSelection == WordListSelection.Custom && Settings.UserWordListPath != _wordListPath)
            {
                RiceDictionary.Load(_wordListPath);
                Settings.UserWordListPath = _wordListPath;
                wordListWasChanged = true;
            }

            Settings.Dictionary = GetPopupButtonValue<DictionarySelection>(DropdownRefDictionary);
            Settings.TextOptions = GetTextOptionsFromUI();

            SetClean();

            // TODO: make this asynchronous on main thread
            Scheduler.ForcedUpdate(changeImage: imageFolderWasChanged, changeWord: wordListWasChanged);

            try
            {
                AppSettings.Save(Settings);
            }
            catch (Exception) { }
        }

        partial void DropdownDictionary(NSObject sender)
        {
            SetDirty();
        }
        #endregion

        #region Private Helpers
        private bool ValidateSettings()
        {
            if (_wordListSelection == WordListSelection.Custom && string.IsNullOrWhiteSpace(_wordListPath))
            {
                Util.Alert(
                    title: "Invalid Settings",
                    message: "Please specify a file path for custom word lists",
                    window: this.View.Window
                );

                return false;
            }

            return true;
        }

        /// <summary>
        /// I couldn't think of a more appropriate name for this method after
        /// a good 10 seconds of pondering. The responsibility of this method
        /// is to set the values for variables used in this controller that
        /// come straight from AppSettings but need to be loaded before
        /// the UI is rendered
        /// </summary>
        private void LoadVolatileValues()
        {
            _wordListPath = Settings.UserWordListPath;
            _wordListSelection = Settings.WordList;
        }

        private void UpdateEntireUI()
        {
            ButtonRefWordListPicker.Enabled = (Settings.WordList == WordListSelection.Custom);

            DropdownRefWordList.SelectItem(DropdownRefWordList.Items()
                .Where(x => x.AccessibilityIdentifier == Settings.WordList.ToString()).FirstOrDefault()
            );

            ButtonRefKanji.StringValue = (Settings.TextOptions.Kanji) ? "1" : "0";
            ButtonRefFurigana.StringValue = (Settings.TextOptions.Furigana) ? "1" : "0";
            ButtonRefRomaji.StringValue = (Settings.TextOptions.Romaji) ? "1" : "0";
            ButtonRefDefinition.StringValue = (Settings.TextOptions.Definition) ? "1" : "0";
            ButtonRefEnglishSentence.StringValue = (Settings.TextOptions.EnglishSentence) ? "1" : "0";
            ButtonRefJapaneseSentence.StringValue = (Settings.TextOptions.JapaneseSentence) ? "1" : "0";

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if (_wordListSelection == WordListSelection.Custom && string.IsNullOrWhiteSpace(_wordListPath) != true)
                LabelWordListPath.StringValue = _wordListPath;
            else
                LabelWordListPath.StringValue = $"Word List: {DropdownRefWordList.Title}";
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
