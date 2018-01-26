using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using JetBrains.Annotations;
using NLog;

namespace NetLib.WPF.Controls
{
    public class CustomRichTextBox : RichTextBox
    {
        public static readonly DependencyProperty CustomTextProperty = DependencyProperty.Register("CustomText",
            typeof(string), typeof(CustomRichTextBox),
       new PropertyMetadata(string.Empty, CustomTextChangedCallback), CustomTextValidateCallback);

        public string CustomText
        {
            get => (string)GetValue(CustomTextProperty);
            set => SetValue(CustomTextProperty, value);
        }
        private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        private static void CustomTextChangedCallback(
                    DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is CustomRichTextBox crt)
            {
                crt.Document = GetCustomDocument(e.NewValue as string);
            }
        }

        private static bool CustomTextValidateCallback([CanBeNull] object value)
        {
            return value != null;
        }

        [CanBeNull]
        private static FlowDocument GetCustomDocument([CanBeNull] string Text)
        {
            if (Text == null) return null;
            var document = new FlowDocument();
            // remove indent between paragraphs
            var paras = Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var paraText in paras)
            {
                var para = new Paragraph { Margin = new Thickness(0) };
                foreach (var word in paraText.Split(' ').ToList())
                {
                    //This condition could be replaced by the Regex
                    if (word.StartsWith("#"))
                    {
                        var linkName = word.Substring(1, word.Length - 1);
                        //linkURL can be changed based on some condition.
                        var linkURL = GetUrl(linkName);
                        try
                        {
                            var link = new Hyperlink { IsEnabled = true };
                            link.Inlines.Add(linkName);
                            link.NavigateUri = new Uri(linkURL);
                            link.RequestNavigate += (sender, args) => Process.Start(args.Uri.ToString());
                            para.Inlines.Add(link);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, $"CustomRichTextBox wront link - {linkName}");
                            para.Inlines.Add(linkURL);
                        }
                    }
                    else
                    {
                        para.Inlines.Add(word);
                    }
                    para.Inlines.Add(" ");
                }
                document.Blocks.Add(para);
            }
            return document;
        }

        /// <summary>
        /// This method may contain any logic to return a Url based on a key string
        /// </summary>
        /// <param name="key"></param>
        [NotNull]
        private static string GetUrl([NotNull] string key)
        {
            return key;
            //return $@"https://www.google.com/#q={key}";
        }
    }
}