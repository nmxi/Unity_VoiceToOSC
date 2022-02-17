using OscJack;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class OscEmitter : MonoBehaviour
{
    [SerializeField] private DataTable dataTable;

    [Space]
    [SerializeField] private TextMeshProUGUI lastSendSignalText;

    OscClient client;

    void Start()
    {
        // IP address, port number
        client = new OscClient("127.0.0.1", 9000);
    }

    void OnDestroy()
    {
        client?.Dispose();
        client = null;
    }

    public async void Send(int index)
    {
        var str = dataTable.OscSignalTable[index];
        var p = str.IndexOf(",");
        var address = str.Substring(0, p);
        var param = str.Substring(p + 1, str.Length - p - 1);

        client.Send(address, int.Parse(param));

        Debug.Log($"{address}, {param}");
        lastSendSignalText.text = $"{address}, {param}";

        await Task.Delay(1000);

        lastSendSignalText.text = "...";
    }
}
