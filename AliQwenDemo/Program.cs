// See https://aka.ms/new-console-template for more information
using MFJIANG.AliDashScopeSDK;
using System.Text;

List<DialogueHistoryEntity> his = new List<DialogueHistoryEntity>();
DashScopeService service = new DashScopeService(String.Empty);
string? inputLine;
Console.WriteLine("Hello, 欢迎使用通义千问.NET版SDK，由MFJIANG https://github.com/mfjiang 贡献\r\n本SDK基于阿里DashScope API");
Console.WriteLine($"{DateTime.Now}\r\n  请向通义千问说点什么：");

Dialogue:
inputLine = "";
inputLine = Console.ReadLine()?.ToString();
if (!String.IsNullOrEmpty(inputLine))
{
    try
    {
        var rsp = await service.CallAigcText(his, $"{inputLine}");
        var rspobj = Newtonsoft.Json.JsonConvert.DeserializeObject<AigcTextResponseData>(rsp);
        if (rspobj != null)
        {
            his.Add(new DialogueHistoryEntity() { bot = rspobj.Output.Text, user = inputLine });
            Console.WriteLine($"{DateTime.Now} 通义千问： \r\n{rspobj.Output.Text}");
            Console.WriteLine("-----------------");
        }
        goto Dialogue;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"异常:{ex.Message}");
        goto Dialogue;
    }
}

