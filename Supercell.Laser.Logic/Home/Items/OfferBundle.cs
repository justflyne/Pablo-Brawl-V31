namespace Supercell.Laser.Logic.Home.Items
{
    using Supercell.Laser.Titan.DataStream;

    public class OfferBundle
    {
        public List<Offer> Items;
        public int Currency;
        public int Cost;
        public bool IsDailyDeals;
        public bool IsDailyOffer;
        public bool IsJsonOffer;
        public bool IsTrue;
        public bool Purchased;
        public DateTime StartTime;
        public DateTime EndTime;
        
        public bool IsLobbyPopupOffer;
        
        public int SaleStyle;
        public int SaleAmount;

        public int OldCost;

        public string Title;
        public string BackgroundExportName;
        public string Claim;

        public int State;
        
        public bool IsDailySkins;

        public OfferBundle()
        {
            Items = new List<Offer>();
            State = 0;
        }

        public void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(Items.Count);  // RewardCount
            foreach (Offer gemOffer in Items)
            {
                gemOffer.Encode(Stream);
            }

            Stream.WriteVInt(Currency); // currency

            Stream.WriteVInt(Cost); // cost

            TimeZoneInfo mskZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
            DateTime nowMsk = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mskZone);
            DateTime endTimeInMsk = TimeZoneInfo.ConvertTimeFromUtc(EndTime, mskZone);
            
            Stream.WriteVInt((int)(EndTime - nowMsk).TotalSeconds); // Seconds left

            Stream.WriteVInt(State); // State
            Stream.WriteVInt(0); // ??

            Stream.WriteBoolean(Purchased); // already bought

            Stream.WriteVInt(0); // ???

            Stream.WriteBoolean(IsDailyDeals); // is daily deals
            Stream.WriteVInt(OldCost); // Old cost???
            Stream.WriteInt(0); // ???
            Stream.WriteString(Title); // Name
            Stream.WriteBoolean(false); // вроде IsLobbyPopupOffer но не работает хз поч
            Stream.WriteString(BackgroundExportName);
            Stream.WriteVInt(0); // ???
            Stream.WriteBoolean(false);
            Stream.WriteVInt(SaleStyle); // SaleStyle ?
            Stream.WriteVInt(SaleAmount); // SaleAmount ?
        }
    }
}

