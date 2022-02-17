using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using TMPro;
using System.Text;
using System;
using UnityEngine.Events;
using System.Threading.Tasks;

public class Dictation : MonoBehaviour
{
    [SerializeField] private DataTable dataTable;
    [SerializeField] private TextMeshProUGUI lastRecognizedText;

    [Space]
    [SerializeField] private UnityEvent<int> RecognizedEvent;

    [Space]
    [SerializeField] private string status;

    private KeywordRecognizer recognizer;

    private void Update()
    {
        status = recognizer.IsRunning.ToString();
    }

    void Start()
    {
        recognizer = new KeywordRecognizer(dataTable.Keywords, ConfidenceLevel.Medium);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();
    }

    private async void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        RecognizedEvent.Invoke(Array.IndexOf(dataTable.Keywords, args.text));

        lastRecognizedText.text = args.text;

        await Task.Delay(1000);

        lastRecognizedText.text = "...";
    }
}