using Magellanic.Speech.Recognition;
using System.Diagnostics;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpeechRecognitionExample
{
    public sealed partial class MainPage : Page
    {
        private const string grammarFile = "grammar.xml";

        private SpeechRecognitionManager recognitionManager;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;

            Unloaded += MainPage_Unloaded;
        }

        private async void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            await recognitionManager.Dispose();
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // initialise the speech recognition manager
            recognitionManager = new SpeechRecognitionManager(grammarFile);

            // register the event for when speech is detected
            recognitionManager
                .SpeechRecognizer
                .ContinuousRecognitionSession
                .ResultGenerated += RecognizerResultGenerated;

            // compile the grammar file
            await recognitionManager.CompileGrammar();
        }

        private void RecognizerResultGenerated(
            SpeechContinuousRecognitionSession session,
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            // only act if the speech is recognised with high confidence
            if (!args.Result.IsRecognisedWithHighConfidence())
            {
                return;
            }

            // interpret key individual parts of the grammar specification
            string command = args.Result.SemanticInterpretation.GetInterpretation("command");
            string direction = args.Result.SemanticInterpretation.GetInterpretation("direction");

            // write to debug
            Debug.WriteLine($"Command: {command}, Direction: {direction}");
        }
    }
}
