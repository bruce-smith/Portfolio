namespace Portfolio.Models
{
    public class gridItem
    {
        public string title { get; set; }
        public string discription { get; set; }
        public string imamgeScr { get; set; }
        public string imageAlt { get; set; }
        public string thumbImamgeScr { get; set; }
        public string thumbImageAlt { get; set; }
        public string link { get; set; }

        public gridItem(string titleI, string discriptionI, string imamgeScrI, string imageAltI, string thumbImamgeScrI, string thumbImageAlTI, string linkI)
        {
            title = titleI;
            discription = discriptionI;
            imamgeScr = imamgeScrI;
            imageAlt = imageAltI;
            thumbImamgeScr = thumbImamgeScrI;
            thumbImageAlt = thumbImageAlTI;
            link = linkI;
        }
        public gridItem(string titleI, string discriptionI, string imamgeScrI, string imageAltI, string linkI)
        {
            title = titleI;
            discription = discriptionI;
            imamgeScr = imamgeScrI;
            imageAlt = imageAltI;
            //LastIndexOf()
            //thumbImamgeScr = "t_"+imamgeScrI;
            thumbImamgeScr = imamgeScrI;
            thumbImageAlt = imageAltI;
            link = linkI;
        }
    }
}
