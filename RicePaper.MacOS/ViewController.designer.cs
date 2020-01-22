// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RicePaper.MacOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField FilePathLabel { get; set; }

		[Outlet]
		AppKit.NSTextField LabelImagePath { get; set; }

		[Outlet]
		AppKit.NSTextField LabelWordListPath { get; set; }

		[Action ("ButtonImageDialog:")]
		partial void ButtonImageDialog (AppKit.NSButton sender);

		[Action ("ButtonNextWord:")]
		partial void ButtonNextWord (Foundation.NSObject sender);

		[Action ("ButtonPositionCB:")]
		partial void ButtonPositionCB (Foundation.NSObject sender);

		[Action ("ButtonPositionCM:")]
		partial void ButtonPositionCM (Foundation.NSObject sender);

		[Action ("ButtonPositionCT:")]
		partial void ButtonPositionCT (Foundation.NSObject sender);

		[Action ("ButtonPositionLB:")]
		partial void ButtonPositionLB (Foundation.NSObject sender);

		[Action ("ButtonPositionLM:")]
		partial void ButtonPositionLM (Foundation.NSObject sender);

		[Action ("ButtonPositionLT:")]
		partial void ButtonPositionLT (Foundation.NSObject sender);

		[Action ("ButtonPositionRB:")]
		partial void ButtonPositionRB (Foundation.NSObject sender);

		[Action ("ButtonPositionRM:")]
		partial void ButtonPositionRM (Foundation.NSObject sender);

		[Action ("ButtonPositionRT:")]
		partial void ButtonPositionRT (Foundation.NSObject sender);

		[Action ("ButtonWordListDialog:")]
		partial void ButtonWordListDialog (Foundation.NSObject sender);

		[Action ("CheckboxDefinition:")]
		partial void CheckboxDefinition (Foundation.NSObject sender);

		[Action ("CheckboxEnglishSentence:")]
		partial void CheckboxEnglishSentence (Foundation.NSObject sender);

		[Action ("CheckboxFurigana:")]
		partial void CheckboxFurigana (Foundation.NSObject sender);

		[Action ("CheckboxJapaneseSentence:")]
		partial void CheckboxJapaneseSentence (Foundation.NSObject sender);

		[Action ("CheckboxKanji:")]
		partial void CheckboxKanji (Foundation.NSObject sender);

		[Action ("CheckboxRomaji:")]
		partial void CheckboxRomaji (Foundation.NSObject sender);

		[Action ("DropdownDictionary:")]
		partial void DropdownDictionary (Foundation.NSObject sender);

		[Action ("DropdownImageIntervalUnit:")]
		partial void DropdownImageIntervalUnit (Foundation.NSObject sender);

		[Action ("DropdownImageList:")]
		partial void DropdownImageList (Foundation.NSObject sender);

		[Action ("DropdownWordIntervalUnit:")]
		partial void DropdownWordIntervalUnit (Foundation.NSObject sender);

		[Action ("DropdownWordList:")]
		partial void DropdownWordList (Foundation.NSObject sender);

		[Action ("FieldImageInterval:")]
		partial void FieldImageInterval (Foundation.NSObject sender);

		[Action ("FieldWordInterval:")]
		partial void FieldWordInterval (Foundation.NSObject sender);

		[Action ("FileDialogButton:")]
		partial void FileDialogButton (AppKit.NSButton sender);

		[Action ("FuriganaCheckbox:")]
		partial void FuriganaCheckbox (Foundation.NSObject sender);

		[Action ("ImageSelectionDropdown:")]
		partial void ImageSelectionDropdown (Foundation.NSObject sender);

		[Action ("KanjiCheckbox:")]
		partial void KanjiCheckbox (Foundation.NSObject sender);

		[Action ("RadioIterateInOrder:")]
		partial void RadioIterateInOrder (Foundation.NSObject sender);

		[Action ("RadioIterateRandom:")]
		partial void RadioIterateRandom (Foundation.NSObject sender);

		[Action ("RomajiCheckbox:")]
		partial void RomajiCheckbox (Foundation.NSObject sender);

		[Action ("SpannerImageInterval:")]
		partial void SpannerImageInterval (Foundation.NSObject sender);

		[Action ("StepperImageInterval:")]
		partial void StepperImageInterval (Foundation.NSObject sender);

		[Action ("StepperWordInterval:")]
		partial void StepperWordInterval (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelImagePath != null) {
				LabelImagePath.Dispose ();
				LabelImagePath = null;
			}

			if (LabelWordListPath != null) {
				LabelWordListPath.Dispose ();
				LabelWordListPath = null;
			}

			if (FilePathLabel != null) {
				FilePathLabel.Dispose ();
				FilePathLabel = null;
			}
		}
	}
}
