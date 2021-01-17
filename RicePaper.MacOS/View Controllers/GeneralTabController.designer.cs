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
	[Register ("GeneralTabController")]
	partial class GeneralTabController
	{
		[Outlet]
		AppKit.NSButton ButtonRefDefinition { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefEnglishSentence { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefFurigana { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefJapaneseSentence { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefKanji { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRomaji { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefWordListPicker { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefDictionary { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefWordList { get; set; }

		[Outlet]
		AppKit.NSTextField LabelWordListPath { get; set; }

		[Action ("ActionWordListDialog:")]
		partial void ActionWordListDialog (Foundation.NSObject sender);

		[Action ("ActionWordListDropdown:")]
		partial void ActionWordListDropdown (Foundation.NSObject sender);

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
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonRefDefinition != null) {
				ButtonRefDefinition.Dispose ();
				ButtonRefDefinition = null;
			}

			if (ButtonRefEnglishSentence != null) {
				ButtonRefEnglishSentence.Dispose ();
				ButtonRefEnglishSentence = null;
			}

			if (ButtonRefFurigana != null) {
				ButtonRefFurigana.Dispose ();
				ButtonRefFurigana = null;
			}

			if (ButtonRefJapaneseSentence != null) {
				ButtonRefJapaneseSentence.Dispose ();
				ButtonRefJapaneseSentence = null;
			}

			if (ButtonRefKanji != null) {
				ButtonRefKanji.Dispose ();
				ButtonRefKanji = null;
			}

			if (ButtonRefRomaji != null) {
				ButtonRefRomaji.Dispose ();
				ButtonRefRomaji = null;
			}

			if (ButtonRefWordListPicker != null) {
				ButtonRefWordListPicker.Dispose ();
				ButtonRefWordListPicker = null;
			}

			if (DropdownRefDictionary != null) {
				DropdownRefDictionary.Dispose ();
				DropdownRefDictionary = null;
			}

			if (DropdownRefWordList != null) {
				DropdownRefWordList.Dispose ();
				DropdownRefWordList = null;
			}

			if (LabelWordListPath != null) {
				LabelWordListPath.Dispose ();
				LabelWordListPath = null;
			}
		}
	}
}
