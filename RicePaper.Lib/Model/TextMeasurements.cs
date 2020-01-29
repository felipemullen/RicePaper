using System;
using CoreGraphics;

namespace RicePaper.Lib.Model
{
    public class TextMeasurements
    {
        public CGSize Kanji;
        public CGSize Furigana;
        public CGSize Romaji;
        public CGSize Definition;
        public CGSize DefinitionLabel;
        public CGSize JapaneseSentence;
        public CGSize JapaneseSentenceLabel;
        public CGSize EnglishSentence;
        public CGSize EnglishSentenceLabel;
        public ExpansionBox Spacing;

        public TextMeasurements()
        {
            Spacing = new ExpansionBox();
        }

        public ExpansionBox Totals
        {
            get
            {
                var box = new ExpansionBox();

                box.AddHeight(Kanji.Height)
                    .AddHeight(Furigana.Height)
                    .AddHeight(Romaji.Height)
                    .AddHeight(Definition.Height)
                    .AddHeight(DefinitionLabel.Height)
                    .AddHeight(JapaneseSentence.Height)
                    .AddHeight(JapaneseSentenceLabel.Height)
                    .AddHeight(EnglishSentence.Height)
                    .AddHeight(EnglishSentenceLabel.Height)
                    .AddHeight((nfloat)Spacing.Height)
                    .MaxWidth(Kanji.Width,
                        Furigana.Width,
                        Romaji.Width,
                        Definition.Width,
                        DefinitionLabel.Width,
                        JapaneseSentence.Width,
                        JapaneseSentenceLabel.Width,
                        EnglishSentenceLabel.Width
                    );

                return box;
            }
        }
    }
}
