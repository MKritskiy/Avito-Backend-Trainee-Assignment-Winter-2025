namespace Application.Models.Dto;

public class CoinHistoryDto
{
    public List<RecievedTransferDto> Recieved {  get; set; }
    public List<SentTransferDto> Sent { get; set; }
}
